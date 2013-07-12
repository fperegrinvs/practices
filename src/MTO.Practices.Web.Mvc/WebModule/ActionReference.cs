namespace MTO.Practices.WebModule
{
    /// <summary>
    /// Referência para uma action
    /// </summary>
    public class ActionReference
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ActionReference"/> class.
        /// </summary>
        /// <param name="controllerName">
        /// Nome do controller que contém a action
        /// </param>
        /// <param name="actionName">
        /// Nome da action
        /// </param>
        /// <param name="area">
        /// Área do controller
        /// </param>
        public ActionReference(string controllerName, string actionName, string area = "")
        {
            this.ControllerName = controllerName;
            this.ActionName = actionName;
            this.AreaName = area;
        }

        /// <summary>
        /// Nome do controller que contém a action
        /// </summary>
        public string ControllerName { get; set; }

        /// <summary>
        /// Nome da action
        /// </summary>
        public string ActionName { get; set; }

        /// <summary>
        /// Nome da área
        /// </summary>
        public string AreaName { get; set; }
    }
}
