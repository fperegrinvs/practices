namespace MTO.Practices.Templating.Lexer.Tests.Engine
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;

    using MTO.Practices.Common.Extensions;
    using MTO.Practices.Templating.Lexer.Enumerators;
    using MTO.Practices.Templating.Lexer.Interfaces;

    /// <summary>
    /// Engine de renderização pra testes
    /// </summary>
    public class TestEngine : TemplateEngineBase
    {
        /// <summary>
        /// índice do elemento atual
        /// </summary>
        private int elementIndex = -1;

        /// <summary>
        /// Indícce máximo processado até o momento
        /// </summary>
        private int maxIndex = -1;

        /// <summary>
        /// Primeiro elemento a ser processado
        /// </summary>
        private int minIndex;

        /// <summary>
        /// Initializes a new instance of the <see cref="TestEngine"/> class.
        /// </summary>
        public TestEngine()
        {
            this.TemplateKeys = new List<Dictionary<string, string>>();
        }

        /// <summary>
        /// Valores associados ao template
        /// </summary>
        public List<Dictionary<string, string>> TemplateKeys { get; set; }

        /// <summary>
        /// Substitui propriedade (variável) pelo valor apropriado
        /// </summary>
        /// <param name="property">propriedade a ser substituida</param>
        /// <returns>retorno da substituição da propriedade</returns>
        public override string ReplaceProperty(string property)
        {
            if (this.TemplateKeys[this.elementIndex].ContainsKey(property))
            {
                return this.TemplateKeys[this.elementIndex][property];
            }

            throw new ArgumentException("Propriedade desconhecida :" + property);
        }

        /// <summary>
        /// Processa comando e retorna o seu resultado
        /// </summary>
        /// <param name="content">conteúdo relacionado ao comando</param>
        /// <returns>conteúdo processado</returns>
        public override string ProcessTag(string content)
        {
            return this.CurrentElement.ToJson();
        }

        /// <summary>
        /// Processa o comando
        /// </summary>
        /// <returns> O resultado do processamento do comando </returns>
        public override string ProcessCommand()
        {
            var result = this.CurrentElement.ToJson();
            Stack.Pop();
            return result;
        }

        /// <summary>
        /// Processa uma url
        /// </summary>
        /// <param name="content">endereço da url (incluindo <![CDATA[<a href="]]]>)</param>
        /// <returns>resultado do processamento</returns>
        public override string ProcessUrl(string content)
        {
            return content;
        }

        /// <summary>
        /// Fim do template
        /// </summary>
        public void EOF()
        {
            // Avança a página
            this.minIndex = this.maxIndex + 1;
            this.elementIndex = -1;
            this.Parser.ProcessTokenList();
        }
    }
}
