namespace MTO.Practices.Common.Wrappers
{
    using System;
    using System.Linq.Expressions;

    /// <summary>
    /// The transparent wrapper.
    /// </summary>
    /// <typeparam name="T">
    /// Tipo da interface implementada
    /// </typeparam>
    public class TransparentWrapper<T> : IWrapper<T> where T : class
    {
        /// <summary>
        /// The wrapped instance
        /// </summary>
        private readonly T instance;

        /// <summary>
        /// Initializes a new instance of the <see cref="TransparentWrapper{T}"/> class.
        /// </summary>
        /// <param name="instance">
        /// The instance.
        /// </param>
        public TransparentWrapper(T instance)
        {
            this.instance = instance;
        }

        #region Implementation of IWrapper<T>

        /// <summary>
        /// Invoca método com retorno
        /// </summary>
        /// <param name="method">
        /// Método a ser invocado
        /// </param>
        /// <param name="options">
        /// The options.
        /// </param>
        /// <typeparam name="TU">
        /// Tipo do retorno
        /// </typeparam>
        /// <returns>
        /// Tipo da interface utilizada
        /// </returns>
        public TU Invoke<TU>(Expression<Func<T, TU>> method, WrapperOptions options = null)
        {
            return method.Compile()(this.instance);
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
        public void Invoke(Expression<Action<T>> method, WrapperOptions options = null)
        {
            method.Compile()(this.instance);
        }

        #endregion
    }
}
