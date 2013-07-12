namespace MTO.Practices.Common
{
    using System;
    using System.Linq;

    /// <summary>
    /// Gerencia uma unidade de trabalho de persistência
    /// </summary>
    public class UnitOfWork : IDisposable
    {
        /// <summary>
        /// Unit of work atual desta thread
        /// </summary>
        [ThreadStatic]
        private static UnitOfWork uow;

        /// <summary>
        /// Marca se a unidade de trabalho já foi consolidada.
        /// </summary>
        private bool committed = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="UnitOfWork"/> class. 
        /// Inicia uma unidade de trabalho de persistência. Somente chamar dentro de using().
        /// </summary>
        public UnitOfWork()
        {
            uow = this;
        }

        /// <summary>
        /// Delegate de consolidação da unidade de trabalho
        /// </summary>
        public delegate void ConsolidateEventHandler();

        /// <summary>
        /// Delegate de rollback da unidade de trabalho
        /// </summary>
        public delegate void RollbackEventHandler();

        /// <summary>
        /// Delegate de dispose da unidade de trabalho
        /// </summary>
        public delegate void DisposeEventHandler();

        /// <summary>
        /// Evento de consolidação da unidade de trabalho
        /// </summary>
        public event ConsolidateEventHandler OnConsolidate;

        /// <summary>
        /// Evento de rollback da unidade de trabalho
        /// </summary>
        public event RollbackEventHandler OnRollback;

        /// <summary>
        /// Evento de dispose da unidade de trabalho
        /// </summary>
        public event DisposeEventHandler OnDispose;

        /// <summary>
        /// Recupera a unidade de trabalho atual, se houver uma
        /// </summary>
        public static UnitOfWork Current
        {
            get
            {
                return uow;
            }
        }

        /// <summary>
        /// Indica se uma Unit of Work está em andamento
        /// </summary>
        public static bool InProgress
        {
            get
            {
                return uow != null;
            }
        }

        /// <summary>
        /// Singleton de chaveamento para testes unitários.
        /// É feio, eu sei. Em breve eu crio um UnitOfWork mockavel.
        /// </summary>
        public static bool TestMode { get; set; }

        /// <summary>
        /// Consolida o trabalho realizado dentro da unidade de trabalho.
        /// Se nenhum trabalho foi realizado, lança uma exception.
        /// </summary>
        public void Commit()
        {
            if (this.OnConsolidate != null)
            {
                this.OnConsolidate();
                this.committed = true;
            }
            else
            {
                if (!TestMode)
                {
                    throw new Exception("Unit of Work foi utilizado sem executar nenhuma operação de persistência.");
                }
            }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        public void Dispose()
        {
            // Se não fizemos commit da unidade de trabalho damos rollback
            if (!this.committed)
            {
                if (this.OnRollback != null)
                {
                    this.OnRollback();
                }
                else
                {
                    // A perda de dados só ocorre se registramos um datacontext, que irá escutar o OnConsolidate.
                    if (!TestMode && !ReadOnlyContext.InProgress && this.OnConsolidate != null)
                    {
                        throw new Exception(
                            "Unidade de trabalho foi eliminada sem commit e não há handlers de Rollback. Possível perda de dados iminente.");
                    }
                }
            }

            uow = null;
            if (this.OnDispose != null)
            {
                this.OnDispose();
            }
            else
            {
                // A inconsistencia no tratamento de unidades de trabalho só ocorre se registramos um datacontext, que irá escutar o OnConsolidate.
                if (!TestMode && !ReadOnlyContext.InProgress && this.OnConsolidate != null)
                {
                    throw new Exception(
                        "Unidade de trabalho foi eliminada mas não há handlers de Dispose. Possível inconsistência no tratamento de futuras unidades de trabalho nesta mesma Thread.");
                }
            }
        }
    }
}
