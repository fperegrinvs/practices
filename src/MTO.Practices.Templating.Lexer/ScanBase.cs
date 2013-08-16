namespace MTO.Templating.Lexer
{
    /// <summary>
    /// Classe base para os scanners de template
    /// </summary>
    public abstract class ScanBase
    {
        /// <summary>
        /// Lista de tokens mapeados pelo scanner
        /// </summary>
        public TokenList TokenOutput = new TokenList();

        /// <summary>
        /// Posição do scanner
        /// </summary>
        public abstract int CurrentPosition { get;  }

        /// <summary>
        /// Coluna do scanner
        /// </summary>
        public abstract int CurrentColumn { get;  }

        /// <summary>
        /// Coluna atual do scanner
        /// </summary>
        public abstract int CurrentLine { get;  }

        /// <summary>
        /// Estado atual do scanner
        /// </summary>
        public abstract StartEnum CurrentStart { get; }

        /// <summary>
        /// Busca próximo token
        /// </summary>
        /// <returns>tipo de token encontrado</returns>
        public abstract int yylex();

        protected virtual bool yywrap() { return true; }
    }
}
