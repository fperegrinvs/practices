namespace MTO.Practices.Common.Minify
{
    /// <summary>
    /// Interface para compressor de CSS
    /// </summary>
    public interface ICssCompressor
    {
        /// <summary>
        /// Comprime conteúdo de arquivo CSS
        /// </summary>
        /// <param name="contents">conteúdo de um arquivo css</param>
        /// <returns>resultado da compressão do css</returns>
        string Compress(string contents);
    }
}
