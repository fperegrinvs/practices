namespace MTO.Practices.Templating.Lexer
{
    using System.Text;

    using MTO.Practices.Templating.Lexer.Enumerators;
    using MTO.Practices.Templating.Lexer.Interfaces;

    /// <summary>
    /// Faz parse de um template tokenizado
    /// </summary>
    public class Parser
    {
        /// <summary>
        /// Lista de tokens a ser processada
        /// </summary>
        private readonly TokenList tokens;

        /// <summary>
        /// BUilder com o resultado do parse
        /// </summary>
        private readonly StringBuilder output;

        /// <summary>
        /// Engine de processamento de template
        /// </summary>
        private readonly ITemplateEngine templateEngine;

        /// <summary>
        /// Indica se os argumentos da tag atual estão abertos.
        /// </summary>
        private bool openTagArg;

        /// <summary>
        /// Initializes a new instance of the <see cref="Parser"/> class.
        /// </summary>
        /// <param name="serializedTokens">
        /// The serialized tokens.
        /// </param>
        /// <param name="engine">
        /// The engine.
        /// </param>
        public Parser(string serializedTokens, ITemplateEngine engine)
            : this(new TokenList(serializedTokens), engine)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Parser"/> class. 
        /// </summary>
        /// <param name="tokens">
        /// tokens a serem processados
        /// </param>
        /// <param name="engine">engine a ser utilizada no processamento do template</param>
        public Parser(TokenList tokens, ITemplateEngine engine)
        {
            this.tokens = tokens;
            this.output = new StringBuilder(tokens.Count);
            this.templateEngine = engine;
            engine.Parser = this;
        }

        /// <summary>
        /// Processa os tokens existentes
        /// </summary>
        /// <returns>string com o resultado do processamento</returns>
        public string ProcessTokenList()
        {
            var tagName = string.Empty;
            var iter = this.tokens.GetEnumerator();
            while (iter.MoveNext())
            {
                var level = 1;
                while (iter.Current != null && this.templateEngine.CurrentElement != null && this.templateEngine.CurrentElement.ElementStatus != ElementStatusEnum.Default)
                {
                    // registra a tag atual
                    if (tagName == string.Empty)
                    {
                        tagName = this.templateEngine.CurrentElement.Name;
                    }

                    if (iter.Current.State == (int)Tokens.TagName && tagName == iter.Current.Content)
                    {
                        ++level;
                    }

                    // fim da tag
                    if (iter.Current.State == (int)Tokens.CloseMtoTag && iter.Current.Content.Length > 3 && iter.Current.Content.Contains(tagName) && --level == 0)
                    {
                        tagName = string.Empty;
                        break;
                    }

                    if (this.templateEngine.CurrentElement.ElementStatus == ElementStatusEnum.SkipContent)
                    {
                        // pula processamento do conteúdo da tag
                    }
                    else
                    {
                        // tag é processado como conteúdo
                        var token = iter.Current;
                        var tresult = this.templateEngine.ProcessContent(token.Content, (Tokens)iter.Current.State);
                        if (!string.IsNullOrEmpty(tresult))
                        {
                            this.output.Append(tresult);
                        }
                    }

                    iter.MoveNext();
                }

                if (iter.Current == null)
                {
                    break;
                }

                var tokenResult = this.ProcessToken(iter.Current);
                if (!string.IsNullOrEmpty(tokenResult))
                {
                    this.output.Append(tokenResult);
                }
            }

            return this.output.ToString();
        }

        /// <summary>
        /// Processa um token
        /// </summary>
        /// <param name="token">
        /// The token.
        /// </param>
        /// <returns>
        /// resultado do processamento do token
        /// </returns>
        internal string ProcessToken(PToken token)
        {
            switch ((Tokens)token.State)
            {
                case Tokens.NewLine:
                case Tokens.OpenHtmlTag:
                case Tokens.CloseHtmlTag:
                case Tokens.Content:
                    return this.templateEngine.ProcessContent(token.Content);
                case Tokens.CommentStart:
                    return this.templateEngine.StartComment(token.Content);
                case Tokens.CommentEnd:
                    return this.templateEngine.EndComment(token.Content);
                case Tokens.TagArg:
                    this.openTagArg = true;
                    this.templateEngine.NewTagArg(token.Content);
                    break;
                case Tokens.TagCloseArg:
                    this.openTagArg = false;
                    return this.templateEngine.ProcessTag(string.Empty);
                case Tokens.CloseMtoTag:
                    if (this.openTagArg)
                    {
                        this.openTagArg = false;
                        var result = this.templateEngine.ProcessTag(string.Empty);
                        if (!string.IsNullOrEmpty(result))
                        {
                            this.output.Append(result);
                        }
                    }

                    return this.templateEngine.ProcessTagContent();
                case Tokens.TagName:
                    this.templateEngine.NewTag(token.Content);
                    break;
                case Tokens.OpenMtoTag:
                    return token.Start == 0 ? this.templateEngine.ProcessOther(Tokens.OpenMtoTag, token.Content) : string.Empty;
                case Tokens.EOF:
                    this.templateEngine.EOF();
                    break;
                case Tokens.Property:
                    return this.templateEngine.ReplaceProperty(token.Content);
                case Tokens.Url:
                    return this.templateEngine.ProcessUrl(token.Content);
                case Tokens.OpenCommand:
                    this.templateEngine.NewCommand(token.Content);
                    break;
                case Tokens.OpenCommandContent:
                case Tokens.OpenCommandArg:
                    this.templateEngine.NewCommandArg();
                    break;
                case Tokens.OpenComandArgValue:
                    this.templateEngine.NewCommandArgValue();
                    break;
                case Tokens.CloseCommandContent:
                    this.templateEngine.ProcessCommandContent();
                    break;
                case Tokens.CloseCommand:
                    return this.ProcessToken(new PToken { Content = this.templateEngine.ProcessCommand(), State = (int)Tokens.Content });
            }

            return string.Empty;
        }
    }
}
