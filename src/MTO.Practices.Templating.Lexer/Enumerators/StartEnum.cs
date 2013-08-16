namespace MTO.Practices.Templating.Lexer
{
    /// <summary>
    /// Enumerador de estados iniciais
    /// </summary>
    public enum StartEnum
    {
        /// <summary>
        /// Estado padrão
        /// </summary>
        Initial = 0,

        /// <summary>
        /// Definição de tag da engine de templates
        /// </summary>
        Tag = 1,

        /// <summary>
        /// Parâmetros de uma tag
        /// </summary>
        TagArg = 2,

        /// <summary>
        /// Nome de uma tag
        /// </summary>
        TagName = 3,

        /// <summary>
        /// Região de comentário
        /// </summary>
        Comment = 4,

        /// <summary>
        /// Processando uma url
        /// </summary>
        Url = 5,
    }
}
