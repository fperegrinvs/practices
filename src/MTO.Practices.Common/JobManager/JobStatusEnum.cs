namespace MTO.Practices.Common.JobManager
{
    using System.ComponentModel;

    /// <summary>
    /// Status de Jobs
    /// </summary>
    public enum JobStatusEnum
    {
        /// <summary>
        /// Job aguardando a vez
        /// </summary>
        [Description("Aguardando")]
        Waiting,
        
        /// <summary>
        /// Job em execução
        /// </summary>
        [Description("Em execução")]
        Executing,
        
        /// <summary>
        /// Job concluído com sucesso
        /// </summary>
        [Description("Concluído com sucesso")]
        Done,
        
        /// <summary>
        /// Job estourou exception e não finalizou
        /// </summary>
        [Description("Concluído com erro")]
        Error
    }
}
