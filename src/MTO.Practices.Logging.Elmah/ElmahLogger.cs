namespace MTO.Practices.Logging.Elmah
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using global::Elmah;

    using MTO.Practices.Common.Enumerators;
    using MTO.Practices.Common.Exceptions;
    using MTO.Practices.Common.Extensions;
    using MTO.Practices.Common.Logging;

    /// <summary>
    /// Imnplementa mecanismo de log de erros.
    /// </summary>
    public class ElmahLogger : ILogger
    {
        /// <summary>
        /// Cadeia de listeners globais
        /// </summary>
        private static IList<ILogger> staticChain;

        /// <summary>
        /// Cadeia de listeners da thread
        /// </summary>
        [ThreadStatic]
        private static IList<ILogger> threadChain;

        /// <summary>
        /// variavel interna usada se comunicar com o elmah
        /// </summary>
        private readonly ErrorLog elmah;

        /// <summary>
        /// Initializes a new instance of the <see cref="ElmahLogger"/> class.
        /// </summary>
        public ElmahLogger()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ElmahLogger"/> class.
        /// </summary>
        /// <param name="errorLog">
        /// The error log.
        /// </param>
        internal ElmahLogger(ErrorLog errorLog)
        {
            this.elmah = errorLog;
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
        public string ApplicationName
        {
            get
            {
                if (this.elmah != null)
                {
                    return this.elmah.ApplicationName;
                }

                return null;
            }

            set
            {
                if (this.elmah != null)
                {
                    this.elmah.ApplicationName = value;
                }
            }
        }

        /// <summary>
        /// Recupera a instância padrão do logger.
        /// </summary>
        /// <returns>
        /// instância padrão do logger.
        /// </returns>
        public ILogger GetDefaultInstance()
        {
            if (this.elmah != null)
            {
                return this;
            }

            return new ElmahLogger(ErrorLog.GetDefault(null));
        }

        /// <summary>
        /// Registra exception que ocorreu no sistema.
        /// </summary>
        /// <param name="ex">
        /// Exception do sistema.
        /// </param>
        /// <param name="storeId">
        /// The store Id.
        /// </param>
        /// <param name="appName">
        /// The app Name.
        /// </param>
        /// <param name="logType">
        /// The log Type.
        /// </param>
        public void LogException(Exception ex, string storeId = null, string appName = null, LogTypeEnum logType = LogTypeEnum.Error)
        {
            // Encadeamento
            if (!this.IsChild && threadChain != null)
            {
                foreach (var childLogger in threadChain)
                {
                    childLogger.LogException(ex, storeId, appName, logType);
                }
            }

            if (!this.IsChild && staticChain != null)
            {
                foreach (var childLogger in staticChain)
                {
                    childLogger.LogException(ex, storeId, appName, logType);
                }
            }

            this.elmah.Log(new Error(ex));
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
            var sb = new StringBuilder(ex.Count);
            foreach (var error in ex.Select(exception => new Error(exception)))
            {
                sb.AppendLine(ErrorJson.EncodeString(error));
            }

            var aggregateException = new EventException("Erros Agregados", sb.ToString(), storeId, appName);
            this.LogException(aggregateException, storeId, appName);
        }

        /// <summary>
        /// Registra exception que ocorreu no sistema.
        /// </summary>
        /// <param name="err">
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
        public void LogEvent(string err, string storeId = null, string appName = null, string details = null, LogTypeEnum logType = LogTypeEnum.Error)
        {
            // Encadeamento
            if (!this.IsChild && threadChain != null)
            {
                foreach (var childLogger in threadChain)
                {
                    childLogger.LogEvent(err, storeId, appName, details, logType);
                }
            }

            if (!this.IsChild && staticChain != null)
            {
                foreach (var childLogger in staticChain)
                {
                    childLogger.LogEvent(err, storeId, appName, details, logType);
                }
            }

            Error error = null;
            if (!string.IsNullOrEmpty(err) && err.StartsWith("<error"))
            {
                try
                {
                    error = ErrorXml.DecodeString(err);
                }
                catch (Exception)
                {
                }
            }

            if (error == null)
            {
                error = new Error(new EventException(err, details, storeId, appName));
            }

            this.elmah.Log(error);
        }

        /// <summary>
        /// Registra um evento
        /// </summary>
        /// <param name="eventVO">
        /// The event VO.
        /// </param>
        public void LogEvent(EventVO eventVO)
        {
            // Encadeamento
            if (!this.IsChild && threadChain != null)
            {
                foreach (var childLogger in threadChain)
                {
                    childLogger.LogEvent(eventVO);
                }
            }

            if (!this.IsChild && staticChain != null)
            {
                foreach (var childLogger in staticChain)
                {
                    childLogger.LogEvent(eventVO);
                }
            }

            var exception = new EventException(eventVO);
            this.elmah.Log(new Error(exception));
        }

        /// <summary>
        /// Encadeia outro logger a este atual
        /// </summary>
        /// <param name="listener">O outro logger</param>
        /// <param name="threadOnly">Se este logger deve ficar encadeado apenas nesta thread</param>
        public void Chain(ILogger listener, bool threadOnly)
        {
            if (threadOnly)
            {
                threadChain = threadChain.AddOrCreate(listener);
                listener.IsChild = true;
            }
            else
            {
                staticChain = staticChain.AddOrCreate(listener);
                listener.IsChild = true;
            }
        }

        /// <summary>
        /// Limpa a cadeia de loggers deste atual
        /// </summary>
        /// <param name="threadOnly">Indica se devemos limpar apenas a cadeia da thread</param>
        public void ClearChain(bool threadOnly)
        {
            threadChain = null;

            if (!threadOnly)
            {
                staticChain = null;
            }
        }
    }
}
