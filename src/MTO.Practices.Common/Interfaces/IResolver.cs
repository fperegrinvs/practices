namespace MTO.Practices.Common.Interfaces
{
    using System;

    /// <summary>
    /// interface para resolução de interfaces
    /// </summary>
    public interface IResolver
    {
        /// <summary>
        /// Resolve uma interface
        /// </summary>
        /// <typeparam name="T">tipo da interface</typeparam>
        /// <param name="name">nome do container</param>
        /// <returns>instância que implementa a interface</returns>
        T Resolve<T>(string name = null);

        /// <summary>
        /// Registra mapeamento de inversão de dependencia
        /// </summary>
        /// <typeparam name="TService">
        /// typo a ser registrado
        /// </typeparam>
        /// <param name="name">
        /// Nome do container
        /// </param>
        /// <param name="factory">
        /// instrução para se criar uma instância
        /// </param>
        void Register<TService>(string name, Func<TService> factory) where TService : class;
    }
}
