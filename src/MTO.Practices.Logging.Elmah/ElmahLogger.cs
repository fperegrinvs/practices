namespace MTO.Practices.Common
{
    using System;
    using System.Collections.Generic;
    using System.Web;

    using Elmah;

    using MTO.Practices.Common.Entity;
    using MTO.Practices.Common.Extensions;

    using Microsoft.Practices.ObjectBuilder2;

    using OneToOne.Practices.Log;

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
        /// Cadeia de listeners globais
        /// </summary>
        [ThreadStatic]
        private static IList<ILogger> threadChain;

        /// <summary>
        /// variavel interna usada se comunicar com o elmah
        /// </summary>
        private ErrorLog elmah;

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
        public void LogError(Exception ex, string storeId = null, string appName = null)
        {
            // Encadeamento
            if (threadChain != null)
            {
                threadChain.ForEach(x => x.LogError(ex, storeId, appName));
            }

            if (staticChain != null)
            {
                staticChain.ForEach(x => x.LogError(ex, storeId, appName));
            }

            var error = new Error(ex, HttpContext.Current);

            if (!string.IsNullOrEmpty(appName))
            {
                error.ApplicationName = appName;
            }

            using (var ts = BatchTransaction.Supress())
            {
                this.elmah.Log(error);

                ts.Complete();
            }
        }

        /// <summary>
        /// Registra exception que ocorreu no sistema.
        /// </summary>
        /// <param name="event"> Evento do sistema </param>
        /// <param name="detail">Detalhamento do evento</param>
        /// <param name="storeId"> Identificador do cliente </param>
        /// <param name="appName"> The app Name. </param>
        public void LogEvent(string @event, string detail, string storeId = null, string appName = null)
        {
            // Encadeamento
            if (threadChain != null)
            {
                threadChain.ForEach(x => x.LogEvent(@event, detail, storeId, appName));
            }

            if (staticChain != null)
            {
                staticChain.ForEach(x => x.LogEvent(@event, detail, storeId, appName));
            }

            var error = new Error(new EventException(@event, null, storeId, appName), HttpContext.Current);

            if (!string.IsNullOrEmpty(detail))
            {
                error.Detail = detail;
            }

            if (!string.IsNullOrEmpty(appName))
            {
                error.ApplicationName = appName;
            }

            using (var ts = BatchTransaction.Supress())
            {
                this.elmah.Log(error);

                ts.Complete();
            }
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
            }
            else
            {
                staticChain = staticChain.AddOrCreate(listener);
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
