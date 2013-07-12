namespace MTO.Practices.Common.Enumerators
{
    /// <summary>
    /// Enumerador para lógica de 3 estados
    /// </summary>
    public enum Tristate : byte
    {
        /// <summary>
        /// Estado Verdadeiro
        /// </summary>
        True = 0x01,

        /// <summary>
        /// Estado Falso
        /// </summary>
        False = 0x00,

        /// <summary>
        /// Estado inválido
        /// </summary>
        Invalid = 0xFF
    }
}
