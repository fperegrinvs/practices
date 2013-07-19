namespace MTO.Practices.Common.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;
    using System.Web.Mvc;
    using System.Web.Mvc.Html;

    public static class DropDownExtensions
    {
        private static Type GetNonNullableModelType(ModelMetadata modelMetadata)
        {
            Type realModelType = modelMetadata.ModelType;

            Type underlyingType = Nullable.GetUnderlyingType(realModelType);
            if (underlyingType != null)
            {
                realModelType = underlyingType;
            }

            return realModelType;
        }

        private static readonly SelectListItem[] SingleEmptyItem = new[] { new SelectListItem { Text = "", Value = "", Selected = true } };

        public static string GetEnumDescription<TEnum>(TEnum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());

            DescriptionAttribute[] attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);

            if ((attributes != null) && (attributes.Length > 0))
                return attributes[0].Description;
            else
                return value.ToString();
        }

        public static MvcHtmlString EnumDropDownListFor<TModel, TEnum>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TEnum>> expression)
        {
            return EnumDropDownListFor(htmlHelper, expression, null);
        }

        public static MvcHtmlString EnumDropDownListFor<TModel, TEnum>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TEnum>> expression, object htmlAttributes)
        {
            ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            Type enumType = GetNonNullableModelType(metadata);
            IEnumerable<TEnum> values = Enum.GetValues(enumType).Cast<TEnum>();

            int i = 0;
            IEnumerable<SelectListItem> items = from value in values
                                                select new SelectListItem
                                                {
                                                    Text = GetEnumDescription(value),
                                                    Value = i++.ToString(),
                                                    Selected = value.Equals(metadata.Model)
                                                };

            items = SingleEmptyItem.Concat(items);

            return htmlHelper.DropDownListFor(expression, items, htmlAttributes);
        }

        public static SelectList ToSelectList<T>(this T enumeration)
        {
            return ToSelectList<T>(enumeration, string.Empty);
        }

        public static SelectList ToSelectList<T>(this T enumeration, string selected)
        {
            var source = Enum.GetValues(typeof(T));

            var items = new Dictionary<object, string>();

            var displayAttributeType = typeof(DisplayAttribute);

            foreach (var value in source)
            {
                FieldInfo field = value.GetType().GetField(value.ToString());

                DisplayAttribute attrs = (DisplayAttribute)field.
                              GetCustomAttributes(displayAttributeType, false).First();

                items.Add(value, attrs.GetName());
            }

            if (!string.IsNullOrEmpty(selected))
                return new SelectList(items, "Key", "Value", selected);
            else
                return new SelectList(items, "Key", "Value");
        }

        public static SelectList ToSelectListDay<T>(this T enumeration, string selected)
        {
            var source = Enum.GetValues(typeof(T));

            var items = new Dictionary<object, string>();

            var displayAttributeType = typeof(DisplayAttribute);

            for (int i = 0; i < source.Length; i++)
            {
                FieldInfo field = source.GetValue(i).GetType().GetField(source.GetValue(i).ToString());
                
                DisplayAttribute attrs = (DisplayAttribute)field.
                              GetCustomAttributes(displayAttributeType, false).First();

                items.Add((int)source.GetValue(i), attrs.Name);
            }

            if (!string.IsNullOrEmpty(selected))
                return new SelectList(items, "Key", "Value", selected);
            else
                return new SelectList(items, "Key", "Value");
        }
    }
}
