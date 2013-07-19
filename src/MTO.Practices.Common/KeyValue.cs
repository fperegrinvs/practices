namespace MTO.Practices.Common
{
    /// <summary>
    /// Par chave/valor
    /// </summary>
    /// <typeparam name="T">
    /// Tipo da chave
    /// </typeparam>
    /// <typeparam name="TU">
    /// Tipo do valor
    /// </typeparam>
    public class KeyValue<T, TU>
    {
        /// <summary>
        /// Chave do par
        /// </summary>
        public T Key { get; set; }

        /// <summary>
        /// Valor do par
        /// </summary>
        public TU Value { get; set; }
    }
}
