namespace MTO.Practices.Common.Extensions
{
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Web.Mvc;

    using MTO.Practices.WebModule;

    /// <summary>
    /// Extensores de permissionamento
    /// </summary>
    public static class HtmlExtensions
    {
        /// <summary>
        /// Returns true if a specific controller action exists and
        /// the user has the ability to access it.
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="actionName"></param>
        /// <param name="controllerName"></param>
        /// <returns></returns>
        public static bool HasActionPermission(this HtmlHelper htmlHelper, string actionName, string controllerName)
        {
            //if the controller name is empty the ASP.NET convention is:
            //"we are linking to a different controller
            ControllerBase controllerToLinkTo = string.IsNullOrEmpty(controllerName)
                                                    ? htmlHelper.ViewContext.Controller
                                                    : GetControllerByName(htmlHelper, controllerName);

            var controllerContext = new ControllerContext(htmlHelper.ViewContext.RequestContext, controllerToLinkTo);

            var controllerDescriptor = new ReflectedControllerDescriptor(controllerToLinkTo.GetType());

            var actionDescriptor = controllerDescriptor.FindAction(controllerContext, actionName);

            return ActionIsAuthorized(controllerContext, actionDescriptor);
        }


        private static bool ActionIsAuthorized(ControllerContext controllerContext, ActionDescriptor actionDescriptor)
        {
            if (actionDescriptor == null) return false; // action does not exist so say yes - should we authorise this?!

            var authContext = new AuthorizationContext(controllerContext, actionDescriptor);

            // run each auth filter until one fails
            // performance could be improved by some caching
            var authorizeAttributes =
                FilterProviders.Providers.GetFilters(controllerContext, actionDescriptor).Where(
                    x => x.Instance is AuthorizeActivityAttribute).Select(x => x.Instance);
            foreach (AuthorizeActivityAttribute authFilter in authorizeAttributes)
            {
                authFilter.OnAuthorization(authContext);

                if (authContext.Result != null) return false;
            }

            authorizeAttributes =
                FilterProviders.Providers.GetFilters(controllerContext, actionDescriptor).Where(
                    x => x.Instance is AuthorizeAttribute).Select(x => x.Instance);
            foreach (AuthorizeAttribute authFilter in authorizeAttributes)
            {
                authFilter.OnAuthorization(authContext);

                if (authContext.Result != null) return false;
            }

            return true;
        }

        private static ControllerBase GetControllerByName(HtmlHelper helper, string controllerName)
        {
            // Instantiate the controller and call Execute
            IControllerFactory factory = ControllerBuilder.Current.GetControllerFactory();

            // se o fallback não é setado, dá erro quando estamos no contexto de uma área e estamos procurando permissões de um controller genérico.
            // habilitar o fallback apenas extende a busca quando um resultado não é encontrado.
            helper.ViewContext.RequestContext.RouteData.DataTokens["UseNamespaceFallback"] = true;

            IController controller = factory.CreateController(helper.ViewContext.RequestContext, controllerName);

            if (controller == null)
            {
                throw new InvalidOperationException(
                    String.Format(
                        CultureInfo.CurrentUICulture,
                        "Controller factory {0} controller {1} returned null",
                        factory.GetType(),
                        controllerName));

            }

            return (ControllerBase)controller;
        }
    }
}