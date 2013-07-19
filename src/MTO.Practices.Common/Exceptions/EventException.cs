namespace MTO.Practices.Common.Exceptions
{
    using System;

    using MTO.Practices.Common.Enumerators;
    using MTO.Practices.Common.Logging;

    /// <summary>
    /// Exception usada para registrar um evento no elmah
    /// </summary>
    [Serializable]
    public class EventException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EventException"/> class.
        /// </summary>
        /// <param name="eventlog">
        /// The eventlog.
        /// </param>
        public EventException(EventVO eventlog)
            : base(eventlog.Title)
        {
            this.Details = eventlog.Details;
            this.StoreId = eventlog.StoreId;
            this.AppName = eventlog.AppName;
            this.LogType = eventlog.LogType;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EventException"/> class.
        /// </summary>
        /// <param name="eventVO">
        /// The event vo.
        /// </param>
        /// <param name="ex">
        /// The excetion
        /// </param>
        public EventException(EventVO eventVO, Exception ex)
            : base(eventVO.Title, ex)
        {
            this.Details = eventVO.Details;
            this.StoreId = eventVO.StoreId;
            this.AppName = eventVO.AppName;
            this.LogType = eventVO.LogType;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EventException"/> class.
        /// </summary>
        /// <param name="title">
        /// Título da exception
        /// </param>
        /// <param name="details">
        /// The details.
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
        public EventException(string title, string details = null, string storeId = null, string appName = null, LogTypeEnum? logType = null)
            : base(title)
        {
            this.Details = details;
            this.StoreId = storeId;
            this.AppName = appName;
            this.LogType = logType;
        }

        /// <summary>
        /// Identificador do cliente.
        /// </summary>
        public string StoreId { get; set; }

        /// <summary>
        /// Detalhes do evento.
        /// </summary>
        public string Details { get; set; }

        /// <summary>
        /// Nome da aplicação
        /// </summary>
        public string AppName { get; set; }

        /// <summary>
        /// Tipo de evento logado
        /// </summary>
        public LogTypeEnum? LogType { get; set; }

        /// <summary>
        /// Override do método toString
        /// </summary>
        /// <returns>texto descrevendo a exception</returns>
        public override string ToString()
        {
            var result = new System.Text.StringBuilder(this.GetType().ToString());
            result.Append(": ").AppendLine(this.Message);

            if (!string.IsNullOrEmpty(this.Details))
            {
                result.AppendLine(this.Details);
            }

            if (this.InnerException != null)
            {
                result.Append(" ---> ").Append(this.InnerException.ToString());
                result.Append(Environment.NewLine);
                result.Append("  --- End of inner exception stack trace ---");
            }

            if (this.StackTrace != null)
            {
                result.Append(Environment.NewLine).Append(this.StackTrace);
            }

            return result.ToString();
        }
    }
}
