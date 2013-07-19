using System.Web;
using System.Web.Mvc;

namespace MTO.Tools.CreatePracticesAssembly
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}