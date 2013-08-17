namespace MTO.Practices.Templating.Lexer.Model
{
    using System;
    using System.Collections.Generic;

    using MTO.Practices.Common;
    using MTO.Practices.Templating.Lexer.Enumerators;
    using MTO.Practices.Templating.Lexer.Interfaces;

    /// <summary>
    /// The command.
    /// </summary>
    public class Command : ITemplateElement
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Command"/> class. 
        /// Cria nova tag
        /// </summary>
        /// <param name="name"> The name. </param>
        public Command(string name)
        {
            this.Name = name;
            this.Arguments = new Stack<KeyValue<string, string>>();
        }

        /// <summary>
        /// Nome da tag
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Indica se o elemento está ativo.
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Estado atual do comando
        /// </summary>
        public ElementStatusEnum ElementStatus { get; set; }

        /// <summary>
        /// Parametros usados no comando
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
                throw new InvalidOperationException("Content recebido fora de contexto de argumento.");
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
