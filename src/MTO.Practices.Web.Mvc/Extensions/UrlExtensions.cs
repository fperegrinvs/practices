namespace MTO.Practices.Common.Extensions
{
    using System.Configuration;
    using System.Web.Mvc;

    public static class UrlExtensions
    {
       public static string ThemedContent(this UrlHelper urlHelper, string path)
        {
            if (!path.StartsWith("/"))
            {
                path = "/" + path;
            }

            path = "~/Content/" + ConfigurationManager.AppSettings["Theme"] + path;
            return urlHelper.Content(path);
        }
    }
}
