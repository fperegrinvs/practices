namespace MTO.Practices.Common.Minify
{
    using Yahoo.Yui.Compressor;

    /// <summary>
    /// Implementação do compressor JS usando o YUI Compressor
    /// </summary>
    internal class YuiJavascriptCompressor : IJavaScriptCompressor
    {
        /// <summary>
        /// Comprime conteúdo de arquivo JavaScript
        /// </summary>
        /// <param name="contents">conteúdo de um arquivo js</param>
        /// <returns>resultado da compressão do js</returns>
        public string Compress(string contents)
        {
            return JavaScriptCompressor.Compress(contents);
        }
    }
}
