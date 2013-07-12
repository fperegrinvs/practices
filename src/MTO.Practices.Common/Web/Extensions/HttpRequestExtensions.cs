using System.Web;

namespace MTO.Practices.Common.Extensions
{
    public static class HttpRequestExtensions
    {
        public static bool HasFile(this HttpPostedFileBase file)
        {
            return (file != null && file.ContentLength > 0);
        }
    }
}
