namespace MTO.Practices.Common.Unity
{
    using System;

    using Microsoft.Practices.Unity;
    using Microsoft.Practices.Unity.Configuration;

    using MTO.Practices.Common.Interfaces;

    /// <summary>
    /// Classe reponsável por efetuar injeções de dependência utilizando unity
    /// </summary>
    public class UnityResolver : IResolver
    {
        /// <summary>
        /// Inicializa o UnityResolver
        /// </summary>
        public static void Init()
        {
            Injector.SetResolver(new UnityResolver());
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
            var type = typeof(T);

            using (IUnityContainer container = new UnityContainer())
            {
                if (string.IsNullOrWhiteSpace(name))
                {
                    container.LoadConfiguration();
                }
                else
                {
                    container.LoadConfiguration(name);
                }

                return (T)container.Resolve(type);
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
            throw new NotImplementedException("Não implementado no unity");
        }

        #endregion
    }
}
