namespace MTO.Practices.Web.Mvc.Models
{
    /// <summary>
    /// Model para as actions do menu lateral
    /// </summary>
    public class SideMenuActionForItemViewModel
    {
        /// <summary>
        /// Título da action
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Action que será invocada
        /// </summary>
        public string Action { get; set; }

        /// <summary>
        /// Controller onde reside a action
        /// </summary>
        public string Controller { get; set; }

        /// <summary>
        /// Mensagem opcional de confirmação
        /// </summary>
        public string ConfirmationMessage { get; set; }

        /// <summary>
        /// Identificador do item atual
        /// </summary>
        public string Id { get; set; }
    }
}