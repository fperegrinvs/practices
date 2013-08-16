namespace MTO.Practices.Templating.Lexer
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
            this.TagStack = new Stack<Tag>();
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
        public Tag CurrentTag
        {
            get
            {
                return this.TagStack.Count == 0 ? null : this.TagStack.Peek();
            }
        }

        /// <summary>
        /// Pilha de Tags
        /// </summary>
        protected Stack<Tag> TagStack { get; set; }

        /// <summary>
        /// Adiciona nova tag à pilha de Tags
        /// </summary>
        /// <param name="tagName">nome da tag</param>
        public void NewTag(string tagName)
        {
            this.TagStack.Push(new Tag(tagName));
        }

        /// <summary>
        /// Adiciona novo parâmetro aa tag atual
        /// </summary>
        /// <param name="argument">nome do parâmetro</param>
        /// <param name="value">valor do parâmetro</param>
        public void AddTagArg(string argument, string value)
        {
            this.TagStack.Peek().Arguments[argument] = value;
        }

        /// <summary>
        /// Finaliza Tag atual
        /// </summary>
        /// <returns>
        /// The System.String.
        /// </returns>
        public virtual string EndTag()
        {
            this.TagStack.Pop();
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
