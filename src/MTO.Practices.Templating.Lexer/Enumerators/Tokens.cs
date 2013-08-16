namespace MTO.Practices.Templating.Lexer
{
    /// <summary>
    /// Tipos de token definidos
    /// </summary>
    public enum Tokens
    {
        /// <summary>
        /// Fim de arquivo
        /// </summary>
        EOF = 0,

        /// <summary>
        /// Propriedade de um elemento que deve ser substituido pelo valor de uma propriedade de objeto
        /// </summary>
        Property = 1,

        /// <summary>
        /// Quebra de linha
        /// </summary>
        NewLine = 2,

        /// <summary>
        /// Abertura de tag da engine de template
        /// </summary>
        OpenMtoTag = 3,

        /// <summary>
        /// Fechamento de tag da engine de template
        /// </summary>
        CloseMtoTag = 4,

        /// <summary>
        /// Abertura de tag qualquer
        /// </summary>
        OpenHtmlTag = 5,

        /// <summary>
        /// Fechamento de tag genérica
        /// </summary>
        CloseHtmlTag = 6,

        /// <summary>
        /// Conteúdo qualquer
        /// </summary>
        Content = 7,

        /// <summary>
        /// Nome da tag a ser executado
        /// </summary>
        TagName = 8,

        /// <summary>
        /// Fim da linha da especificação de parametros de uma tag
        /// </summary>
        TagCloseArg = 9,

        /// <summary>
        /// Parâmetro de uma tag
        /// </summary>
        TagArg = 10,

        /// <summary>
        /// Inicio de um comentário
        /// </summary>
        CommentStart = 11,

        /// <summary>
        /// Fim de um comentário
        /// </summary>
        CommentEnd = 12,

        /// <summary>
        /// Identifica a url de um link
        /// </summary>
        Url = 13,

        /// <summary>
        /// Inicio de um comando
        /// </summary>
        OpenCommand = 14,
        
        /// <summary>
        /// Finaliza (processa) um comando
        /// </summary>
        CloseCommand = 15,

        /// <summary>
        /// Inicia um comando
        /// </summary>
        OpenCommandArg = 16,

        /// <summary>
        /// Conteúdo de um comando
        /// </summary>
        OpenCommandContent = 17,

        /// <summary>
        /// Fim do conteúdo de um comando
        /// </summary>
        CloseCommandContent = 18,

        /// <summary>
        /// Inicio dos argumentos de um comando
        /// </summary>
        OpenComandArgValue = 18,
    }
}
