//namespace MTO.Practices.Common.Extensions
//{
//    using System;
//    using System.Collections.Generic;
//    using System.ComponentModel;
//    using System.Linq;
//    using System.Linq.Expressions;
//    using System.Web.Mvc;
//    using System.Web.Mvc.Html;

//    /// <summary>
//    /// Métodos auxiliares para criação de dropdown lists no MVC
//    /// </summary>
//    public static class SelectExtensions
//    {
//        /// <summary>
//        /// Retorna o nome de um enumerador
//        /// </summary>
//        /// <typeparam name="TModel">Tipo da model</typeparam>
//        /// <typeparam name="TProperty">Tipo do enumerador</typeparam>
//        /// <param name="expression">Expressao linq</param>
//        /// <returns>Nome do campo.</returns>
//        public static string GetInputName<TModel, TProperty>(Expression<Func<TModel, TProperty>> expression)
//        {
//            if (expression.Body.NodeType == ExpressionType.Call)
//            {
//                var methodCallExpression = (MethodCallExpression)expression.Body;
//                var name = GetInputName(methodCallExpression);
//                return name.Substring(expression.Parameters[0].Name.Length + 1);
//            }

//            return expression.Body.ToString().Substring(expression.Parameters[0].Name.Length + 1);
//        }

//        /// <summary>
//        /// Cria um dropdown para uma propriedade que é um enumerador.
//        /// </summary>
//        /// <typeparam name="TModel">Tipo da model</typeparam>
//        /// <typeparam name="TProperty">Tipo da propriedade</typeparam>
//        /// <param name="htmlHelper">propriedade html helper</param>
//        /// <param name="expression">Expressão linq</param>
//        /// <returns>Dropdown list representando uma propriedade do tipo enumerador.</returns>
//        public static MvcHtmlString EnumDropDownListFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression) where TModel : class
//        {
//            string inputName = GetInputName(expression);
//            var value = htmlHelper.ViewData.Model == null
//                ? default(TProperty)
//                : expression.Compile()(htmlHelper.ViewData.Model);

//            return htmlHelper.DropDownList(inputName, ToSelectList(value));
//        }

//        /// <summary>
//        /// Cria lista de opções para um enumerador.
//        /// </summary>
//        /// <typeparam name="T">
//        /// Tipo do enumerador
//        /// </typeparam>
//        /// <param name="selectedItem">
//        /// Item selecionado
//        /// </param>
//        /// <returns>
//        /// Lista de itens de um dropdown
//        /// </returns>
//        public static SelectList ToSelectList<T>(T selectedItem)
//        {
//            if (!typeof(T).IsEnum)
//            {
//                throw new InvalidEnumArgumentException("The specified type is not an enum");
//            }

//            var selectedItemName = Enum.GetName(typeof(T), selectedItem);
//            var items = new List<SelectListItem>();
//            foreach (var item in Enum.GetValues(typeof(T)))
//            {
//                var enumName = Enum.GetName(typeof(T), item);

//                var listItem = new SelectListItem
//                {
//                    Value = enumName,
//                    Text = ((T)item).EnumToString(),
//                    Selected = selectedItemName == enumName
//                };
//                items.Add(listItem);
//            }

//            return new SelectList(items, "Value", "Text");
//        }

//        /// <summary>
//        /// Retorna o nome do campo de enumeração.
//        /// </summary>
//        /// <param name="expression">Expressão linq</param>
//        /// <returns>Nome do campo de enumeração.</returns>
//        private static string GetInputName(MethodCallExpression expression)
//        {
//            var methodCallExpression = expression.Object as MethodCallExpression;
//            if (methodCallExpression != null)
//            {
//                return GetInputName(methodCallExpression);
//            }
//            return expression.Object.ToString();
//        }
//    }
//}
