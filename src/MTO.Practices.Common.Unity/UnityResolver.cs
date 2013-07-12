namespace MTO.Practices.Common
{
    using System;
    using System.IO;

    using Microsoft.Practices.Unity;
    using Microsoft.Practices.Unity.Configuration;

    /// <summary>
    /// Classe reponsável por efetuar injeções de dependência utilizando unity
    /// </summary>
    public class UnityResolver
    {
        /// <summary>
        /// Inicializa o UnityResolver
        /// </summary>
        public static void Init()
        {
            Injector.SetResolver(Resolve);
        }

        /// <summary>
        /// Resolve dependências utilizando o Unity
        /// </summary>
        /// <param name="type">
        /// Tipo da interface a ser resolvida.
        /// </param>
        /// <param name="containerName">
        /// TNome do container que contém as configurações da interface.
        /// null = container padrão.
        /// </param>
        /// <returns>
        /// Objeto com instância da classe injetada.
        /// </returns>
        internal static object Resolve(Type type, string containerName)
        {
            using (IUnityContainer container = new UnityContainer())
            {
                if (string.IsNullOrWhiteSpace(containerName))
                {
                    container.LoadConfiguration();
                }
                else
                {
                    try
                    {
                        container.LoadConfiguration(containerName);
                    }
                    catch (FileLoadException ex)
                    {
                        throw new FileLoadException(ex.Message + "\r\n >> Assembly referenciado por injeção de dependência precisa ser referenciado no projeto também: " + containerName);
                    }
                }

                return container.Resolve(type);
            }
        }
    }
}
