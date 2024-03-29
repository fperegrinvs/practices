﻿namespace MTO.Practices.Templating.Lexer
{
    using System.Collections.Generic;

    using MTO.Practices.Templating.Lexer.Interfaces;
    using MTO.Practices.Templating.Lexer.Model;

    /// <summary>
    /// Classe base para implementação de template engine
    /// </summary>
    public abstract class TemplateEngineBase : ITemplateEngine
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TemplateEngineBase"/> class.
        /// </summary>
        protected TemplateEngineBase()
        {
            this.Stack = new Stack<Element>();
        }

        /// <summary>
        /// Estado atual do motor
        /// </summary>
        public StartEnum CurrentState { get; set; }

        /// <summary>
        /// Parser associado a engine
        /// </summary>
        public Parser Parser { get; set; }

        /// <summary>
        /// Tag atual
        /// </summary>
        public Element CurrentElement
        {
            get
            {
                return this.Stack.Count == 0 ? null : this.Stack.Peek();
            }
        }

        /// <summary>
        /// Pilha de elementos do template durante o processamento
        /// </summary>
        protected Stack<Element> Stack { get; set; }

        /// <summary>
        /// Adiciona nova tag à pilha de Tags
        /// </summary>
        /// <param name="tagName">nome da tag</param>
        public void NewTag(string tagName)
        {
            this.Stack.Push(new Element(tagName));
        }

        /// <summary>
        /// Inicia adição de novo parâmetro à tag atual
        /// </summary>
        /// <param name="name">nome do parâmetro</param>
        public void NewTagArg(string name)
        {
            this.Stack.Peek().StartArgument();
            this.ProcessContent(name);
        }

        /// <summary>
        /// Finaliza Tag atual
        /// </summary>
        /// <returns>
        /// The System.String.
        /// </returns>
        public virtual string EndTag()
        {
            this.Stack.Pop();
            return string.Empty;
        }

        /// <summary>
        /// Substitui propriedade (variável) pelo valor apropriado
        /// </summary>
        /// <param name="property">propriedade a ser substituida</param>
        /// <returns>retorno da substituição da propriedade</returns>
        public virtual string ReplaceProperty(string property)
        {
            return property;
        }

        /// <summary>
        /// Processa conteúdo
        /// </summary>
        /// <param name="content">
        /// conteúdo a ser processado
        /// </param>
        /// <param name="token">
        /// The token.
        /// </param>
        /// <returns>
        /// resultado do processamento de conteúdo
        /// </returns>
        public virtual string ProcessContent(string content, Tokens? token = null)
        {
            if (this.Stack.Count == 0)
            {
                return content;
            }

            if (this.Stack.Peek().IsActive)
            {
                this.Stack.Peek().Content += content;
                return "";
            }

            return this.Stack.Peek().ProcessContent(content) ?? "";
        }

        /// <summary>
        /// Processa conteúdo da tag
        /// </summary>
        /// <returns>resultado do processamento</returns>
        public abstract string ProcessTagContent();

        /// <summary>
        /// Processa tag e retorna o seu resultado
        /// </summary>
        /// <param name="content">conteúdo relacionado à tag</param>
        /// <returns>conteúdo processado</returns>
        public abstract string ProcessTag(string content);

        /// <summary>
        /// Processa outro tipo de token
        /// </summary>
        /// <param name="tokenType">tipo do token</param>
        /// <param name="tokenValue">valor associado ao token</param>
        /// <returns>resultado do processamento do token</returns>
        public virtual string ProcessOther(Tokens tokenType, string tokenValue)
        {
            return "";
        }

        /// <summary>
        /// Processa uma url
        /// </summary>
        /// <param name="content">endereço da url (incluindo <![CDATA[<a href="]]]>)</param>
        /// <returns>resultado do processamento</returns>
        public virtual string ProcessUrl(string content)
        {
            return content;
        }

        /// <summary>
        /// Evento disparado quando o fim do template é atingido
        /// </summary>
        public virtual void EOF()
        {
        }

        /// <summary>
        /// Inicia novo comando na pilha de comandos
        /// </summary>
        /// <param name="name">Nome do novo comando</param>
        public virtual void NewCommand(string name)
        {
            this.Stack.Push(new Element(name));
        }

        /// <summary>
        /// Inicia novo argumento
        /// </summary>
        public virtual void NewCommandArg()
        {
            this.Stack.Peek().StartArgument();
        }

        /// <summary>
        /// Inicia novo argumento de comando
        /// </summary>
        public virtual void NewCommandArgValue()
        {
        }

        /// <summary>
        /// Processa argumento ou conteúdo do comando
        /// </summary>
        public virtual void ProcessCommandContent()
        {
        }

        /// <summary>
        /// Processa o comando
        /// </summary>
        /// <returns> O resultado do processamento do comando </returns>
        public abstract string ProcessCommand();

        /// <summary>
        /// Inicio de comentário
        /// </summary>
        /// <param name="comment">
        /// tag de abertura do comentário
        /// </param>
        /// <returns>
        /// The start comment.
        /// </returns>
        public virtual string StartComment(string comment)
        {
            return comment;
        }

        /// <summary>
        /// Fim de comentário
        /// </summary>
        /// <param name="comment">
        /// tag que finaliza o comentário
        /// </param>
        /// <returns>
        /// The end comment.
        /// </returns>
        public virtual string EndComment(string comment)
        {
            return comment;
        }
    }
}
