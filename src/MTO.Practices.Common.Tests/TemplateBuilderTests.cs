namespace MTO.Practices.Common.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Web.Script.Serialization;

    using Antlr3.ST;

    using MTO.Practices.Common.Templating;
    using MTO.Practices.Common.Templating.AttributeRenderer;
    using MTO.Practices.Common.Templating.Models;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Testes do TemplateBuilder
    /// </summary>
    [TestClass]
    public class TemplateBuilderTests
    {
        /// <summary>
        /// Testa um template simples sem model, usando escape no $
        /// </summary>
        [TestMethod]
        public void GetHtml_SimpleTemplate_NoModel_Escapes()
        {
            var builder = new TemplateBuilder("Teste \\$ oi", null);
            Assert.AreEqual("Teste $ oi", builder.GetHtml());
        }

        /// <summary>
        /// Testa um model simples com valores diretos
        /// </summary>
        [TestMethod]
        public void GetHtml_SimpleModel()
        {
            var model = new TestModel { String = "teste", Guid = Guid.NewGuid(), Time = null, Children = null };
            var builder = new TemplateBuilder("Teste $String$ oi $Guid$ $Time$ $Children$", model);

            var expected = string.Format("Teste {0} oi {1}  ", model.String, model.Guid);
            Assert.AreEqual(expected, builder.GetHtml());
        }

        /// <summary>
        /// Testa condicional em propriedade nullable, com valor não nulo
        /// </summary>
        [TestMethod]
        public void GetHtml_NullableProperty_NotNull()
        {
            var model = new TestModel { String = "teste", Guid = Guid.NewGuid(), Time = DateTime.Now, Children = null };
            var builder = new TemplateBuilder("Teste $String$ oi $Guid$ $if(Time)$$Time$$endif$ $Children$", model);

            var expected = string.Format("Teste {0} oi {1} {2} ", model.String, model.Guid, model.Time);
            Assert.AreEqual(expected, builder.GetHtml());
        }

        /// <summary>
        /// Testa condicional em propriedade nullable, com valor nulo
        /// </summary>
        [TestMethod]
        public void GetHtml_NullableProperty_Null()
        {
            var model = new TestModel { String = "teste", Guid = Guid.NewGuid(), Time = null, Children = null };
            var builder = new TemplateBuilder("Teste $String$ oi $Guid$ $if(Time)$$Time$$endif$ $Children$", model);

            var expected = string.Format("Teste {0} oi {1}  ", model.String, model.Guid, model.Time);
            Assert.AreEqual(expected, builder.GetHtml());
        }

        /// <summary>
        /// Testa iterar a lista de objetos filhos
        /// </summary>
        [TestMethod]
        public void GetHtml_IterateChildren()
        {
            var model = new TestModel
                {
                    String = "teste", 
                    Guid = Guid.NewGuid(), 
                    Time = DateTime.Now, 
                    Children = new List<TestModel>
                        {
                            new TestModel
                                {
                                    Children = null,
                                    String = "oioi",
                                    Guid = Guid.NewGuid(),
                                    Time = DateTime.Now
                                },
                            new TestModel
                            {
                                Children = null,
                                String = "eiei",
                                Guid = Guid.NewGuid(),
                                Time = DateTime.Now
                            }
                        }
                };
            var builder = new TemplateBuilder("Teste $String$ oi $Guid$ $Children:{a $it.String$ b}$", model);

            var expected = string.Format("Teste {0} oi {1} a oioi ba eiei b", model.String, model.Guid, model.Time);
            Assert.AreEqual(expected, builder.GetHtml());
        }

        /// <summary>
        /// Testa iterar a lista de objetos filhos
        /// </summary>
        [TestMethod]
        public void GetHtml_IterateChildren_NullChildren()
        {
            var model = new TestModel { String = "teste", Guid = Guid.NewGuid(), Time = DateTime.Now, Children = null };
            var builder = new TemplateBuilder("Teste $String$ oi $Guid$ $Children:{a $it.String$ b}$", model);

            var expected = string.Format("Teste {0} oi {1} ", model.String, model.Guid, model.Time);
            Assert.AreEqual(expected, builder.GetHtml());
        }

        /// <summary>
        /// Testa iterar a lista de objetos filhos
        /// </summary>
        [TestMethod]
        public void GetHtml_Conditional_EmptyList()
        {
            var model = new TestModel { String = "teste", Guid = Guid.NewGuid(), Time = DateTime.Now, Children = new List<TestModel>() };
            var builder = new TemplateBuilder("Teste $String$ oi $Guid$ $if(Children)$oi$endif$", model);

            var expected = string.Format("Teste {0} oi {1} ", model.String, model.Guid, model.Time);
            Assert.AreEqual(expected, builder.GetHtml());
        }

        /// <summary>
        /// Model de teste
        /// </summary>
        internal class TestModel : StringTemplateModelBase<TestModel>
        {
            /// <summary>
            /// Uma string
            /// </summary>
            public string String { get; set; }

            /// <summary>
            /// Um guid
            /// </summary>
            public Guid Guid { get; set; }

            /// <summary>
            /// Uma lista de objetos complexos
            /// </summary>
            public List<TestModel> Children { get; set; }

            /// <summary>
            /// Um objeto nullable
            /// </summary>
            public DateTime? Time { get; set; }
        }
    }
}
