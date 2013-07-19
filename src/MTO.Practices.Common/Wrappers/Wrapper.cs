namespace MTO.Practices.Common.Wrappers
{
    using System;
    using System.Linq.Expressions;

    using MTO.Practices.Common;

    /// <summary>
    /// The wrapper.
    /// </summary>
    public static class Wrapper<T> where T : class
    {
        /// <summary>
        /// Invoca método com retorno
        /// </summary>
        /// <param name="method">
        /// Método a ser invocado
        /// </param>
        /// <param name="options">
        /// The options.
        /// </param>
        /// <typeparam name="T">
        /// Tipo da interface
        /// </typeparam>
        /// <typeparam name="TU">
        /// Tipo do retorno
        /// </typeparam>
        /// <returns>
        /// Tipo da interface utilizada
        /// </returns>
        public static TU Invoke<TU>(Expression<Func<T, TU>> method, WrapperOptions options = null)
        {
            var wrapper = Injector.ResolveInterface<IWrapper<T>>();
            return wrapper.Invoke(method, options);
        }

        /// <summary>
        /// Invoca um método ser retorno
        /// </summary>
        /// <param name="method">
        /// Método a ser invocado
        /// </param>
        /// <param name="options">
        /// The options.
        /// </param>
        /// <typeparam name="T">
        /// Tipo de interface utilizada
        /// </typeparam>
        public static void Invoke(Expression<Action<T>> method, WrapperOptions options = null)
        {
            var wrapper = Injector.ResolveInterface<IWrapper<T>>();
            wrapper.Invoke(method, options);
        }
    }
}
