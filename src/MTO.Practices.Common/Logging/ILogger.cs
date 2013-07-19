namespace MTO.Practices.Common.Logging
{
    using System;
    using System.Collections.Generic;
    using System.ServiceModel;

    using MTO.Practices.Common.Enumerators;

    /// <summary>
    /// Interface base para Log
    /// </summary>
    [ServiceContract]
    public interface ILogger
    {
        /// <summary>
        /// Indica se o logger está encadeado a outro logger
        /// Quando um log está encadeado a outro, ele sempre vai ser disparado como efeito colateral do logger principal
        /// Tal hierarquia é importante para evitar loops infinitos
        /// </summary>
        bool IsChild { get; set; }

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
        /// <param name="logType">
        /// The log Type.
        /// </param>
        void LogException(Exception ex, string storeId = null, string appName = null, LogTypeEnum logType = LogTypeEnum.Error);

        /// <summary>
        /// Registra listas de exceptions em um único pacote
        /// </summary>
        /// <param name="ex">
        /// Lista de exceptions
        /// </param>
        /// <param name="storeId">
        /// Identificador do cliente
        /// </param>
        /// <param name="appName">
        /// The app Name.
        /// </param>
        void LogException(List<Exception> ex, string storeId = null, string appName = null);

        /// <summary>
        /// Registra exception que ocorreu no sistema.
        /// </summary>
        /// <param name="title">
        /// Título da problema/exception
        /// </param>
        /// <param name="storeId">
        /// Identificador do cliente
        /// </param>
        /// <param name="appName">
        /// The app Name.
        /// </param>
        /// <param name="logType">
        /// The log Type.
        /// </param>
        [OperationContract]
        void LogEvent(string err, string storeId = null, string appName = null, string title = null, LogTypeEnum logType = LogTypeEnum.Error);

        /// <summary>
        /// Registra um evento
        /// </summary>
        /// <param name="eventVO">
        /// The event VO.
        /// </param>
        [OperationContract]
        void LogEvent(EventVO eventVO);

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
