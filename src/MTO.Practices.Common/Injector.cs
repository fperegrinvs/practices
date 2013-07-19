namespace MTO.Practices.Common
{
    using System;

    using MTO.Practices.Common.Interfaces;

    /// <summary>
    /// Classe reponsável por efetuar injeções de dependência.
    /// Ela é usada ao invés do container unity para facilitar os testes unitários e o isolamento das dependências.
    /// </summary>
    public class Injector
    {
        /// <summary>
        /// Handler para o método que injeta dependências.
        /// </summary>
        private static IResolver resolver = null;

        /// <summary>
        /// Initializes static members of the <see cref="Injector"/> class.
        /// </summary>
        static Injector()
        {
            // resolver = new FunqResolver();
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
            return resolver.Resolve<T>(containerName);
        }

        /// <summary>
        /// Registra mapeamento de inversão de dependencia
        /// </summary>
        /// <typeparam name="TService">typo a ser registrado</typeparam>
        /// <param name="factory">instrução para se criar uma instância</param>
        public static void Register<TService>(Func<TService> factory) where TService : class
        {
            resolver.Register(null, factory);
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
        public static void Register<TService>(string name, Func<TService> factory) where TService : class
        {
            resolver.Register(name, factory);
        }

        /// <summary>
        /// Recupera o método usado para realizar a injeção de dependências.
        /// Deve ser usado apenas no contexto de testes.
        /// </summary>
        /// <returns>
        /// The UpStore.Practices.Common.Interfaces.IResolver.
        /// </returns>
        internal static IResolver GetResolver()
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
        internal static void SetResolver(IResolver resolverHandler)
        {
            resolver = resolverHandler;
        }
    }
}
