namespace MTO.Practices.Common.Templating.Models
{
    using System.Collections.Generic;

    /// <summary>
    /// Model do StringTemplate construído a partir de um dicionario
    /// </summary>
    public class StringTemplateDictionaryModel : IStringTemplateModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StringTemplateDictionaryModel"/> class.
        /// </summary>
        /// <param name="model"> The model. </param>
        public StringTemplateDictionaryModel(IDictionary<string, object> model)
        {
            this.Model = model;
        }

        /// <summary>
        /// O model
        /// </summary>
        protected IDictionary<string, object> Model { get; set; }

        /// <summary>
        /// Escreve os atributos do model no template
        /// </summary>
        /// <param name="writer">O ModelWriter</param>
        public void WriteModel(IModelWriter writer)
        {
            writer.SetAttributes(this.Model);
        }
    }
}
