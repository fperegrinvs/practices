namespace MTO.Practices.Common.Templating.Models
{
    /// <summary>
    /// Interface que deve ser implementada pelos Models do StringTemplate
    /// </summary>
    public interface IStringTemplateModel
    {
        /// <summary>
        /// Escreve os atributos do model no template
        /// </summary>
        /// <param name="writer">O ModelWriter</param>
        void WriteModel(IModelWriter writer);
    }
}
