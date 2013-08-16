namespace MTO.Practices.Templating.Lexer.Interfaces
{
    /// <summary>
    /// Contrato para processamento de templates
    /// </summary>
    public interface ITemplateEngine
    {
        /// <summary>
        /// Referência para o parser
        /// </summary>
        Parser Parser { get; set; }

        /// <summary>
        /// Estado atual do motor
        /// </summary>
        StartEnum CurrentState { get; set; }

        /// <summary>
        /// tag atual
        /// </summary>
        ITemplateElement CurrentElement { get; }

        /// <summary>
        /// Inicio de comentário
        /// </summary>
        /// <param name="comment">
        /// tag de abertura do comentário
        /// </param>
        /// <returns>
        /// The start comment.
        /// </returns>
        string StartComment(string comment);

        /// <summary>
        /// Fim de comentário
        /// </summary>
        /// <param name="comment">
        /// tag que finaliza o comentário
        /// </param>
        /// <returns>
        /// The end comment.
        /// </returns>
        string EndComment(string comment);

        /// <summary>
        /// Adiciona nova tag à pilha de tags
        /// </summary>
        /// <param name="tagName">nome da tag</param>
        void NewTag(string tagName);

        /// <summary>
        /// Adiciona novo parâmetro à tag atual
        /// </summary>
        /// <param name="argument">nome do parâmetro</param>
        /// <param name="value">valor do parâmetro</param>
        void AddTagArg(string argument, string value);

        /// <summary>
        /// Finaliza tag atual
        /// </summary>
        /// <returns>
        /// The System.String.
        /// </returns>
        string EndTag();

        /// <summary>
        /// Substitui propriedade (variável) pelo valor apropriado
        /// </summary>
        /// <param name="property">propriedade a ser substituida</param>
        /// <returns>retorno da substituição da propriedade</returns>
        string ReplaceProperty(string property);

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
        string ProcessContent(string content, Tokens? token = null);

        /// <summary>
        /// Processa tag e retorna o seu resultado
        /// </summary>
        /// <param name="content">conteúdo relacionado à tag</param>
        /// <returns>conteúdo processado</returns>
        string ProcessTag(string content);

        /// <summary>
        /// Processa outro tipo de token
        /// </summary>
        /// <param name="tokenType">tipo do token</param>
        /// <param name="tokenValue">valor associado ao token</param>
        /// <returns>resultado do processamento do token</returns>
        string ProcessOther(Tokens tokenType, string tokenValue);

        /// <summary>
        /// Processa uma url
        /// </summary>
        /// <param name="content">endereço da url (incluindo <![CDATA[<a href="]]]>)</param>
        /// <returns>resultado do processamento</returns>
        string ProcessUrl(string content);

        /// <summary>
        /// Evento disparado quando o fim do template é atingido
        /// </summary>
        void EOF();

        /// <summary>
        /// Inicia novo comando na pilha de comandos
        /// </summary>
        /// <param name="name">Nome do novo comando</param>
        void NewCommand(string name);
        
        /// <summary>
        /// Inicia novo argumento
        /// </summary>
        void NewCommandArg();

        /// <summary>
        /// Inicia novo argumento de comando
        /// </summary>
        void NewCommandArgValue();

        /// <summary>
        /// Processa argumento ou conteúdo do comando
        /// </summary>
        void ProcessCommandContent();

        /// <summary>
        /// Processa o comando
        /// </summary>
        /// <returns> O resultado do processamento do comando </returns>
        string ProcessCommand();
    }
}
