namespace MTO.Practices.Common
{
    using System;

    /// <summary>
    /// Armazena contexto atual da aplicação
    /// </summary>
    public class Context
    {
        /// <summary>
        /// Armazena localmente o contexto para evitar o resolve
        /// </summary>
        private static readonly IContext InitialResolve = Injector.ResolveInterface<IContext>();

        /// <summary>
        /// Armazena o contexto para evitar o resolve
        /// </summary>
        [ThreadStatic]
        private static IContext localContext;

        /// <summary>
        /// Recupera o contexto atual da aplicação
        /// </summary>
        public static IContext Current
        {
            get
            {
                if (localContext != null)
                {
                    return localContext;
                }

                return localContext = InitialResolve.NewInstance();
            }
        }

        /// <summary>
        /// Zera o contexto, útil ao iniciar e finalizar processamento em uma Thread de Context diferente
        /// </summary>
        internal static void Clear()
        {
            localContext = null;
        }

        /// <summary>
        /// Define novo contexto pra essa thread
        /// </summary>
        /// <param name="ctx"> O contexto </param>
        internal static void SetContext(IContext ctx)
        {
            localContext = ctx;
        }
    }
}
