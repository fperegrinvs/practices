namespace MTO.Practices.Common
{
    /// <summary>
    /// Classe helper usada para fazer DI do logger.
    /// </summary>
    public static class Logger
    {
        /// <summary>
        /// variável usada para armazenar valor do singleton Instance
        /// </summary>
        private static ILogger logger;

        /// <summary>
        /// Retorna instancia padrão do logger
        /// </summary>
        public static ILogger Instance
        {
            get
            {
                if (logger == null)
                {
                    logger = Injector.ResolveInterface<ILogger>("Manytoone.Logger").GetDefaultInstance();
                }

                return logger;
            }

            set
            {
                logger = value;
            }
        }
    }
}
