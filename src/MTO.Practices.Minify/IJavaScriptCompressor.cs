namespace MTO.Practices.Common.Minify
{
    /// <summary>
    /// Interface para compressor de JavaScript
    /// </summary>
    public interface IJavaScriptCompressor
    {
        /// <summary>
        /// Comprime conteúdo de arquivo JavaScript
        /// </summary>
        /// <param name="contents">conteúdo de um arquivo js</param>
        /// <returns>resultado da compressão do js</returns>
        string Compress(string contents);
    }
}
