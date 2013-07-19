namespace MTO.Practices.Common.Wrappers
{
    using System;
    using System.Linq.Expressions;

    /// <summary>
    /// The Wrapper interface.
    /// </summary>
    /// <typeparam name="T">
    /// Tipo da interface utilizada pelo wrapper
    /// </typeparam>
    public interface IWrapper<T> where T : class
    {
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
        TU Invoke<TU>(Expression<Func<T, TU>> method, WrapperOptions options = null);

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
        void Invoke(Expression<Action<T>> method, WrapperOptions options = null);
    }
}
