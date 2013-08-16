namespace MTO.Templating.Lexer
{
    /// <summary>
    /// Métodos para facilitar a manipulação do scanner
    /// </summary>
    public static class ScannerExtensions
    {
        /// <summary>
        /// Adiciona um token a lista de tokens do scanner
        /// </summary>
        /// <param name="scan">
        /// The scan.
        /// </param>
        /// <param name="token">
        /// The token.
        /// </param>
        /// <param name="content">
        /// The text.
        /// </param>
        public static void AddToken(this ScanBase scan, Tokens token, string content)
        {
            if (scan.TokenOutput == null)
            {
                scan.TokenOutput = new TokenList();
            }

            scan.TokenOutput.Add(token, content, scan.CurrentLine, scan.CurrentColumn, scan.CurrentStart);
        }
    }
}
