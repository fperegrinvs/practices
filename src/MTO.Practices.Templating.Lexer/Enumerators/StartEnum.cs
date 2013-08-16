namespace MTO.Templating.Lexer
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
        /// Definição de comando da upstore
        /// </summary>
        Command = 1,

        /// <summary>
        /// Parâmetros de um comando
        /// </summary>
        CommandArg = 2,

        /// <summary>
        /// Nome de um comando
        /// </summary>
        CommandName = 3,

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
