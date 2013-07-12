namespace MTO.Practices.Common.Minify
{
    using Yahoo.Yui.Compressor;

    /// <summary>
    /// Implementação do compressor de CSS usando o YUI Compressor.
    /// </summary>
    internal class YuiCssCompressor : ICssCompressor
    {
        /// <summary>
        /// Comprime conteúdo de arquivo CSS
        /// </summary>
        /// <param name="contents">conteúdo de um arquivo css</param>
        /// <returns>resultado da compressão do css</returns>
        public string Compress(string contents)
        {
            return CssCompressor.Compress(contents);
        }
    }
}
