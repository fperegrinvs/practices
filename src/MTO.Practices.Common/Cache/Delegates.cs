namespace MTO.Practices.Cache
{
    /// <summary>
    /// Delegates usados para manipulação de cache
    /// </summary>
    public static class Delegates
    {
        /// <summary>
        /// Delegate padrão usado para refresh de cache
        /// </summary>
        /// <returns>string com o conteúdo a ser armazenado em cache</returns>
        public delegate T RefreshCacheDelegate<T>();
    }
}
