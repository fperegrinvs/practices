namespace MTO.Practices.Cache
{
    using System.Configuration;

    using MTO.Practices.Common;

    /// <summary>
    /// Helper para uso do cache ativo
    /// </summary>
    public class ActiveCache
    {
        /// <summary>
        /// Instância do cache ativo.
        /// </summary>
        private static IActiveCache cacheInstance = null;

        /// <summary>
        /// Initializes static members of the <see cref="ActiveCache"/> class. 
        /// </summary>
        static ActiveCache()
        {
            UseRemoteCache = ConfigurationManager.AppSettings.Get("RemoteCacheEnabled") == "true";
            UseLocalCache = ConfigurationManager.AppSettings.Get("LocalCacheEnabled") == "true";

            int interval;
            if (int.TryParse(ConfigurationManager.AppSettings.Get("CacheTime"), out interval))
            {
                CacheTime = interval;
            }

            LocalCacheTime = int.TryParse(ConfigurationManager.AppSettings.Get("LocalCacheTime"), out interval) ? interval : 60;
        }

        /// <summary>
        /// Indica se o cache remoto deve ser utilizado
        /// </summary>
        public static bool UseRemoteCache { get; private set; }

        /// <summary>
        /// Indica se o cache local deve ser utilizado
        /// </summary>
        public static bool UseLocalCache { get; private set; }

        /// <summary>
        /// Tempo de expiração de cache usado para conteúdos diversos, em segundos
        /// </summary>
        public static int? CacheTime { get; private set; }

        /// <summary>
        /// Tempo de expiração de cache local, em segundos
        /// </summary>
        public static int LocalCacheTime { get; private set; }

        /// <summary>
        /// Singleton para instância do cache ativo
        /// </summary>
        public static IActiveCache Instance
        {
            get
            {
                return cacheInstance ?? (cacheInstance = Injector.ResolveInterface<IActiveCache>());
            }
            internal set
            {
                cacheInstance = value;
            }
        }
    }
}
