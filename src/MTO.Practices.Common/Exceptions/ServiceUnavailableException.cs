namespace MTO.Practices.Common.Exceptions
{
    using System;

    /// <summary>
    /// Exception disparada quando um serviço externo não está disponível.
    /// </summary>
    public class ServiceUnavailableException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceUnavailableException"/> class.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        public ServiceUnavailableException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceUnavailableException"/> class.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        /// <param name="ex">
        /// The ex.
        /// </param>
        public ServiceUnavailableException(string message, Exception ex)
            : base(message, ex)
        {
        }
    }
}
