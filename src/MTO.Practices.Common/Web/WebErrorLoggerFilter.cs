namespace MTO.Practices.Common.Web
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using System.Text;
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
        /// <param name="context">Contexto da exceção</param>
        public void OnException(ExceptionContext context)
        {
            if (context.Exception.GetBaseException() is HttpException)
            {
                var ex = (HttpException)context.Exception.GetBaseException();
                if (ex.GetHttpCode() == 404)
                {
                    return;
                }
            }

            var host = context.HttpContext.Request.Url != null ? context.HttpContext.Request.Url.Host : "unknown";
            Logger.Instance.LogError(context.Exception, host);
        }
    }
}
