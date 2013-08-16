namespace MTO.Templating.Lexer
{
    using System.Collections.Generic;

    using MTO.Templating.Lexer.Enumerators;

    /// <summary>
    /// Comando incluso no template
    /// </summary>
    public class Command
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Command"/> class. 
        /// Cria novo comando
        /// </summary>
        /// <param name="name">
        /// Nome do comando
        /// </param>
        public Command(string name)
        {
            this.Name = name;
            this.Arguments = new Dictionary<string, string>();
        }

        /// <summary>
        /// Nome do comando
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Estado atual do commando
        /// </summary>
        public CommandStatusEnum CommandStatus { get; set; }

        /// <summary>
        /// Parametros usados no comando
        /// </summary>
        public Dictionary<string, string> Arguments { get; set; }
    }
}
