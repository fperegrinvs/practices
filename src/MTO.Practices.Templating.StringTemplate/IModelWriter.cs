namespace MTO.Practices.Common.Templating
{
    using System.Collections.Generic;

    /// <summary>
    /// Contrato da classe que escreve os atributos do model no template
    /// </summary>
    public interface IModelWriter
    {
        /// <summary>
        /// Escreve o parâmetro do model
        /// </summary>
        /// <param name="name">Nome do parâmetro</param>
        /// <param name="value">Valor do parâmetro</param>
        void WriteAttribute(string name, object value);

        /// <summary>
        /// Sobrescreve todos os atributos
        /// </summary>
        /// <param name="attributes">os atributos</param>
        void SetAttributes(IDictionary<string, object> attributes);
    }
}
