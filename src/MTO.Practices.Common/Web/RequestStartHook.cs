namespace MTO.Practices.Common.Web
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Gancho para eventos que devem ocorrer ao inicializar um request
    /// Trigger manual, devido a http://stackoverflow.com/questions/3712598/httpmodule-init-safely-add-httpapplication-beginrequest-handler-in-iis7-integr
    /// </summary>
    public static class RequestStartHook
    {
        /// <summary>
        /// Lista de actions a serem executadas quando o request iniciar
        /// </summary>
        private static List<Action> Actions { get; set; }

        /// <summary>
        /// Inclui uma action
        /// </summary>
        /// <param name="action">A action a ser realizada no inicio de cada request</param>
        public static void Add(Action action)
        {
            if (Actions == null)
            {
                Actions = new List<Action>();
            }

            if (!Actions.Contains(action))
            {
                Actions.Add(action);
            }
        }

        /// <summary>
        /// Roda as actions.
        /// Trigger manual, devido a http://stackoverflow.com/questions/3712598/httpmodule-init-safely-add-httpapplication-beginrequest-handler-in-iis7-integr
        /// </summary>
        public static void Trigger()
        {
            if (Actions != null)
            {
                foreach (var action in Actions)
                {
                    action();
                }
            }
        }
    }
}
