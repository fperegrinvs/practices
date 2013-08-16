namespace MTO.Templating.Lexer.Enumerators
{
    /// <summary>
    /// Estado de um comando
    /// </summary>
    public enum CommandStatusEnum
    {
        /// <summary>
        /// Estado padrão de um comando
        /// </summary>
        Default = 0,

        /// <summary>
        /// Pular o conteúdo do comando
        /// </summary>
        SkipContent = 1,

        /// <summary>
        /// Pula conteúdo do comando
        /// </summary>
        DumpContent = 2
    }
}
