namespace MTO.Practices.WebModule
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// The menu item.
    /// </summary>
    public class MenuItem
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MenuItem"/> class.
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
        public MenuItem(string title, string controllerName, string actionName, string area = "")
        {
            this.Title = title;
            this.Action = new ActionReference(controllerName, actionName, area);
            this.Items = new List<MenuItem>();
        }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the action.
        /// </summary>
        public ActionReference Action { get; set; }

        /// <summary>
        /// Gets or sets the required permission.
        /// </summary>
        public ActionReference PermissionAction { get; set; }

        /// <summary>
        /// Função para validar permissão de um item do menu
        /// </summary>
        public Func<bool> PermissionFunction { get; set; }

        /// <summary>
        /// Gets the items.
        /// </summary>
        public List<MenuItem> Items { get; private set; }

    }
}
