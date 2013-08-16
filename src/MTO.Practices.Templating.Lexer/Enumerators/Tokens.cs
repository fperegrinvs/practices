namespace MTO.Templating.Lexer
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
        /// Abertura de tag de comando
        /// </summary>
        CommandOpenTag = 3,

        /// <summary>
        /// Fechamento de tag de comando
        /// </summary>
        CommandCloseTag = 4,

        /// <summary>
        /// Abertura de tag qualquer
        /// </summary>
        OpenTag = 5,

        /// <summary>
        /// Fechamento de tag genérica
        /// </summary>
        CloseTag = 6,

        /// <summary>
        /// Conteúdo qualquer
        /// </summary>
        Content = 7,

        /// <summary>
        /// Nome do comando a ser executado
        /// </summary>
        CommandName = 8,

        /// <summary>
        /// Fim da linha da especificação de parametros de um comando
        /// </summary>
        CommandCloseArg = 9,

        /// <summary>
        /// Parâmetro de um comando
        /// </summary>
        CommandArg = 10,

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
