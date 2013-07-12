namespace MTO.Practices.Common.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;
    using System.Web.Mvc;

    /// <summary>
    /// Métodos auxiliares para criação de checkboxes no MVC
    /// </summary>
    public static class FlagsExtensions
    {
        public static IEnumerable<SelectListItem> ToSelectList<TEnum, TAttributeType>(this TEnum enumObj) where TEnum : struct
        {
            Type type = typeof(TEnum);

            var fields = type.GetFields(BindingFlags.Static | BindingFlags.GetField | BindingFlags.Public);

            var values = from field in fields
                         select new SelectListItem
                         {
                             Value = field.GetRawConstantValue().ToString(),
                             Text = field.GetCustomAttributes(typeof(TAttributeType), true).FirstOrDefault().ToString(),
                             Selected = (Convert.ToInt32(field.GetRawConstantValue()) &
                                        Convert.ToInt32(enumObj)) ==
                                       Convert.ToInt32(field.GetRawConstantValue())
                         };


            return values;
        }

        /// <summary>
        /// Cria lista de checkboxes para uma propriedade que é um enumerador.
        /// </summary>
        /// <typeparam name="TModel">Tipo da model</typeparam>
        /// <typeparam name="TProperty">Tipo da propriedade</typeparam>
        /// <param name="htmlHelper">propriedade html helper</param>
        /// <param name="expression">Expressão linq</param>
        /// <returns>Dropdown list representando uma propriedade do tipo enumerador.</returns>
        public static MvcHtmlString FlagsCheckboxList<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression) where TModel : class where TProperty : struct
        {
            string inputName = SelectExtensions.GetInputName(expression);
            var value = htmlHelper.ViewData.Model == null
                ? default(TProperty)
                : expression.Compile()(htmlHelper.ViewData.Model);

            return MvcHtmlString.Create(htmlHelper.CheckBoxList(inputName, ToSelectList(value, true)));
        }

        public static IEnumerable<SelectListItem> ToSelectList<TEnum>(this TEnum enumObj, bool useIntegerValue) where TEnum : struct
        {
            Type type = typeof(TEnum);

            var values = from TEnum e in Enum.GetValues(typeof(TEnum))
                         orderby Convert.ToUInt32(Enum.Parse(type, e.ToString())).CountBits()
                         select
                             new SelectListItem
                             {
                                 Value =
                                     useIntegerValue
                                         ? Enum.Format(type, Enum.Parse(type, e.ToString()), "d")
                                         : e.ToString(),
                                 Text = e.EnumToString(),
                                 Selected = (Convert.ToInt32(e) & Convert.ToInt32(enumObj)) == Convert.ToInt32(e)
                             };


            return values;
        }

        /// <summary>
        /// Retorna uma string para um enumerador
        /// </summary>
        /// <param name="item">
        /// Item de enumeração
        /// </param>
        /// <typeparam name="T">
        /// Tipo do enumerador.
        /// </typeparam>
        /// <returns>
        /// Nome público do item.
        /// </returns>
        public static string EnumToString<T>(this T item)
        {
            var fi = typeof(T).GetField(item.ToString());
            var attribute = fi.GetCustomAttributes(typeof(DescriptionAttribute), true).FirstOrDefault();

            var enumName = Enum.GetName(typeof(T), item);
            var title = attribute == null ? enumName : ((DescriptionAttribute)attribute).Description;
            return title;
        }
    }
}
