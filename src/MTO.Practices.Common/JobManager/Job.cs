namespace MTO.Practices.Common.JobManager
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Representa um job do JobManager
    /// </summary>
    public class Job
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Job"/> class.
        /// </summary>
        /// <param name="key">The key</param>
        /// <param name="owner"> The owner. </param>
        internal Job(string key, string owner)
        {
            this.Key = key;
            this.Id = Guid.NewGuid();
            this.Log = new List<string>();
            this.Owner = owner;
        }

        /// <summary>
        /// Evento de conclusão do job
        /// </summary>
        public event EventHandler<JobDoneEventArgs> JobDoneEvent;

        /// <summary>
        /// O id do job
        /// </summary>
        public Guid Id { get; internal set; }

        /// <summary>
        /// Chave contextual do Job, identifica um job dentro de uma Queue
        /// </summary>
        public string Key { get; internal set; }

        /// <summary>
        /// O usuário que iniciou o job
        /// </summary>
        public string Owner { get; private set; }

        /// <summary>
        /// Mensagens de log enviadas pelo job durante o gerenciamento
        /// </summary>
        public IList<string> Log { get; set; }

        /// <summary>
        /// O resultado da execução do método
        /// </summary>
        public object Result { get; set; }

        /// <summary>
        /// Status de conclusão do job
        /// </summary>
        public bool Done { get; private set; }

        /// <summary>
        /// Status de erro do job
        /// </summary>
        public Exception Error { get; private set; }

        /// <summary>
        /// Data de início do job
        /// </summary>
        public DateTime StartDate { get; private set; }

        /// <summary>
        /// Data de conclusão do job
        /// </summary>
        public DateTime EndDate { get; private set; }

        /// <summary>
        /// O tempo de execução do job em segundos
        /// </summary>
        public TimeSpan TempoDeExecucao
        {
            get
            {
                if (this.Done.Equals(false))
                {
                    return new TimeSpan();
                }

                return this.EndDate.Subtract(this.StartDate);
            }
        }

        /// <summary>
        /// Status do job
        /// </summary>
        public JobStatusEnum Status
        {
            get
            {
                if (this.Done)
                {
                    if (this.Error == null)
                    {
                        return JobStatusEnum.Done;
                    }

                    return JobStatusEnum.Error;
                }

                if (this.StartDate == DateTime.MinValue)
                {
                    return JobStatusEnum.Waiting;
                }

                return JobStatusEnum.Executing;
            }
        }

        /// <summary>
        /// O método a ser executado
        /// </summary>
        private Action Operation { get; set; }

        /// <summary>
        /// Define a operação que será executada pelo job
        /// </summary>
        /// <param name="action">Uma Action</param>
        public void SetOperation(Action action)
        {
            var userId = Context.Current.GetCurrentUserId();
            var userName = Context.Current.GetCurrentUserName();

            this.Operation = delegate
            {
                try
                {
                    Context.SetContext(new ThreadContext());

                    Context.Current.SetAuthenticated(userId, userName, string.Empty);

                    // cadastrar listener pra capturar as mensagens de log
                    Logger.Instance.Chain(new JobLogger(this.Log), threadOnly: true);

                    action();
                }
                catch (Exception ex)
                {
                    this.Error = ex;
                    Logger.Instance.LogError(new Exception("Erro ao rodar job " + this.Id, ex));
                }
                finally
                {
                    // descadastrar listener, de outra forma eles vazam pela threadpool
                    Logger.Instance.ClearChain(threadOnly: true);

                    // Limpamos o context para essa thread, como vai pro Pool, não há garantia de que estará limpo para a próxima thread
                    Context.Clear();
                }
            };
        }

        /// <summary>
        /// Define a operação que será executada pelo job
        /// </summary>
        /// <typeparam name="TResult">O tipo do resultado da operação</typeparam>
        /// <param name="func">Uma função com retorno</param>
        public void SetOperation<TResult>(Func<TResult> func)
        {
            var userId = Context.Current.GetCurrentUserId();
            var userName = Context.Current.GetCurrentUserName();

            this.Operation = delegate
            {
                try
                {
                    Context.SetContext(new ThreadContext());

                    Context.Current.SetAuthenticated(userId, userName, string.Empty);

                    // cadastrar listener pra capturar as mensagens de log
                    Logger.Instance.Chain(new JobLogger(this.Log), threadOnly: true);

                    this.Result = func();
                }
                catch (Exception ex)
                {
                    this.Error = ex;
                    Logger.Instance.LogError(new Exception("Erro ao rodar job " + this.Id, ex));
                }
                finally
                {
                    // descadastrar listener, de outra forma eles vazam pela threadpool
                    Logger.Instance.ClearChain(threadOnly: true);

                    // Limpamos o context para essa thread, como vai pro Pool, não há garantia de que estará limpo para a próxima thread
                    Context.Clear();
                }
            };
        }

        /// <summary>
        /// Formatação em string do job, indicando todos os seus campos de forma textual
        /// </summary>
        /// <returns>O job descrito em string</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine(FormatLine("Id", this.Id));
            sb.AppendLine(FormatLine("Chave", this.Key));
            sb.AppendLine(FormatLine("Dono", this.Owner));
            sb.AppendLine(FormatLine("Operação", this.Operation));
            sb.AppendLine(FormatLine("Log", this.Log.Count));
            sb.AppendLine(FormatLine("Concluído", this.Done));
            sb.AppendLine(FormatLine("Tempo de Execução (segundos)", this.TempoDeExecucao.TotalSeconds));
            sb.AppendLine(FormatLine("Data início", this.StartDate.ToShortTimeString()));
            sb.AppendLine(FormatLine("Data Término", this.EndDate.ToShortTimeString()));
            return sb.ToString();
        }

        /// <summary>
        /// Executa o job
        /// </summary>
        internal void Start()
        {
            if (this.Operation == null)
            {
                throw new Exception("Job iniciado sem método definido.");
            }

            this.StartDate = DateTime.Now;
            Task.Factory.StartNew(this.Operation).ContinueWith(this.MarkAsDone);
        }

        /// <summary>
        /// Formata linha para exibicao no ToString
        /// </summary>
        /// <param name="label">Rotulo da linha</param>
        /// <param name="obj">Obj com valo da linha</param>
        /// <returns>A linha formatada</returns>
        private static string FormatLine(string label, object obj)
        {
            const int Tamanho = 40;
            return label + ":".PadRight(Tamanho, '.').Insert(Tamanho - obj.ToString().Length, obj.ToString()).Remove(Tamanho);
        }

        /// <summary>
        /// Marca o job como concluído e avisa quem estiver ouvindo
        /// </summary>
        /// <param name="ar">The Async Result</param>
        private void MarkAsDone(Task ar)
        {
            this.EndDate = DateTime.Now;
            this.Done = true;

            if (this.JobDoneEvent != null)
            {
                this.JobDoneEvent(this, new JobDoneEventArgs(this));
            }
        }
    }
}
