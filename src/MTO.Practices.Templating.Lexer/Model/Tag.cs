namespace MTO.Practices.Templating.Lexer.Model
{
    using System.Collections.Generic;

    using MTO.Practices.Templating.Lexer.Enumerators;
    using MTO.Practices.Templating.Lexer.Interfaces;

    /// <summary>
    /// Tag incluso no template
    /// </summary>
    public class Tag : ITemplateElement
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
        public ElementStatusEnum ElementStatus { get; set; }

        /// <summary>
        /// Parametros usados na tag
        /// </summary>
        public Dictionary<string, string> Arguments { get; set; }

        /// <summary>
        /// Processa conteúdo recebido durante o contexto de processamento do elemento
        /// </summary>
        /// <param name="content">O conteúdo</param>
        /// <returns>O resultado</returns>
        public string ProcessContent(string content)
        {
            return null;
        }

        /// <summary>
        /// Adiciona um novo argumetno
        /// </summary>
        /// <param name="name">Nome do argumento</param>
        /// <param name="value">Valor do argumento</param>
        public void AddArgument(string name, string value)
        {
            this.Arguments.Add(name, value);
        }

        /// <summary>
        /// Inicia novo argumento quando nao sabemos nome nem valor
        /// </summary>
        public void StartArgument()
        {
            throw new System.NotImplementedException();
        }
    }
}
