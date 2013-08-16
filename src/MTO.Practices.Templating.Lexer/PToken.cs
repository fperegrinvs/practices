namespace MTO.Templating.Lexer
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
        [DataMember(Name = "STT")]
        public int State { get; set; }

        /// <summary>
        /// Conteúdo do token
        /// </summary>
        [DataMember(Name = "CON")]
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
        [DataMember(Name = "STR")]
        public int Start { get; set; }
    }
}
