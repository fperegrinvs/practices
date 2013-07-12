// -----------------------------------------------------------------------
// <copyright file="HttpRequestBaseExtenders.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace MTO.Practices.Common.Web
{
    using System;
    using System.Web;

    /// <summary>
    /// Extenders do HttpRequestBase
    /// </summary>
    public static class HttpRequestBaseExtenders
    {
        public static bool AcceptsGZip(this HttpRequestBase request)
        {
            if (request == null)
            {
                throw new ArgumentNullException("request");
            }

            var acceptEncoding = request.Headers["Accept-Encoding"];

            if (string.IsNullOrEmpty(acceptEncoding))
            {
                return false;
            }

            acceptEncoding = acceptEncoding.ToUpperInvariant();
            return acceptEncoding.Contains("GZIP");
        }

        public static bool AcceptsGZip(this HttpRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException("request");
            }

            var acceptEncoding = request.Headers["Accept-Encoding"];

            if (string.IsNullOrEmpty(acceptEncoding))
            {
                return false;
            }

            acceptEncoding = acceptEncoding.ToUpperInvariant();
            return acceptEncoding.Contains("GZIP");
        }

        public static bool IsAjaxRequest(this HttpRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException("request");
            }

            return (request["X-Requested-With"] == "XMLHttpRequest") || ((request.Headers != null) && (request.Headers["X-Requested-With"] == "XMLHttpRequest"));
        }
    }
}
