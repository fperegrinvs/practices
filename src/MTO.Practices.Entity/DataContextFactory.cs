namespace MTO.Practices.Common
{
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;

    /// <summary>
    /// Classe de costrução e gerenciamento de DbContexts
    /// </summary>
    /// <typeparam name="TFactoryType"> A classe do contexto que será fabricado </typeparam>
    public abstract class DataContextFactory<TFactoryType> : DbContext, IDataContext where TFactoryType : class, IDataContext, new()
    {
        /// <summary>
        /// tracking de objetos
        /// </summary>
        private bool trackingEnabled = true;

        /// <summary>
        /// Initializes a new instance of the <see cref="DataContextFactory{TFactoryType}"/> class.
        /// </summary>
        /// <param name="namedConnection"> The named Connection. </param>
        public DataContextFactory(string namedConnection) : base(namedConnection)
        {
        }

        /// <summary>
        /// Recupera o DataContext atual
        /// </summary>
        public static TFactoryType Current
        {
            get
            {
                // contexto apenas para leitura ativo, um contexto de leitura sobrescreve um unit of work
                if (ReadOnlyContext.InProgress)
                {
                    if (RocDataContext == null)
                    {
                        ReadOnlyContext.Current.OnDispose += delegate { Current.TrackingEnabled = true; };
                        RocDataContext = new TFactoryType { TrackingEnabled = false };
                    }

                    return RocDataContext;
                }

                // Se estamos trabalhando
                if (UnitOfWork.InProgress)
                {
                    // Se ainda geramos context para essa UoW, também precisamos cadastrar o 
                    if (UowDataContext == null)
                    {
                        // Quando a Unit completar, chamamos o código de consolidação do trabalho (em tese apenas um SaveChanges)
                        UnitOfWork.Current.OnConsolidate += delegate { Current.ConsolidateUnitOfWork(); };
                        UnitOfWork.Current.OnRollback += delegate { Current.RollbackUnitOfWork(); };
                        UnitOfWork.Current.OnDispose += delegate { UowDataContext = null; };

                        return UowDataContext = new TFactoryType();
                    }

                    return UowDataContext;
                }

                // Se estivermos fora de Unit of Work, usamos o DataContext compartilhado da Thread
                return SharedDataContext ?? (SharedDataContext = new TFactoryType());
            }
        }

        /// <summary>
        /// Chave de armazenamento dos DbContext compartilhados no contexto da aplicação
        /// </summary>
        private static string StorageKey
        {
            get
            {
                return typeof(TFactoryType).Namespace;
            }
        }

        /// <summary>
        /// DataContext compartilhado do contexto atual (Request/Thread)
        /// </summary>
        private static TFactoryType SharedDataContext
        {
            get
            {
                return Context.Current.Get<TFactoryType>(StorageKey + "_sharedDataContext");
            }

            set
            {
                Context.Current.Set(StorageKey + "_sharedDataContext", value);
            }
        }

        /// <summary>
        /// DataContext compartilhado do Unit of Work
        /// </summary>
        private static TFactoryType RocDataContext
        {
            get
            {
                return Context.Current.Get<TFactoryType>(StorageKey + "_rocDataContext");
            }

            set
            {
                Context.Current.Set(StorageKey + "_rocDataContext", value);
            }
        }

        /// <summary>
        /// DataContext compartilhado do Unit of Work
        /// </summary>
        private static TFactoryType UowDataContext
        {
            get
            {
                return Context.Current.Get<TFactoryType>(StorageKey + "_uowDataContext");
            }

            set
            {
                Context.Current.Set(StorageKey + "_uowDataContext", value);
            }
        }

        /// <summary>
        /// Salva as alterações caso não estejamos num contexto de Unit of Work
        /// </summary>
        /// <returns>Zero caso estejamos em UoW, número de rows alteradas caso contrário</returns>
        public override int SaveChanges()
        {
            if (!UnitOfWork.InProgress)
            {
                return base.SaveChanges();
            }

            return 0;
        }

        /// <summary>
        /// Consolida unidade de trabalho (roda SaveChanges do DbContext)
        /// </summary>
        public void ConsolidateUnitOfWork()
        {
            base.SaveChanges();
        }

        /// <summary>
        /// Desatrela a entidade deste context, matando todas as suas navigation properties e efetivamente desligando o objeto do DbContext.
        /// Necessário quando jogamos entidades pra lá e pra cá através de threads.
        /// </summary>
        /// <param name="entity">A entidade</param>
        public void Detach(object entity)
        {
            ((IObjectContextAdapter)this).ObjectContext.Detach(entity);
        }

        /// <summary>
        /// Consolida unidade de trabalho (roda SaveChanges do DbContext)
        /// </summary>
        public void RollbackUnitOfWork()
        {
            // Nada precisa ser feito no rollback para o DataContext
            // From : http://msdn.microsoft.com/en-us/library/bb336792.aspx

            // "SaveChanges operates within a transaction. 
            // SaveChanges will roll back that transaction and throw an exception 
            // if any of the dirty ObjectStateEntry objects cannot be persisted. "
        }

        /// <summary>
        /// Ativa e desabilita o tracking de objetos
        /// </summary>
        public bool TrackingEnabled
        {
            get
            {
                return this.trackingEnabled;
            }

            set
            {
                if (this.trackingEnabled != value)
                {
                    this.trackingEnabled = value;
                    this.UpdateTrackingStatus(value);
                }
            }
        }

        /// <summary>
        /// Atualiza estado de tracking do contexto
        /// </summary>
        /// <param name="enabled">indica se o tracking está ligado ou desligado</param>
        protected virtual void UpdateTrackingStatus(bool enabled)
        {
            this.Configuration.AutoDetectChangesEnabled = enabled;
            this.Configuration.ProxyCreationEnabled = enabled;
        }
    }
}
