namespace OneToOne.Practices.Log
{
    using System;

    /// <summary>
    /// Exception usada para registrar um evento no elmah
    /// </summary>
    [Serializable]
    public class EventException : Exception
    {
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
        public EventException(string title, string details = null, string storeId = null, string appName = null)
            : base(title)
        {
            this.Details = details;
            this.StoreId = storeId;
            this.AppName = appName;
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
        /// Override do método toString
        /// </summary>
        /// <returns>texto descrevendo a exception</returns>
        public override string ToString()
        {
            var result = new System.Text.StringBuilder(GetType().ToString());
            result.Append(": ").AppendLine(Message);

            if (!string.IsNullOrEmpty(this.Details))
            {
                result.AppendLine(this.Details);
            }

            if (InnerException != null)
            {
                result.Append(" ---> ").Append(this.InnerException.ToString());
                result.Append(Environment.NewLine);
                result.Append("  --- End of inner exception stack trace ---");
            }

            if (StackTrace != null)
            {
                result.Append(Environment.NewLine).Append(StackTrace);
            }

            return result.ToString();
        }
    }
}
