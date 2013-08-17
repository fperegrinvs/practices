namespace MTO.Practices.Templating.Lexer
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Classe interna usada para representar um token
    /// </summary>
    [DataContract]
    public class PToken
    {
        /// <summary>
        /// Estado do token
        /// </summary>
        public int State { get; set; }

        /// <summary>
        /// Conteúdo do token
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// Linha em que o token foi encontrado
        /// </summary>
        public int Line { get; set; }

        /// <summary>
        /// Coluna em que o token foi encontrado
        /// </summary>
        public int Col { get; set; }

        /// <summary>
        /// Ponto de partida do token (estado)
        /// </summary>
        public int Start { get; set; }
    }
}
