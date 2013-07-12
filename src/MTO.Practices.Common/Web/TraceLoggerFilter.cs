namespace MTO.Practices.Common.Web
{
    using System.Web.Mvc;

    using MTO.Practices.Common.Debug;

    /// <summary>
    /// Filtro de logging de debug para requests
    /// </summary>
    public class TraceLoggerFilter : ActionFilterAttribute
    {
        /// <summary>
        /// Handler do evento de termino do request
        /// </summary>
        /// <param name="filterContext">Contexto do filtro</param>
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            DebugTracer.Flush(filterContext.RouteData.Values["controller"] as string);
        }
    }
}
