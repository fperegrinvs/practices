namespace MTO.Practices.Common.Web
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Web.Routing;

    /// <summary>
    /// Classe de constraint de rota
    /// </summary>
    public class RegexRouteConstraint : IRouteConstraint, IEquatable<RegexRouteConstraint>
    {
        /// <summary>
        /// Regex da constraint
        /// </summary>
        private readonly Regex regex;

        /// <summary>
        /// Initializes a new instance of the <see cref="RegexRouteConstraint"/> class.
        /// </summary>
        /// <param name="pattern">
        /// The pattern.
        /// </param>
        /// <param name="options">
        /// The options.
        /// </param>
        public RegexRouteConstraint(string pattern, RegexOptions options = RegexOptions.CultureInvariant | RegexOptions.Compiled | RegexOptions.IgnoreCase)
        {
            this.regex = new Regex(pattern, options);
        }

        /// <summary>
        /// Verifica se o valor do parâmetro da rota atende a constraint
        /// </summary>
        /// <param name="httpContext">Contexto HTTP atual</param>
        /// <param name="route">A rota</param>
        /// <param name="parameterName">O parâmetro</param>
        /// <param name="values">Os valores dos parâmetros informados</param>
        /// <param name="routeDirection">O routedirection</param>
        /// <returns>Verdadeiro caso o valor do parâmetro atenda a constraint</returns>
        public bool Match(System.Web.HttpContextBase httpContext, Route route, string parameterName, RouteValueDictionary values, RouteDirection routeDirection)
        {
            object val;
            values.TryGetValue(parameterName, out val);
            string input = Convert.ToString(val, CultureInfo.InvariantCulture);
            return this.regex.IsMatch(input);
        }

        /// <summary>
        /// Comparação entre RegexRouteConstraint
        /// </summary>
        /// <param name="other">O objeto com o qual comparamos</param>
        /// <returns>Verdadeiro se são iguais</returns>
        public bool Equals(RegexRouteConstraint other)
        {
            if (other == null)
            {
                throw new ArgumentNullException("other");
            }

            return other.regex == this.regex;
        }
    }
}
