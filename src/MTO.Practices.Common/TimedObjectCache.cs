namespace MTO.Practices.Common
{
    using System;
    using System.Runtime.Caching;

    /// <summary>
    /// Cache simples de objetos por tempo de expiração
    /// </summary>
    public class TimedObjectCache
    {
        /// <summary>
        /// Recupera objeto do cache pela chave
        /// </summary>
        /// <typeparam name="T">Tipo do objeto</typeparam>
        /// <param name="key">Chave do objeto</param>
        /// <returns>O objeto em cache ou o default() do seu tipo</returns>
        public static T Get<T>(string key)
        {
            return (T)(MemoryCache.Default.Get(key) ?? default(T));
        }

        /// <summary>
        /// Insere o objeto no cache
        /// </summary>
        /// <param name="key">Chave do objeto</param>
        /// <param name="obj">O objeto a ser cacheado</param>
        /// <param name="offset">O tempo de duração do objeto no cache</param>
        public static void Set(string key, object obj, DateTimeOffset offset)
        {
            MemoryCache.Default.Set(key, obj, offset);
        }
    }
}
