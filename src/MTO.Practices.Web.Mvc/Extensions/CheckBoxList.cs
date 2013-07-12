namespace MTO.Practices.Common.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Web.Mvc;
    using System.Web.Routing;

    /// <summary>
    /// Implementação do CheckBoxList
    /// </summary>
    public static partial class HtmlHelperExtensions
    {
        public static string CheckBoxList(this HtmlHelper htmlHelper, string name, IEnumerable<SelectListItem> listInfo)
        {
            return htmlHelper.CheckBoxList(name, listInfo,
                ((IDictionary<string, object>)null));
        }

        public static string CheckBoxList(this HtmlHelper htmlHelper, string name, IEnumerable<SelectListItem> listInfo, object htmlAttributes)
        {
            return htmlHelper.CheckBoxList(name, listInfo,
                ((IDictionary<string, object>)new RouteValueDictionary(htmlAttributes)));
        }

        public static string CheckBoxList(this HtmlHelper htmlHelper, string name, IEnumerable<SelectListItem> listInfo, IDictionary<string, object> htmlAttributes)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException("The argument must have a value", "name");
            }

            if (listInfo == null)
            {
                throw new ArgumentNullException("listInfo");
            }

            var sb = new StringBuilder();

            foreach (SelectListItem info in listInfo)
            {
                var builder = new TagBuilder("input");
                if (info.Selected)
                {
                    builder.MergeAttribute("checked", "checked");
                }

                builder.MergeAttributes(htmlAttributes);
                builder.MergeAttribute("type", "checkbox");
                builder.MergeAttribute("value", info.Value);
                builder.MergeAttribute("name", name);
                builder.InnerHtml = info.Text;
                sb.Append(builder.ToString(TagRenderMode.Normal));
                sb.Append("<br />");
            }

            return sb.ToString();
        }
    }
}
