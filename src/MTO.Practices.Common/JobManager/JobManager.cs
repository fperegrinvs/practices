namespace MTO.Practices.Common.JobManager
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Gerenciador de jobs em memória
    /// </summary>
    public class JobManager : InstanceComponent<JobManager>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="JobManager"/> class.
        /// </summary>
        public JobManager()
        {
            this.Queues = new List<JobQueue>();
        }

        /// <summary>
        /// A lista de filas de jobs
        /// </summary>
        private List<JobQueue> Queues { get; set; }

        /// <summary>
        /// Recupera um job pelo seu identificador único
        /// </summary>
        /// <param name="id">Id do job</param>
        /// <returns>O job</returns>
        public Job FindJob(Guid id)
        {
            return this.Queues.Select(x => x.FindJob(id)).SingleOrDefault(x => x != null);
        }

        /// <summary>
        /// Retorna a lista de filas de jobs
        /// </summary>
        /// <returns>A lista de filas de job</returns>
        public List<JobQueue> GetQueues()
        {
            return this.Queues.ToArray().ToList();
        }

        /// <summary>
        /// Inicia uma iteração de verificação de jobs que podem rodar
        /// </summary>
        public void Go()
        {
            var jobs = this.GetStartableJobs();
            
            foreach (var job in jobs)
            {
                job.JobDoneEvent += delegate { this.Go(); };
                job.Start();
            }
        }

        /// <summary>
        /// Registra uma fila de jobs
        /// </summary>
        /// <param name="jobQueue">A fila de jobs</param>
        internal void RegisterQueue(JobQueue jobQueue)
        {
            if (this.Queues.Any(x => x.Name == jobQueue.Name))
            {
                throw new InvalidOperationException("Fila já registrada: " + jobQueue.Name);
            }

            this.Queues.Add(jobQueue);
        }

        /// <summary>
        /// Retorna jobs que podem ser startados
        /// </summary>
        /// <returns>Os jobs iniciáveis de todas as filas</returns>
        private IEnumerable<Job> GetStartableJobs()
        {
            return this.Queues.SelectMany(x => x.GetStartableJobs());
        }
    }
}
