namespace MTO.Practices.Common.Templating.AttributeRenderer
{
    using System;

    using Antlr3.ST;

    /// <summary>
    /// Renderizador de atributos de operações de data
    /// </summary>
    public class DateOpRenderer : IAttributeRenderer
    {
        /// <summary>
        /// Converte o atributo para string sem formatação
        /// </summary>
        /// <param name="o">O atributo</param>
        /// <returns>A string</returns>
        public string ToString(object o)
        {
            return this.ToString(o, null);
        }

        /// <summary>
        /// Converte o atributo para string sem formatação
        /// </summary>
        /// <param name="o">O atributo</param>
        /// <param name="formatName">Nome da formatação a ser aplicada</param>
        /// <returns>A string</returns>
        public string ToString(object o, string formatName)
        {
            if (o == null)
            {
                return null;
            }

            if (string.IsNullOrEmpty(formatName))
            {
                return o.ToString();
            }

            int i;
            if (formatName == "date" && int.TryParse(o.ToString(), out i))
            {
                return DateTime.Now.AddDays(i).ToShortDateString();
            }

            return o.ToString();
        }
    }
}
