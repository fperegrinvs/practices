namespace MTO.Practices.Common.Wrappers
{
    using System;
    using System.Linq.Expressions;

    using MTO.Practices.Cache;
    using MTO.Practices.Common.Cache;

    /// <summary>
    /// Wrapper com cache
    /// </summary>
    /// <typeparam name="T">tipo da interface implementada</typeparam>
    public class CachedWrapper<T> : IWrapper<T> where T : class
    {
        /// <summary>
        /// The wrapped instance
        /// </summary>
        private readonly T instance;

        /// <summary>
        /// gestor de cache a ser utilizado
        /// </summary>
        private ICache cache;

        /// <summary>
        /// duração do cache
        /// </summary>
        private TimeSpan duration;

        /// <summary>
        /// Initializes a new instance of the <see cref="CachedWrapper{T}"/> class.
        /// </summary>
        /// <param name="cacheManager">
        /// The cache manager.
        /// </param>
        /// <param name="cacheDuration">
        /// The cache Duration.
        /// </param>
        public CachedWrapper(T instance, ICache cacheManager, TimeSpan cacheDuration)
        {
            this.instance = instance;
            this.cache = cacheManager;
            this.duration = cacheDuration;
        }

        #region Implementation of IWrapper<T>

        /// <summary>
        /// Invoca método com retorno
        /// </summary>
        /// <param name="method">
        /// Método a ser invocado
        /// </param>
        /// <param name="options">Configurações do wrapper</param>
        /// <typeparam name="TU">
        /// Tipo do retorno
        /// </typeparam>
        /// <returns>
        /// Tipo da interface utilizada
        /// </returns>
        public TU Invoke<TU>(Expression<Func<T, TU>> method, WrapperOptions options = null)
        {
            var key = CacheKey.ExpressionKey(method);
            TU result;
            if (!this.cache.TryGet(key, out result))
            {
                result = method.Compile()(this.instance);
                this.cache.Add(key, result, this.duration);
            }

            return result;
        }

        /// <summary>
        /// Invoca um método ser retorno
        /// </summary>
        /// <param name="method">
        /// Método a ser invocado
        /// </param>
        /// <param name="options">Configurações do wrapper</param>
        /// <typeparam name="T">
        /// Tipo de interface utilizada
        /// </typeparam>
        public void Invoke(Expression<Action<T>> method, WrapperOptions options = null)
        {
            method.Compile()(this.instance);
        }

        #endregion
    }
}
