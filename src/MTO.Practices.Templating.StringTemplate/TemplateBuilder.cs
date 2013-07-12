namespace MTO.Practices.Common.Templating
{
    using System;
    using System.Collections.Generic;

    using Antlr3.ST;

    using MTO.Practices.Common.Templating.AttributeRenderer;
    using MTO.Practices.Common.Templating.Models;

    /// <summary>
    /// Renderiza StringTemplates com base no template e model fornecidos
    /// </summary>
    public class TemplateBuilder : IModelWriter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TemplateBuilder"/> class.
        /// </summary>
        /// <param name="template"> The template. </param>
        /// <param name="model"> The model. </param>
        public TemplateBuilder(string template, IStringTemplateModel model)
        {
            this.Template = new StringTemplate(template);
            this.Model = model;

            // this.Template.RegisterRenderer(typeof(string), new DateOpRenderer());
        }

        /// <summary>
        /// O template
        /// </summary>
        protected StringTemplate Template { get; set; }

        /// <summary>
        /// Model do template
        /// </summary>
        protected IStringTemplateModel Model { get; set; }

        /// <summary>
        /// Retorna o Html do e-mail
        /// </summary>
        /// <returns>O html</returns>
        public string GetHtml()
        {
            return this.GetHtml(null);
        }

        /// <summary>
        /// Retorna o Html do e-mail
        /// </summary>
        /// <param name="attributeRenderers">Renderizadores de atributos a serem usados pelo template</param>
        /// <returns>O html</returns>
        public string GetHtml(Dictionary<Type, IAttributeRenderer> attributeRenderers)
        {
            if (attributeRenderers != null)
            {
                this.Template.SetAttributeRenderers(attributeRenderers);
            }

            if (this.Model != null)
            {
                this.Model.WriteModel(this);
            }

            return this.Template.ToString();
        }

        /// <summary>
        /// Escreve o parâmetro do model
        /// </summary>
        /// <param name="name">Nome do parâmetro</param>
        /// <param name="value">Valor do parâmetro</param>
        public void WriteAttribute(string name, object value)
        {
            this.Template.SetAttribute(name, value);
        }

        /// <summary>
        /// Sobrescreve todos os atributos
        /// </summary>
        /// <param name="attributes">os atributos</param>
        public void SetAttributes(IDictionary<string, object> attributes)
        {
            this.Template.Attributes = attributes;
        }
    }
}
