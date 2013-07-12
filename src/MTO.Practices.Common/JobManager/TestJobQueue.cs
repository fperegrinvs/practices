namespace MTO.Practices.Common.JobManager
{
    /// <summary>
    /// Fila de teste com implementação padrão e parâmetros simples
    /// </summary>
    public class TestJobQueue : JobQueue
    {
        /// <summary>
        /// The instance.
        /// </summary>
        private static TestJobQueue instance;

        /// <summary>
        /// Gets the instance.
        /// </summary>
        public static TestJobQueue Instance
        {
            get
            {
                return instance ?? (instance = new TestJobQueue());
            }
        }

        /// <summary>
        /// Nome da Fila
        /// </summary>
        public override string Name
        {
            get
            {
                return "Testes";
            }
        }

        /// <summary>
        /// Quantidade de jobs simultaneos
        /// </summary>
        public override int MaxParalelism
        {
            get
            {
                return 1;
            }
        }
    }
}
