namespace MTO.Practices.Templating.Lexer
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    using MTO.Practices.Common.Serializers;

    /// <summary>
    /// Estrutura para armazenar lista de tokens resultandes do processo de lex
    /// </summary>
    public class TokenList : IEnumerable<PToken>
    {
        /// <summary>
        /// Lista de tokens armazenados
        /// </summary>
        private readonly List<PToken> tokens = new List<PToken>();

        /// <summary>
        /// Initializes a new instance of the <see cref="TokenList"/> class. 
        /// </summary>
        /// <param name="serializedData">
        /// The serialized Data.
        /// </param>
        public TokenList(string serializedData)
        {
            this.tokens = JsonSerializer.DeSerialize<List<PToken>>(serializedData);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TokenList"/> class. 
        /// </summary>
        /// <param name="data">
        /// The serialized Data.
        /// </param>
        public TokenList(List<PToken> data)
        {
            this.tokens = data;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TokenList"/> class.
        /// </summary>
        public TokenList()
        {
        }

        /// <summary>
        /// Quantidade de tokens
        /// </summary>
        public int Count
        {
            get
            {
                return this.tokens.Count;
            }
        }

        /// <summary>
        /// Adiciona um token à lista de token
        /// </summary>
        /// <param name="state">
        /// tipo de token
        /// </param>
        /// <param name="content">
        /// conteúdo do token
        /// </param>
        /// <param name="line">
        /// linha em que o token foi encontrado
        /// </param>
        /// <param name="col">
        /// coluna em que o token foi encontrado
        /// </param>
        /// <param name="start">
        /// The start.
        /// </param>
        public void Add(Tokens state, string content, int line, int col, StartEnum start)
        {
            if (this.tokens.Any() && state == Tokens.Content && this.tokens.Last().State == (int)state)
            {
                this.tokens.Last().Content += content;
            }
            else
            {
                this.tokens.Add(new PToken { Col = col, Content = content, Line = line, State = (int)state, Start = (int)start });
            }
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
        /// </returns>
        /// <filterpriority>1</filterpriority>
        public IEnumerator<PToken> GetEnumerator()
        {
            return this.tokens.GetEnumerator();
        }
            
        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
