namespace MTO.Practices.Common
{
    using System;

    /// <summary>
    /// Classe reponsável por efetuar injeções de dependência.
    /// Ela é usada ao invés do container unity para facilitar os testes unitários e o isolamento das dependências.
    /// </summary>
    public static class Injector
    {
        /// <summary>
        /// Handler para o método que injeta dependências.
        /// </summary>
        private static ResolveEventHandler resolver = null;

        /// <summary>
        /// Initializes static members of the <see cref="Injector"/> class.
        /// </summary>
        static Injector()
        {
            // resolver = UnityResolver.Resolve;
        }

        /// <summary>
        /// Delegate usado para invocar método para resolução de dependências.
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
        internal delegate object ResolveEventHandler(Type type, string containerName);

        /// <summary>
        /// Resolve dependências utilizando o handler definido para tal propósito.
        /// </summary>
        /// <param name="containerName">
        /// TNome do container que contém as configurações da interface.
        /// null = container padrão.
        /// </param>
        /// <typeparam name="T">
        /// Tipo da interface a ser resolvida.
        /// </typeparam>
        /// <returns>
        /// Objeto com instância da classe injetada.
        /// </returns>
        public static T ResolveInterface<T>(string containerName = null) where T : class
        {
            if (resolver == null)
            {
                throw new InvalidOperationException("Nenhum resolver está definido para o injector. Utilize o comando UnityResolver.Init() para usar o unity como resolver.");
            }

            var resolvedObject = resolver(typeof(T), containerName);
            return (T)resolvedObject;
        }

        /// <summary>
        /// Altera o método usado para realizar a injeção de dependências.
        /// Deve ser usado apenas no contexto de testes.
        /// </summary>
        /// <returns>
        /// Retorna resolver utilizado
        /// </returns>
        internal static ResolveEventHandler GetResolver()
        {
            return resolver;
        }

        /// <summary>
        /// Altera o método usado para realizar a injeção de dependências.
        /// Deve ser usado apenas no contexto de testes.
        /// </summary>
        /// <param name="resolverHandler">
        /// Handler para o método responsável por injetar depenências.
        /// </param>
        internal static void SetResolver(ResolveEventHandler resolverHandler)
        {
            resolver = resolverHandler;
        }
    }
}
