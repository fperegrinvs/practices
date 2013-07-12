namespace MTO.Practices.Common.Templating.Models
{
    using System.Linq;

    /// <summary>
    /// Classe base dos models do StringTemplate
    /// </summary>
    /// <typeparam name="T">O tipo do model</typeparam>
    public class StringTemplateModelBase<T> : IStringTemplateModel where T : new()
    {
        /// <summary>
        /// Escreve os atributos do model no template
        /// </summary>
        /// <param name="writer">O ModelWriter</param>
        public void WriteModel(IModelWriter writer)
        {
            var props = typeof(T).GetProperties();
            foreach (var t in props.Where(t => t.CanRead))
            {
                writer.WriteAttribute(t.Name, t.GetValue(this, null));
            }
        }
    }
}
