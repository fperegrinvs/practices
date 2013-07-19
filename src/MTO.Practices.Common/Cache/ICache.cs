namespace MTO.Practices.Cache
{
    using System;

    /// <summary>
    /// Interface para manipulação de cache de objetos
    /// </summary>
    public interface ICache
    {
        /// <summary>
        /// Tenta recuperar um valor do cache
        /// </summary>
        /// <typeparam name="T">Tipo de objeto armazenado</typeparam>
        /// <param name="key">chave do objeto</param>
        /// <param name="result">valor recuperado do cache</param>
        /// <returns>true caso a  operação seja bem sucedida, false em caso contrário</returns>
        bool TryGet<T>(string key, out T result);

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
        void Add<T>(string key, T value, TimeSpan duration);
    }
}
