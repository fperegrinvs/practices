namespace MTO.Practices.Templating.Lexer
{
    using System.Collections.Generic;

    using MTO.Practices.Templating.Lexer.Enumerators;

    /// <summary>
    /// Tag incluso no template
    /// </summary>
    public class Tag
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Tag"/> class. 
        /// Cria nova tag
        /// </summary>
        /// <param name="name">
        /// Nome da tag
        /// </param>
        public Tag(string name)
        {
            this.Name = name;
            this.Arguments = new Dictionary<string, string>();
        }

        /// <summary>
        /// Nome da tag
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Estado atual da tag
        /// </summary>
        public TagStatusEnum TagStatus { get; set; }

        /// <summary>
        /// Parametros usados na tag
        /// </summary>
        public Dictionary<string, string> Arguments { get; set; }
    }
}
