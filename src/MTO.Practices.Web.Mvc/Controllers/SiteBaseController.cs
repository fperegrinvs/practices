namespace MTO.Practices.Web.Mvc.Controllers
{
    using System.IO;
    using System.Web.Mvc;

    /// <summary>
    /// The site base controller.
    /// </summary>
    public class SiteBaseController : Controller
    {
        /// <summary>
        /// Renderiza uma view para uma string
        /// </summary>
        /// <param name="viewName">nome da views</param>
        /// <param name="model">dados da view</param>
        /// <returns>string com a view renderizada</returns>
        protected string RenderPartialViewToString(string viewName, object model)
        {
            if (string.IsNullOrEmpty(viewName))
            {
                viewName = this.ControllerContext.RouteData.GetRequiredString("action");
            }

            this.ViewData.Model = model;

            using (var sw = new StringWriter())
            {
                var viewResult = ViewEngines.Engines.FindPartialView(this.ControllerContext, viewName);
                var viewContext = new ViewContext(this.ControllerContext, viewResult.View, this.ViewData, this.TempData, sw);
                viewResult.View.Render(viewContext, sw);

                return sw.GetStringBuilder().ToString();
            }
        }
    }
}