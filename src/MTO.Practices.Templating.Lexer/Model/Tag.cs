namespace MTO.Practices.Templating.Lexer.Model
{
    using System;
    using System.Collections.Generic;

    using MTO.Practices.Common;
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
            this.Arguments = new Stack<KeyValue<string, string>>();
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
        public Stack<KeyValue<string, string>> Arguments { get; set; }

        /// <summary>
        /// Processa conteúdo recebido durante o contexto de processamento do elemento
        /// </summary>
        /// <param name="content">O conteúdo</param>
        /// <returns>O resultado</returns>
        public string ProcessContent(string content)
        {
            if (this.Arguments.Count == 0)
            {
                return content;
            }

            var arg = this.Arguments.Peek();
            if (string.IsNullOrEmpty(arg.Key))
            {
                arg.Key = content;
            }
            else
            {
                arg.Value = content;
            }

            return null;
        }

        /// <summary>
        /// Inicia novo argumento quando nao sabemos nome nem valor
        /// </summary>
        public void StartArgument()
        {
            this.Arguments.Push(new KeyValue<string, string>());
        }
    }
}
