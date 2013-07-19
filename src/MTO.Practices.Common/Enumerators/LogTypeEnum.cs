namespace MTO.Practices.Common.Enumerators
{
    /// <summary>
    /// Tipo de log
    /// </summary>
    public enum LogTypeEnum
    {
        /// <summary>
        /// Não especificado
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// The info.
        /// </summary>
        Info = 'I',

        /// <summary>
        /// The warning.
        /// </summary>
        Warning = 'W',

        /// <summary>
        /// The error.
        /// </summary>
        Error = 'E',

        /// <summary>
        /// Erro que não deve ser registrado no log.
        /// </summary>
        NoLog = 'N'
    }
}
