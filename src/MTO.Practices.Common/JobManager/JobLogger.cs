namespace MTO.Practices.Common.JobManager
{
    using System;
    using System.Collections.Generic;

    using MTO.Practices.Common.Enumerators;
    using MTO.Practices.Common.Logging;

    /// <summary>
    /// Loga eventos e erros durante a execução de um job
    /// </summary>
    internal class JobLogger : ILogger
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="JobLogger"/> class.
        /// </summary>
        /// <param name="log">
        /// The log.
        /// </param>
        public JobLogger(IList<string> log)
        {
            if (log == null)
            {
                throw new ArgumentNullException("log");
            }

            this.Log = log;
        }

        /// <summary>
        /// Indica se o logger está encadeado a outro logger
        /// Quando um log está encadeado a outro, ele sempre vai ser disparado como efeito colateral do logger principal
        /// Tal hierarquia é importante para evitar loops infinitos
        /// </summary>
        public bool IsChild { get; set; }

        /// <summary>
        /// Define o nome da aplicação
        /// </summary>
        public string ApplicationName { get; set; }

        /// <summary>
        /// Gets or sets the log.
        /// </summary>
        protected IList<string> Log { get; set; }

        /// <summary>
        /// Recupera a instância padrão do logger.
        /// </summary>
        /// <returns>
        /// instância padrão do logger.
        /// </returns>
        public ILogger GetDefaultInstance()
        {
            throw new Exception("JobLogger nao pode ser instanciado pelo Unity");
        }

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
        public void LogException(Exception ex, string storeId = null, string appName = null, LogTypeEnum logType = LogTypeEnum.Error)
        {
            if (ex == null)
            {
                throw new ArgumentNullException("ex");
            }

            var storeApp = string.Empty;
            if (!string.IsNullOrEmpty(storeId) || !string.IsNullOrEmpty(appName))
            {
                storeApp = string.Format("({0}/{1})", storeId ?? string.Empty, appName ?? string.Empty);
            }

            this.Log.Add(string.Format("<b>[{0}]</b> - {1} {4}- {2}<br/>{3}", DateTime.Now.ToShortTimeString(), "Erro:", ex.Message, ex, storeApp));
        }

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
        public void LogException(List<Exception> ex, string storeId = null, string appName = null)
        {
            foreach (var e in ex)
            {
                this.LogException(e, storeId, appName);
            }
        }

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
        public void LogEvent(string title, string storeId = null, string appName = null, string details = null, LogTypeEnum logType = LogTypeEnum.Error)
        {
            var storeApp = string.Empty;
            if (!string.IsNullOrEmpty(storeId) || !string.IsNullOrEmpty(appName))
            {
                storeApp = string.Format("({0}/{1})", storeId ?? string.Empty, appName ?? string.Empty);
            }

            this.Log.Add(string.Format("<b>[{0}]</b> - {1} {4}- {2}<br/>{3}", DateTime.Now.ToShortTimeString(), "Evento:",  title, details, storeApp));
        }

        /// <summary>
        /// Registra um evento
        /// </summary>
        /// <param name="eventVO">
        /// The event VO.
        /// </param>
        public void LogEvent(EventVO eventVO)
        {
            var storeApp = string.Empty;
            if (!string.IsNullOrEmpty(eventVO.StoreId) || !string.IsNullOrEmpty(eventVO.AppName))
            {
                storeApp = string.Format("({0}/{1})", eventVO.AppName ?? string.Empty, eventVO.AppName ?? string.Empty);
            }

            this.Log.Add(string.Format("<b>[{0}]</b> - {1} {4}- {2}<br/>{3}", DateTime.Now.ToShortTimeString(), "Evento:", eventVO.Title, eventVO.Details, storeApp));
        }

        /// <summary>
        /// Encadeia outro logger a este atual
        /// </summary>
        /// <param name="listener">O outro logger</param>
        /// <param name="threadOnly">Se este logger deve ficar encadeado apenas nesta thread</param>
        public void Chain(ILogger listener, bool threadOnly)
        {
            throw new NotImplementedException("JobLogger não encadeia");
        }

        /// <summary>
        /// Limpa a cadeia de loggers deste atual
        /// </summary>
        /// <param name="threadOnly">Indica se devemos limpar apenas a cadeia da thread</param>
        public void ClearChain(bool threadOnly)
        {
            throw new NotImplementedException("JobLogger não encadeia");
        }
    }
}
