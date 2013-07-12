namespace MTO.Practices.Common.JobManager
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;

    /// <summary>
    /// Fila abstrata de jobs
    /// </summary>
    public abstract class JobQueue
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="JobQueue"/> class.
        /// </summary>
        protected JobQueue()
        {
            this.WaitList = new Queue<Job>();
            this.DoneOrDoing = new List<Job>();
            this.MaxHistory = ConfigurationManager.AppSettings["MaxJobHistory"].ToIntNullable() ?? 5;

            JobManager.Instance.RegisterQueue(this);
        }

        /// <summary>
        /// Nome da Fila
        /// </summary>
        public abstract string Name { get; }

        /// <summary>
        /// Quantidade de jobs simultaneos
        /// </summary>
        public abstract int MaxParalelism { get; }

        /// <summary>
        /// Tamanho máximo do histórico de jobs
        /// </summary>
        protected int MaxHistory { get; private set; }

        /// <summary>
        /// A fila de jobs
        /// </summary>
        protected Queue<Job> WaitList { get; set; }

        /// <summary>
        /// Lista de jobs sendo rodados atualmente
        /// </summary>
        protected List<Job> DoneOrDoing { get; set; }

        /// <summary>
        /// Enfileira uma Action para ser executada pelo JobManager
        /// </summary>
        /// <param name="key">Chave do job</param>
        /// <param name="action">A action</param>
        /// <param name="owner">O usuário dono do job</param>
        /// <returns>O ID único do Job</returns>
        public Guid Enqueue(string key, Action action, string owner)
        {
            var job = new Job(key, owner);

            job.SetOperation(action);

            this.WaitList.Enqueue(job);

            JobManager.Instance.Go();

            return job.Id;
        }

        /// <summary>
        /// Enfileira uma Action para ser executada pelo JobManager
        /// </summary>
        /// <param name="key">Chave do job</param>
        /// <param name="action">A action</param>
        /// <param name="owner">O usuário dono do job</param>
        /// <returns>O ID único do Job</returns>
        public Guid Enqueue(string key, Action<Action<string>> action, string owner)
        {
            var job = new Job(key, owner);

            // Criamos a ação de logar que passa strings ao job.Log
            var loggingAction = new Action<string>(str => job.Log.Add(str));

            // Atribuímos a ação à uma nova ação na recebida
            var actionWithLogging = new Action(() => action(loggingAction));

            // Startamos a ação com logging
            job.SetOperation(actionWithLogging);

            this.WaitList.Enqueue(job);

            JobManager.Instance.Go();

            return job.Id;
        }

        /// <summary>
        /// Enfileira uma Função que retorna valor para ser executada pelo JobManager
        /// </summary>
        /// <typeparam name="TResult">O tipo de retorno da função</typeparam>
        /// <param name="key">Chave do job</param>
        /// <param name="func">A função</param>
        /// <param name="owner">O usuário dono do job</param>
        /// <returns>O ID único do Job</returns>
        public Guid Enqueue<TResult>(string key, Func<TResult> func, string owner)
        {
            var job = new Job(key, owner);

            job.SetOperation(func);

            this.WaitList.Enqueue(job);

            JobManager.Instance.Go();

            return job.Id;
        }

        /// <summary>
        /// Recupera uma cópia da lista de jobs em espera
        /// </summary>
        /// <returns>A lista de espera</returns>
        public List<Job> GetWaitList()
        {
            return this.WaitList.ToList();
        }

        /// <summary>
        /// Recupera uma cópia da lista de jobs executados ou em execução
        /// </summary>
        /// <returns>A lista de executados/em execução</returns>
        public List<Job> GetDoneOrDoing()
        {
            return this.DoneOrDoing.ToArray().ToList();
        }

        /// <summary>
        /// Retorna os jobs que podem ser iniciados
        /// </summary>
        /// <returns>Os jobs que podem ser iniciados</returns>
        public virtual List<Job> GetStartableJobs()
        {
            var doingCount = this.DoneOrDoing.Count(x => !x.Done);
            if (doingCount < this.MaxParalelism)
            {
                return this.Dequeue(this.MaxParalelism - doingCount).Where(x => x != null).ToList();
            }

            return new List<Job>();
        }

        /// <summary>
        /// Retorna o job com o id definido
        /// </summary>
        /// <param name="id">O id</param>
        /// <returns>O job</returns>
        public Job FindJob(Guid id)
        {
            return this.WaitList.Union(this.DoneOrDoing).SingleOrDefault(x => x.Id == id);
        }

        /// <summary>
        /// Remove da fila um ou mais jobs
        /// </summary>
        /// <param name="count"> Quantidade de itens para remover da fila</param>
        /// <returns>O job</returns>
        protected IEnumerable<Job> Dequeue(int count)
        {
            for (int i = 0; i < count; i++)
            {
                yield return this.Dequeue();
            }
        }

        /// <summary>
        /// Remove da fila um job
        /// </summary>
        /// <returns>O job</returns>
        protected Job Dequeue()
        {
            if (this.WaitList.Count == 0)
            {
                return null;
            }

            var job = this.WaitList.Dequeue();
            this.DoneOrDoing.Add(job);

            job.JobDoneEvent += delegate { this.CleanupHistory(); };

            return job;
        }

        /// <summary>
        /// Limpa o histórico, respeitando tamanho máximo configurado
        /// </summary>
        private void CleanupHistory()
        {
            var curHistory = this.DoneOrDoing.Count(x => x.Done);
            if (curHistory > this.MaxHistory)
            {
                var dropList = this.DoneOrDoing.OrderBy(y => y.StartDate).Take(curHistory - this.MaxHistory);
                this.DoneOrDoing.RemoveAll(dropList.Contains);
            }
        }
    }
}
