namespace MTO.Practices.WebModule
{
    using System.Collections.Generic;

    /// <summary>
    /// Menu de navegação
    /// </summary>
    public class Menu : MenuItem
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Menu"/> class.
        /// </summary>
        /// <param name="title">
        /// Título do ítem de menu
        /// </param>
        /// <param name="controllerName">
        /// Nome do controller que contém a ação a ser acessada
        /// </param>
        /// <param name="actionName">
        /// Nome da action referenciada
        /// </param>
        /// <param name="area">
        /// Área da action
        /// </param>
        public Menu(string title, string controllerName, string actionName, string area = "") 
            : base(title, controllerName, actionName, area)
        {
            this.IsFloatLeft = true;
        }

        public Menu(string title, string controllerName, string actionName)
            : base(title, controllerName, actionName, "")
        {
            this.IsFloatLeft = true;
        }

        public Menu(string title, string controllerName, string actionName, bool isFloatLeft = true, string area = "")
            : base(title, controllerName, actionName, area)
        {
            this.IsFloatLeft = isFloatLeft;
        }

        /// <summary>
        /// Informa se o menu será fluido a direita
        /// </summary>
        public bool IsFloatLeft { get; set; }

    }
}
