namespace MTO.Practices.Common
{
    using System;
    using System.Linq;

    /// <summary>
    /// Contexto somente para leitura
    /// </summary>
    public class ReadOnlyContext : IDisposable
    {
        /// <summary>
        /// Unit of work atual desta thread
        /// </summary>
        [ThreadStatic]
        private static ReadOnlyContext roc;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReadOnlyContext"/> class. 
        /// Inicia um contexto somente para leitura. Somente chamar dentro de using().
        /// </summary>
        public ReadOnlyContext()
        {
            roc = this;
        }

        /// <summary>
        /// Delegate de dispose do contexto de leitura
        /// </summary>
        public delegate void DisposeEventHandler();

        /// <summary>
        /// Evento de dispose da unidade de trabalho
        /// </summary>
        public event DisposeEventHandler OnDispose;

        /// <summary>
        /// Recupera o contexto de leitura atual, se houver uma
        /// </summary>
        public static ReadOnlyContext Current
        {
            get
            {
                return roc;
            }
        }

        /// <summary>
        /// Indica se um contexto somente para leitura está ativo
        /// </summary>
        public static bool InProgress
        {
            get
            {
                return roc != null;
            }
        }

        /// <summary>
        /// Singleton de chaveamento para testes unitários.
        /// É feio, eu sei. Em breve eu crio um ReadOnlyContext mockavel.
        /// </summary>
        internal static bool TestMode { get; set; }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        public void Dispose()
        {
            roc = null;
            if (this.OnDispose != null)
            {
                this.OnDispose();
            }
        }
    }
}
