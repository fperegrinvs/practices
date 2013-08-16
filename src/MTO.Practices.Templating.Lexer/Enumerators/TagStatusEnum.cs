namespace MTO.Practices.Templating.Lexer.Enumerators
{
    /// <summary>
    /// Estado de uma tag
    /// </summary>
    public enum TagStatusEnum
    {
        /// <summary>
        /// Estado padrão de uma tag
        /// </summary>
        Default = 0,

        /// <summary>
        /// Omite o conteúdo da tag
        /// </summary>
        SkipContent = 1,

        /// <summary>
        /// Não pocessa o conteúdo da tag
        /// </summary>
        DumpContent = 2
    }
}
