namespace MTO.Practices.Common
{
    using System;
    using System.Collections.Generic;

    using Rhino.Mocks;

    /// <summary>
    /// Usado para substituir o unity na resolução de dependências
    /// </summary>
    public class MockResolver
    {
        /// <summary>
        /// Armazena uma instância para cada tipo criado.
        /// É thread static para garantir que as instâncias não serão compartilhadas entre os testes 
        /// unitários, ao menos no MSTest, que é obrigatoriamente multi-threaded.
        /// </summary>
        [ThreadStatic]
        private static Dictionary<string, object> mockedTypes;

        /// <summary>
        /// Initializes static members of the <see cref="MockResolver"/> class.
        /// </summary>
        static MockResolver()
        {
            Injector.SetResolver(RhinoResolver);
        }

        /// <summary>
        /// Repositório de mocks do teste atual
        /// </summary>
        private static Dictionary<string, object> MockedTypes
        {
            get
            {
                return mockedTypes ?? (mockedTypes = new Dictionary<string, object>());
            }
        }

        /// <summary>
        /// Adiciona interface à lista de interface que devem ser instanciadas por mocking.
        /// </summary>
        /// <param name="mock">
        /// Parâmetro opcional para associar o mock a ser criado a um objeto já existente.
        /// </param>
        /// <typeparam name="T">
        /// Tipo da interface a ser instanciada por mocking.
        /// </typeparam>
        /// <returns>
        /// Intância que implementa a interface mencionada.
        /// </returns>
        public static T RegisterClass<T>(T mock = null) where T : class
        {
            return RegisterClass<T>(mock, null);
        }

        /// <summary>
        /// Adiciona interface à lista de interface que devem ser instanciadas por mocking.
        /// </summary>
        /// <param name="containerName">
        /// O container onde será válida a associação
        /// </param>
        /// <typeparam name="T">
        /// Tipo da interface a ser instanciada por mocking.
        /// </typeparam>
        /// <returns>
        /// Intância que implementa a interface mencionada.
        /// </returns>
        public static T RegisterClass<T>(string containerName) where T : class
        {
            return RegisterClass<T>(null, containerName);
        }

        /// <summary>
        /// Adiciona interface à lista de interface que devem ser instanciadas por mocking.
        /// </summary>
        /// <param name="mock">
        /// Parâmetro opcional para associar o mock a ser criado a um objeto já existente.
        /// </param>
        /// <param name="containerName">
        /// Container opcional onde será válida a associação
        /// </param>
        /// <typeparam name="T">
        /// Tipo da interface a ser instanciada por mocking.
        /// </typeparam>
        /// <returns>
        /// Intância que implementa a interface mencionada.
        /// </returns>
        private static T RegisterClass<T>(T mock = null, string containerName = null) where T : class
        {
            string key = typeof(T).Name;
            if (!string.IsNullOrEmpty(containerName))
            {
                key += "@" + containerName;
            }

            if (mock != null)
            {
                MockedTypes[key] = mock;
            }

            if (mock == null)
            {
                if (MockedTypes.ContainsKey(key))
                {
                    mock = (T)MockedTypes[key];
                }
                else
                {
                    mock = MockRepository.GenerateMock<T>();
                    MockedTypes[key] = mock;
                }
            }

            return mock;
        }

        /// <summary>
        /// Limpa a lista de mocks.
        /// </summary>
        public static void Clear()
        {
            MockedTypes.Clear();
        }

        /// <summary>
        /// Resolve dependências utilizando o RhinoMocks
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
        internal static object RhinoResolver(Type type, string containerName)
        {
            var key = type.Name;
            if (!string.IsNullOrEmpty(containerName))
            {
                key += "@" + containerName;
            }

            if (MockedTypes.ContainsKey(key))
            {
                return MockedTypes[key];
            }

            // Fallback para tipos que não registrados
            return UnityResolver.Resolve(type, containerName);
        }
    }
}
