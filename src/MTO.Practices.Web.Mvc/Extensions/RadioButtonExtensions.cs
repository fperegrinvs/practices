using System;
using System.Collections.Generic;
using System.Text;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace MTO.Practices.Common.Extensions
{
    public static class RadioButtonExtensions
    {
        public static MvcHtmlString RadioButtonForSelectList<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IEnumerable<SelectListItem> listOfValues)
        {
            var metaData = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            var sb = new StringBuilder();

            if (listOfValues != null)
            {
                foreach (SelectListItem item in listOfValues)
                {
                    var id = string.Format(
                        "{0}_{1}",
                        metaData.PropertyName,
                        item.Value
                    );

                    var radio = htmlHelper.RadioButtonFor(expression, item.Value, new { id = id }).ToHtmlString();
                    sb.AppendFormat(
                        "<span>{0}<label for=\"{1}\">{2}</label></span>",
                        radio,
                        id,
                        HttpUtility.HtmlEncode(item.Text)
                    );
                }
            }

            return MvcHtmlString.Create(sb.ToString());
        }

        public static MvcHtmlString RadioButtonForSelectList<TModel, TProperty>(
            this HtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TProperty>> expression,
            IEnumerable<SelectListItem> listOfValues,
            string imagePath)
        {
            var metaData = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            var sb = new StringBuilder();

            if (listOfValues != null)
            {
                foreach (SelectListItem item in listOfValues)
                {
                    var id = string.Format(
                        "{0}_{1}",
                        metaData.PropertyName,
                        item.Value
                    );

                    var radio = htmlHelper.RadioButtonFor(expression, item.Value, new { id = id }).ToHtmlString();
                    sb.AppendFormat(
                        "<span><label for=\"{1}\"><img src=\"" + imagePath + "{2}.png\" alt=\"{2}\" /></label>{0}</span>",
                        radio,
                        id,
                        HttpUtility.HtmlEncode(item.Text)
                    );
                }
            }

            return MvcHtmlString.Create(sb.ToString());
        }
    }
}
