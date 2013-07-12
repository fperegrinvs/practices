namespace MTO.Practices.Common
{
    using System.Threading;

    /// <summary>
    /// A thread with context.
    /// </summary>
    public class ThreadWithContext
    {
        /// <summary>
        /// The thread.
        /// </summary>
        private Thread thread;

        /// <summary>
        /// Initializes a new instance of the <see cref="ThreadWithContext"/> class.
        /// </summary>
        /// <param name="threadStart"> The delegate for the thread. </param>
        public ThreadWithContext(ThreadStart threadStart)
        {
            var userName = Context.Current.GetCurrentUserName();
            var userId = Context.Current.GetCurrentUserId();

            this.thread = new Thread(() =>
            {
                Context.SetContext(new ThreadContext());
                try
                {
                    Context.Current.SetAuthenticated(userId, userName, string.Empty);

                    threadStart();
                }
                finally
                {
                    Context.Clear();
                }
            });
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ThreadWithContext"/> class.
        /// </summary>
        /// <param name="threadStart"> The delegate for the thread. </param>
        public ThreadWithContext(ParameterizedThreadStart threadStart)
        {

            var userName = Context.Current.GetCurrentUserName();
            var userId = Context.Current.GetCurrentUserId();

            this.thread = new Thread((obj) =>
            {
                Context.SetContext(new ThreadContext());
                try
                {
                    Context.Current.SetAuthenticated(userId, userName, string.Empty);

                    threadStart(obj);
                }
                finally
                {
                    Context.Clear();
                }
            });
        }

        /// <summary>
        /// Start the thread
        /// </summary>
        public void Start()
        {
            this.thread.Start();
        }

        /// <summary>
        /// Start the parametrized thread
        /// </summary>
        /// <param name="obj">The parameter</param>
        public void Start(object obj)
        {
            this.thread.Start(obj);
        }
    }
}
