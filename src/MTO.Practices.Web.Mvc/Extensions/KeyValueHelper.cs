namespace MTO.Practices.Web.Mvc.Extensions
{
    using System.Collections.Generic;
    using System.Web.Mvc;

    using MTO.Practices.Common;

    public static class KeyValueHelper
    {
        public static JsonResult ToJsonResult<T, TU>(this IEnumerable<KeyValue<T, TU>> keyValues)
        {
            return new JsonResult { Data = new SelectList(keyValues, "Key", "Value") };
        }

        public static SelectList ToSelectList<T, TU>(this IEnumerable<KeyValue<T, TU>> keyValues)
        {
            return new SelectList(keyValues, "Key", "Value");
        }
    }
}
