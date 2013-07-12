namespace MTO.Practices.Common.JobManager
{
    using System;
    using System.Collections.Generic;

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
        public void LogError(Exception ex, string storeId = null, string appName = null)
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
        /// Registra exception que ocorreu no sistema.
        /// </summary>
        /// <param name="event"> Evento do sistema </param>
        /// <param name="detail">Detalhamento do evento</param>
        /// <param name="storeId"> Identificador do cliente </param>
        /// <param name="appName"> The app Name. </param>
        public void LogEvent(string @event, string detail = null, string storeId = null, string appName = null)
        {
            var storeApp = string.Empty;
            if (!string.IsNullOrEmpty(storeId) || !string.IsNullOrEmpty(appName))
            {
                storeApp = string.Format("({0}/{1})", storeId ?? string.Empty, appName ?? string.Empty);
            }

            this.Log.Add(string.Format("<b>[{0}]</b> - {1} {4}- {2}<br/>{3}", DateTime.Now.ToShortTimeString(), "Evento:", @event, detail, storeApp));
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
