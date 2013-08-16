namespace MTO.Templating.Lexer
{
    using System.Text;

    using MTO.Templating.Lexer.Enumerators;
    using MTO.Templating.Lexer.Interfaces;

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
        /// Indica se os argumentos do comando atual estão abertos.
        /// </summary>
        private bool openCommandArg = false;

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
            var commandName = string.Empty;
            var iter = this.tokens.GetEnumerator();
            while (iter.MoveNext())
            {
                var level = 1;
                while (this.templateEngine.CurrentCommand != null && this.templateEngine.CurrentCommand.CommandStatus != CommandStatusEnum.Default)
                {
                    // registra o comando atual
                    if (commandName == string.Empty)
                    {
                        commandName = this.templateEngine.CurrentCommand.Name;
                    }

                    if (iter.Current.State == (int)Tokens.CommandName && commandName == iter.Current.Content)
                    {
                        ++level;
                    }

                    // fim do comando
                    if (iter.Current.State == (int)Tokens.CommandCloseTag && commandName == iter.Current.Content && --level == 0)
                    {
                        commandName = string.Empty;
                        break;
                    }

                    if (this.templateEngine.CurrentCommand.CommandStatus == CommandStatusEnum.SkipContent)
                    {
                        // pula processamento do conteúdo do comando
                    }
                    else
                    {
                        // comando é processado como conteúdo
                        var token = iter.Current;
                        var tresult = this.templateEngine.ProcessContent(token.Content, (Tokens)iter.Current.State);
                        if (!string.IsNullOrEmpty(tresult))
                        {
                            this.output.Append(tresult);
                        }
                    }

                    iter.MoveNext();
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
                case Tokens.OpenTag:
                case Tokens.CloseTag:
                case Tokens.Content:
                    return this.templateEngine.ProcessContent(token.Content);
                case Tokens.CommentStart:
                    return this.templateEngine.StartComment(token.Content);
                case Tokens.CommentEnd:
                    return this.templateEngine.EndComment(token.Content);
                case Tokens.CommandArg:
                    this.openCommandArg = true;
                    var parts = token.Content.Split('=');
                    this.templateEngine.AddCommandArg(parts[0], parts[1]);
                    break;
                case Tokens.CommandCloseArg:
                    this.openCommandArg = false;
                    return this.templateEngine.ProcessCommand(string.Empty);
                case Tokens.CommandCloseTag:
                    if (this.openCommandArg)
                    {
                        this.openCommandArg = false;
                        var result = this.templateEngine.ProcessCommand(string.Empty);
                        if (!string.IsNullOrEmpty(result))
                        {
                            this.output.Append(result);
                        }
                    }

                    return this.templateEngine.EndCommand();
                case Tokens.CommandName:
                    this.templateEngine.NewCommand(token.Content);
                    break;
                case Tokens.CommandOpenTag:
                    return token.Start == 0 ? this.templateEngine.ProcessOther(Tokens.CommandOpenTag, token.Content) : string.Empty;
                case Tokens.EOF:
                    this.templateEngine.EOF();
                    break;
                case Tokens.Property:
                    return this.templateEngine.ReplaceProperty(token.Content);
                case Tokens.Url:
                    return this.templateEngine.ProcessUrl(token.Content);
            }

            return string.Empty;
        }
    }
}
