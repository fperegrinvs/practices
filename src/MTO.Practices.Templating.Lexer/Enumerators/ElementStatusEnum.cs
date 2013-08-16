namespace MTO.Practices.Templating.Lexer.Enumerators
{
    /// <summary>
    /// Estado de um elemento
    /// </summary>
    public enum ElementStatusEnum
    {
        /// <summary>
        /// Estado padrão de um elemento = processando
        /// </summary>
        Default = 0,

        /// <summary>
        /// Omite o conteúdo do elemento
        /// </summary>
        SkipContent = 1,

        /// <summary>
        /// Exibe o conteúdo do elemento sem processar
        /// </summary>
        DumpContent = 2
    }
}
