namespace MTO.Templating.Lexer
{
    using System.Collections.Generic;
    using System.Text;

    /// <summary>
    /// Classe base para implementação de template engine
    /// </summary>
    public class TemplateEngineBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TemplateEngineBase"/> class.
        /// </summary>
        public TemplateEngineBase()
        {
            this.CommandStack = new Stack<Command>();
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
        /// Comando atual
        /// </summary>
        public Command CurrentCommand
        {
            get
            {
                return this.CommandStack.Count == 0 ? null : this.CommandStack.Peek();
            }
        }

        /// <summary>
        /// Pilha de comandos
        /// </summary>
        protected Stack<Command> CommandStack { get; set; }

        /// <summary>
        /// Adiciona novo comando à pilha de comandos
        /// </summary>
        /// <param name="commandName">nome do commando</param>
        public void NewCommand(string commandName)
        {
            this.CommandStack.Push(new Command(commandName));
        }

        /// <summary>
        /// Adiciona novo parâmetro ao comando atual
        /// </summary>
        /// <param name="argument">nome do parâmetro</param>
        /// <param name="value">valor do parâmetro</param>
        public void AddCommandArg(string argument, string value)
        {
            this.CommandStack.Peek().Arguments[argument] = value;
        }

        /// <summary>
        /// Finaliza comando atual
        /// </summary>
        /// <returns>
        /// The System.String.
        /// </returns>
        public virtual string EndCommand()
        {
            this.CommandStack.Pop();
            return string.Empty;
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
            return content;
        }

        /// <summary>
        /// Processa outro tipo de token
        /// </summary>
        /// <param name="tokenType">tipo do token</param>
        /// <param name="tokenValue">valor associado ao token</param>
        /// <returns>resultado do processamento do token</returns>
        public virtual string ProcessOther(Tokens tokenType, string tokenValue)
        {
            return string.Empty;
        }

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
