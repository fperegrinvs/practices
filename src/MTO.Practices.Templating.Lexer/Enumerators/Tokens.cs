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
        TagClosingArg = 9,

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
    }
}
