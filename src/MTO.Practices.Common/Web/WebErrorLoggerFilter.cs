namespace MTO.Practices.Common.Web
{
    using System;
    using System.Web;
    using System.Web.Mvc;

    /// <summary>
    /// Filtro de logging usando elmah
    /// Fonte: http://ivanz.com/2011/05/08/asp-net-mvc-magical-error-logging-with-elmah/
    /// </summary>
    public class WebErrorLoggerFilter : IExceptionFilter
    {
        /// <summary>
        /// Handler do evento de exception
        /// </summary>
        /// <param name="filterContext">Contexto da exceção</param>
        public void OnException(ExceptionContext filterContext)
        {
            if (filterContext == null)
            {
                throw new ArgumentNullException();
            }

            if (filterContext.Exception.GetBaseException() is HttpException)
            {
                var ex = (HttpException)filterContext.Exception.GetBaseException();
                if (ex.GetHttpCode() == 404)
                {
                    return;
                }
            }

            var host = filterContext.HttpContext.Request.Url != null ? filterContext.HttpContext.Request.Url.Host : "unknown";
            Logger.Instance.LogError(filterContext.Exception, host);
        }
    }
}
