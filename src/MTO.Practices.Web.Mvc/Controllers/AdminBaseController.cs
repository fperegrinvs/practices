namespace MTO.Practices.Web.Mvc.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Web.Mvc;

    using MTO.Practices.Common;
    using MTO.Practices.Web.Mvc.Models;

    /// <summary>
    /// Controller base com facilitadores para o Admin
    /// </summary>
    public abstract class AdminBaseController : Controller
    {
        /// <summary>
        /// Mensagem de sucesso padrão
        /// </summary>
        private const string SuccessMessageDefault = "Operação realizada com sucesso";

        /// <summary>
        /// Mensagem de erro padrão
        /// </summary>
        private const string ErrorMessageDefault = "Erro ao realizar operação. {0}";

        /// <summary>
        /// Título da seção regida pelo controle
        /// </summary>
        protected string SectionTitle { get; set; }

        /// <summary>
        /// Controla a exibição da TreeView de conteúdo na Sidebar
        /// </summary>
        protected bool ShowTreeView { get; set; }

        /// <summary>
        /// Actions do menu lateral
        /// </summary>
        protected List<KeyValuePair<string, string>> SideMenuActions { get; set; }

        /// <summary>
        /// Lista de ações do menu lateral para o item atualmente exibido
        /// </summary>
        protected List<SideMenuActionForItemViewModel> SideMenuActionsForItem { get; set; }

        /// <summary>
        /// Sets SuccessMessage.
        /// </summary>
        private string SuccessMessage
        {
            set
            {
                this.TempData["SuccessMessage"] = value;
            }
        }

        /// <summary>
        /// Sets ErrorMessage.
        /// </summary>
        private string ErrorMessage
        {
            set
            {
                this.TempData["ErrorMessage"] = value;
            }
        }

        /// <summary>
        /// Exibe mensagem de erro
        /// </summary>
        /// <param name="details">Detalhes do erro</param>
        protected void Error(string details)
        {
            this.ErrorMessage = string.Format(ErrorMessageDefault, details);
        }

        /// <summary>
        /// Exibe mensagem de sucesso
        /// </summary>
        protected void Success()
        {
            this.SuccessMessage = SuccessMessageDefault;
        }

        /// <summary>
        /// Exibe mensagem de sucesso personalizado
        /// </summary>
        /// <param name="mensagem">a mensagem</param>
        protected void Success(string mensagem)
        {
            this.SuccessMessage = mensagem;
        }

        /// <summary>
        /// Eventos de execução das actions
        /// </summary>
        /// <param name="filterContext">O contexto</param>
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.ActionParameters.ContainsKey("id") && this.SideMenuActionsForItem != null)
            {
                foreach (var action in this.SideMenuActionsForItem)
                {
                    action.Controller = action.Controller ?? filterContext.RouteData.Values["controller"].ToString();
                    action.Id = filterContext.ActionParameters["id"].ToString();
                }

                filterContext.Controller.ViewBag.SideMenuActionsForItem = this.SideMenuActionsForItem;
            }

            filterContext.Controller.ViewData["SectionTitle"] = this.SectionTitle;
            if (this.SideMenuActions != null && this.SideMenuActions.Count > 0)
            {
                filterContext.Controller.ViewBag.SideMenuActions = this.SideMenuActions;
            }

            filterContext.Controller.ViewBag.ShowTreeView = this.ShowTreeView;
            filterContext.Controller.ViewBag.CurrentAction = filterContext.ActionDescriptor.ActionName;
            filterContext.Controller.ViewBag.CurrentController = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;
            base.OnActionExecuting(filterContext);
        }

        /// <summary>
        /// Adiciona ações ao menu lateral
        /// </summary>
        /// <param name="title">Título da ação</param>
        /// <param name="action">Nome da action do controller</param>
        protected void AddMenuAction(string title, string action)
        {
            if (this.SideMenuActions == null)
            {
                this.SideMenuActions = new List<KeyValuePair<string, string>>();
            }

            this.SideMenuActions.Add(new KeyValuePair<string, string>(title, action));
        }

        /// <summary>
        /// Adiciona ações contextuais ao menu lateral. Essas ações são realizadas em cima do 
        /// item atualmente exibido, quando há um.
        /// </summary>
        /// <param name="title">Título da ação</param>
        /// <param name="action">Action chamada</param>
        /// <param name="controller">Controller (opcional) onde reside a action. Default é o atual. </param>
        /// <param name="message">Mensagem opcional de confirmação. Caso definida mostra um confirmation antes de proceder.</param>
        /// <param name="id">Identificador opcional pro item atual</param>
        protected void AddMenuActionForItem(string title, string action, string controller = null, string message = null, string id = null)
        {
            if (this.SideMenuActionsForItem == null)
            {
                this.SideMenuActionsForItem = new List<SideMenuActionForItemViewModel>();
            }

            var newAction = new SideMenuActionForItemViewModel { Title = title, Action = action, Controller = controller, ConfirmationMessage = message };

            this.SideMenuActionsForItem.Add(newAction);

            // Se estamos dentro de uma action
            if (this.ViewBag != null)
            {
                if (this.RouteData != null)
                {
                    if (this.RouteData.Values["id"] != null)
                    {
                        newAction.Id = this.RouteData.Values["id"].ToString();
                    }

                    if (id != null)
                    {
                        newAction.Id = id;
                    }

                    this.ViewBag.SideMenuActionsForItem = this.SideMenuActionsForItem;
                }
            }
        }

        /// <summary>
        /// Limpa a lista de ações do item atualmente exibido
        /// </summary>
        protected void ClearMenuActionsForItem()
        {
            if (this.SideMenuActionsForItem != null)
            {
                this.SideMenuActionsForItem.Clear();
            }

            this.ViewBag.SideMenuActionsForItem = null;
        }

        /// <summary>
        /// Executa uma ação capturando sucesso ou fracasso
        /// </summary>
        /// <typeparam name="T"> O tipo do model da view de sucesso </typeparam>
        /// <param name="func"> O metodo a ser executado </param>
        /// <param name="success"> O método de sucesso recebendo um model </param>
        /// <param name="failure"> O método de  falha recebendo um model </param>
        /// <returns>A actionResult </returns>
        protected ActionResult CaptureSucessFailure<T>(Func<T> func, Func<T, ActionResult> success, Func<ActionResult> failure = null) where T : class
        {
            try
            {
                var model = func();
                this.Success();

                return success(model);
            }
            catch (Exception ex)
            {
                Logger.Instance.LogException(ex);
                this.Error(ex.Message);
            }

            if (failure != null)
            {
                return failure();
            }

            return success(null);
        }

        /// <summary>
        /// Executa uma ação capturando sucesso ou fracasso
        /// </summary>
        /// <param name="func"> O metodo a ser executado </param>
        /// <param name="success"> O método de sucesso recebendo um model </param>
        /// <returns>A actionResult </returns>
        protected ActionResult CaptureSucessFailure(Action func, Func<ActionResult> success)
        {
            try
            {
                func();
                this.Success();
            }
            catch (Exception ex)
            {
                Logger.Instance.LogException(ex);
                this.Error(ex.Message);
            }

            return success();
        }
    }
}