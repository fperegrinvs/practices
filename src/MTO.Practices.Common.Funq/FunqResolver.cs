namespace MTO.Practices.Common.Funq
{
    using System;

    using global::Funq;

    using MTO.Practices.Common.Interfaces;

    /// <summary>
    /// Resolver usando o Funq
    /// </summary>
    public class FunqResolver : IResolver
    {
        /// <summary>
        /// private value for Funq container
        /// </summary>
        private static Container funqContainer;

        /// <summary>
        /// Resolver usado como failover do Funq (manter compatibilidade com o Unity)
        /// </summary>
        private static IResolver failoverResolver;

        /// <summary>
        /// Inicializa o FunqResolver
        /// </summary>
        public static void Init()
        {
            Injector.SetResolver(new FunqResolver());
        }

        #region Implementation of IResolver

        /// <summary>
        /// Resolve uma interface
        /// </summary>
        /// <typeparam name="T">tipo da interface</typeparam>
        /// <param name="name">nome do container</param>
        /// <returns>instância que implementa a interface</returns>
        public T Resolve<T>(string name = null)
        {
            if (failoverResolver == null)
            {
                return funqContainer.ResolveNamed<T>(name);
            }

            try
            {
                return funqContainer.ResolveNamed<T>(name);
            }
            catch (ResolutionException)
            {
                return failoverResolver.Resolve<T>(name);
            }
        }

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
        public void Register<TService>(string name, Func<TService> factory) where TService : class
        {
            Register(name, c => factory.Invoke());
        }

        #endregion

        /// <summary>
        /// Register the DI into Funq
        /// </summary>
        /// <typeparam name="TService">type to be registered</typeparam>
        /// <param name="name">container name</param>
        /// <param name="factory">factory method</param>
        internal static void Register<TService>(string name, Func<Container, TService> factory) where TService : class
        {
            if (funqContainer == null)
            {
                funqContainer = new Container();
            }

            var currentResolver = Injector.GetResolver();
            if (!(currentResolver is FunqResolver))
            {
                if (failoverResolver == null)
                {
                    failoverResolver = Injector.GetResolver();
                }

                Injector.SetResolver(new FunqResolver());
            }

            funqContainer.Register(name, factory);
        }
    }
}
