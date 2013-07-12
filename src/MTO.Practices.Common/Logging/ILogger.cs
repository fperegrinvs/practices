namespace MTO.Practices.Common
{
    using System;
    using System.ServiceModel;

    /// <summary>
    /// Interface base para Log
    /// </summary>
    [ServiceContract]
    public interface ILogger
    {
        /// <summary>
        /// Define o nome da aplicação
        /// </summary>
        string ApplicationName { get; set; }

        /// <summary>
        /// Recupera a instância padrão do logger.
        /// </summary>
        /// <returns>
        /// instância padrão do logger.
        /// </returns>
        [OperationContract]
        ILogger GetDefaultInstance();

        /// <summary>
        /// Registra exception que ocorreu no sistema.
        /// </summary>
        /// <param name="ex">
        /// Exception do sistema.
        /// </param>
        /// <param name="storeId">
        /// Identificador do cliente
        /// </param>
        /// <param name="appName">
        /// The app Name.
        /// </param>
        void LogError(Exception ex, string storeId = null, string appName = null);

        /// <summary>
        /// Registra exception que ocorreu no sistema.
        /// </summary>
        /// <param name="event"> Evento do sistema </param>
        /// <param name="detail">Detalhamento do evento</param>
        /// <param name="storeId"> Identificador do cliente </param>
        /// <param name="appName"> The app Name. </param>
        [OperationContract]
        void LogEvent(string @event, string detail = null, string storeId = null, string appName = null);

        /// <summary>
        /// Encadeia outro logger a este atual
        /// </summary>
        /// <param name="listener">O outro logger</param>
        /// <param name="threadOnly">Se este logger deve ficar encadeado apenas nesta thread</param>
        void Chain(ILogger listener, bool threadOnly);

        /// <summary>
        /// Limpa a cadeia de loggers deste atual
        /// </summary>
        /// <param name="threadOnly">Indica se devemos limpar apenas a cadeia da thread</param>
        void ClearChain(bool threadOnly);
    }
}
