namespace MTO.Practices.Common.Wrappers
{
    using System;
    using System.Linq.Expressions;

    using MTO.Practices.Common.WCF;

    /// <summary>
    /// The simple wcf wrapper.
    /// </summary>
    /// <typeparam name="T">
    /// Tipo do serviço
    /// </typeparam>
    public class SimpleWcfWrapper<T> : ManualServiceBase<T>, IWrapper<T> where T : class
    {
        private WrapperOptions pOptions = null;

        public SimpleWcfWrapper(WrapperOptions options = null)
        {
            this.pOptions = options;
        }

        #region Implementation of IWrapper<U>

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
            if (options == null && this.pOptions != null)
            {
                options = this.pOptions;
            }

            return this.InvokeService(method.Compile(), options);
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
            if (options == null && this.pOptions != null)
            {
                options = this.pOptions;
            }

            this.InvokeService(method.Compile(), options);
        }

        #endregion
    }
}
