namespace MTO.Practices.Common.Minify
{
    using MTO.Practices.Common;

    /// <summary>
    /// Compressor de conteúdo de arquivos
    /// </summary>
    public class Compressor
    {
        /// <summary>
        /// Comprime conteúdo de arquivo JavaScript
        /// </summary>
        /// <param name="script">conteúdo de um arquivo js</param>
        /// <returns>resultado da compressão do js</returns>
        public static string CompressJavascript(string script)
        {
            return Injector.ResolveInterface<IJavaScriptCompressor>().Compress(script);
        }

        /// <summary>
        /// Comprime conteúdo de arquivo CSS
        /// </summary>
        /// <param name="css">conteúdo de um arquivo css</param>
        /// <returns>resultado da compressão do css</returns>
        public static string CompressCss(string css)
        {
            return Injector.ResolveInterface<ICssCompressor>().Compress(css);
        }
    }
}
