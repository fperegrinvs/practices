namespace MTO.Practices.Common.JobManager
{
    using System;

    /// <summary>
    /// Evento de Job Concluído
    /// </summary>
    public class JobDoneEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="JobDoneEventArgs"/> class.
        /// </summary>
        /// <param name="job">
        /// The job.
        /// </param>
        public JobDoneEventArgs(Job job)
        {
            this.Job = job;
        }

        /// <summary>
        /// Gets or sets the job.
        /// </summary>
        public Job Job { get; set; }
    }
}
