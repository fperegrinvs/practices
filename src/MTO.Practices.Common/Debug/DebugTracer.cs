namespace MTO.Practices.Common.Debug
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Diagnostics;
    using System.Linq;
    using System.Text;
    using System.Web;

    /// <summary>
    /// Tracer de eventos de debug
    /// </summary>
    public static class DebugTracer
    {
        /// <summary>
        /// Traces que estão habilitados em Release
        /// </summary>
        private static List<string> ReleaseTracesEnabled { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DebugTracer"/> class.
        /// </summary>
        static DebugTracer()
        {
            ReleaseTracesEnabled = new List<string>();
            var config = ConfigurationManager.AppSettings["RelaseTracesEnabled"];

            if (!string.IsNullOrEmpty(config))
            {
                var items = config.Split(',');
                ReleaseTracesEnabled.AddRange(items.Where(x => !string.IsNullOrEmpty(x)));
            }
        }

        /// <summary>
        /// Guarda eventos de Trace da thread
        /// </summary>
        private static StringBuilder Trace
        {
            get
            {
                if (HttpContext.Current != null)
                {
                    return (StringBuilder)HttpContext.Current.Items["Trace_StringBuilder"]
                           ?? (StringBuilder)(HttpContext.Current.Items["Trace_StringBuilder"] = new StringBuilder());
                }

                var ctx = new ThreadContext();
                var sb = ctx.Get<StringBuilder>("Trace_StringBuilder");
                if (sb == null)
                {
                    sb = new StringBuilder();
                    ctx.Set("Trace_StringBuilder", sb);
                }

                return sb;
            }
        }

        /// <summary>
        /// Escreve uma linha de log
        /// </summary>
        /// <param name="message">Mensagem de log</param>
        /// <param name="category">Categoria do log</param>
        [Conditional("DEBUG")]
        public static void Log(string message, string category = null)
        {
            if (category == null)
            {
                WriteLine(message);
            }
            else
            {
                WriteLine(category + ": " + (message ?? string.Empty));
            }
        }

        /// <summary>
        /// Escreve uma linha de log
        /// Atenção!! Funciona em release, cuidado deve ser tomado para não sobrecarregar o log.
        /// </summary>
        /// <param name="message">Mensagem de log</param>
        /// <param name="category">Categoria do log</param>
        public static void LogRelease(string message, string category = null)
        {
            if (category == null)
            {
                WriteLine(message);
            }
            else
            {
                WriteLine(category + ": " + (message ?? string.Empty));
            }
        }

        /// <summary>
        /// Escreve o log
        /// </summary>
        /// <param name="event">Nome do evento do qual fizemos Trace</param>
        [Conditional("DEBUG")]
        public static void Flush(string @event)
        {
            if (Trace != null && Trace.Length > 0)
            {
                Logger.Instance.LogEvent(@event ?? "Trace", Trace.ToString());
                Trace.Clear();
            }
        }

        /// <summary>
        /// Escreve o log. 
        /// Atenção!! Funciona em release, cuidado deve ser tomado para não sobrecarregar o log.
        /// </summary>
        /// <param name="event">Nome do evento do qual fizemos Trace</param>
        public static void FlushRelease(string @event)
        {
            if (Trace != null && Trace.Length > 0)
            {
                if (!string.IsNullOrEmpty(@event))
                {
                    // Verificamos se o tracing está habilitado
                    if (ReleaseTracesEnabled.Contains(@event))
                    {
                        Logger.Instance.LogEvent(@event, Trace.ToString());
                    }
                }

                Trace.Clear();
            }
        }

        /// <summary>
        /// Escreve uma linha no Trace
        /// </summary>
        /// <param name="line">A linha que vamos escrever</param>
        private static void WriteLine(string line)
        {
            Trace.AppendLine(line);
        }
    }
}
