namespace MTO.Practices.Common
{
    using System;

    using Microsoft.Practices.Unity;
    using Microsoft.Practices.Unity.InterceptionExtension;

    /// <summary>
    /// Atributo que garante que um método só será executado dentro de UnitOfWork
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class RequiresUnitOfWorkAttribute : HandlerAttribute, ICallHandler
    {
        /// <summary>
        /// Singleton do Handler
        /// </summary>
        private static RequiresUnitOfWorkAttribute instance;

        /// <summary>
        /// Instancia estatica do handler
        /// </summary>
        internal static RequiresUnitOfWorkAttribute Instance
        {
            get
            {
                return instance ?? (instance = new RequiresUnitOfWorkAttribute());
            }
        }

        /// <summary>
        /// Evento de criação do atributo, deve retornar um Handler das chamadas
        /// </summary>
        /// <param name="container">O container</param>
        /// <returns>O handler</returns>
        public override ICallHandler CreateHandler(IUnityContainer container)
        {
            return Instance;
        }

        /// <summary>
        /// Handler de invocação do método com atributo
        /// </summary>
        /// <param name="input">Informações da chamada</param>
        /// <param name="getNext">Próximo handler de execução do método</param>
        /// <returns>Caso estejamos fora dum UoW, retorna exception, caso contrário segue a execução</returns>
        public IMethodReturn Invoke(IMethodInvocation input, GetNextHandlerDelegate getNext)
        {
            if (UnitOfWork.Current == null)
            {
                return input.CreateExceptionMethodReturn(new Exception("Método " + input.MethodBase.Name + " chamado fora de UnitOfWork."));
            }

            // Retornamos normalmente
            return getNext()(input, getNext);
        }
    }
}
