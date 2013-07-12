namespace MTO.Practices.Common
{
    using System;
    using System.Collections.Generic;

    using MTO.Practices.Common.Extensions;

    using Microsoft.Practices.ObjectBuilder2;

    /// <summary>
    /// Implementa mecanismo de log de para debugging.
    /// </summary>
    public class DebugLogger : ILogger
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
        /// Define o nome da aplicação
        /// </summary>
        public string ApplicationName { get; set; }

        /// <summary>
        /// Recupera a instância padrão do logger.
        /// </summary>
        /// <returns>
        /// instância padrão do logger.
        /// </returns>
        public ILogger GetDefaultInstance()
        {
            return this;
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

            System.Diagnostics.Debug.WriteLine("--------------------------------------------");
            System.Diagnostics.Debug.WriteLine("[{0}] Exception: \r\n{1}", DateTime.Now, ex);
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
            // Encadeamento
            if (threadChain != null)
            {
                threadChain.ForEach(x => x.LogEvent(@event, detail, storeId, appName));
            }

            if (staticChain != null)
            {
                staticChain.ForEach(x => x.LogEvent(@event, detail, storeId, appName));
            }

            System.Diagnostics.Debug.WriteLine("--------------------------------------------");
            System.Diagnostics.Debug.WriteLine("[{0}] Event: {1}\r\nDetails: {2}", DateTime.Now, @event, detail);
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
