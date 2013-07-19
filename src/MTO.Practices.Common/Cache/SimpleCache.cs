namespace MTO.Practices.Cache
{
    using System;
    using System.Runtime.Caching;

    /// <summary>
    /// Implementação de cache simples, em memória
    /// </summary>
    public class SimpleCache : ICache
    {
        /// <summary>
        /// Cache em memória
        /// </summary>
        private static readonly ObjectCache Cache = MemoryCache.Default;

        #region Implementation of ICache

        /// <summary>
        /// Tenta recuperar um valor do cache
        /// </summary>
        /// <typeparam name="T">Tipo de objeto armazenado</typeparam>
        /// <param name="key">chave do objeto</param>
        /// <param name="result">valor recuperado do cache</param>
        /// <returns>true caso a  operação seja bem sucedida, false em caso contrário</returns>
        public bool TryGet<T>(string key, out T result)
        {
            try
            {
                if (Cache.Contains(key))
                {
                    result = (T)Cache.Get(key);
                    return true;
                }

                result = default(T);
                return false;
            }
            catch (Exception)
            {
                result = default(T);
                return false;
            }
        }

        /// <summary>
        /// Adiciona objeto ao cache
        /// </summary>
        /// <param name="key">
        /// chave do cache
        /// </param>
        /// <param name="value">
        /// Valor a ser armazenado em cache
        /// </param>
        /// <param name="duration">
        /// Duração do cache
        /// </param>
        /// <typeparam name="T">
        /// Tipo de objeto do cache
        /// </typeparam>
        public void Add<T>(string key, T value, TimeSpan duration)
        {
            Cache.Add(key, value, DateTime.Now.Add(duration));
        }

        #endregion
    }
}
