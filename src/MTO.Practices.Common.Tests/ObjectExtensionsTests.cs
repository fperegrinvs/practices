namespace MTO.Practices.Common.Tests
{
    using System;

    using MTO.Practices.Common.Extensions;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Testes do TemplateBuilder
    /// </summary>
    [TestClass]
    public class ObjectExtensionsTests
    {
        /// <summary>
        /// Mapeia dois objetos do mesmo tipo
        /// </summary>
        [TestMethod]
        public void MapFrom_DoisMesmoTipo()
        {
            var origin = new TestClass { Child = null, Id = Guid.NewGuid(), Number = 1, String = "oioi" };
            var destination = new TestClass();

            destination.MapFrom(origin);

            Assert.AreEqual(origin.Id, destination.Id);
            Assert.AreEqual(origin.String, destination.String);
            Assert.AreEqual(origin.Number, destination.Number);
        }

        /// <summary>
        /// Mapeia dois objetos de tipo diferente
        /// </summary>
        [TestMethod]
        public void MapFrom_DoisTiposDiferentes()
        {
            var origin = new TestClass { Child = null, Id = Guid.NewGuid(), Number = 1, String = "oioi" };
            var destination = new TestOtherClass();

            destination.MapFrom(origin);

            Assert.AreEqual(origin.Id, destination.Id);
            Assert.AreEqual(origin.String, destination.String);
            Assert.AreNotEqual(origin.Number, destination.Number);
        }

        /// <summary>
        /// Mapeia dois objetos de tipo diferente
        /// </summary>
        [TestMethod]
        public void MapFrom_DoisTiposDiferentes_Child()
        {
            var origin = new TestClass
                {
                    Child = new TestClass
                        {
                            Id = Guid.NewGuid(),
                            Number = 1,
                            String = "epa"
                        }, 
                    Id = Guid.NewGuid(), 
                    Number = 1, String = "oioi"
                };
            var destination = new TestOtherClass();

            destination.MapFrom(origin);

            Assert.AreEqual(origin.Id, destination.Id);
            Assert.AreEqual(origin.String, destination.String);
            Assert.AreNotEqual(origin.Number, destination.Number);
            Assert.AreEqual(origin.Child.Id, destination.Child.Id);
            Assert.AreEqual(origin.Child.String, destination.Child.String);
            Assert.AreEqual(origin.Child.Number, destination.Child.Number);
        }

        /// <summary>
        /// Mapeia dois objetos de tipo diferente
        /// </summary>
        [TestMethod]
        public void MapFrom_DoisTiposDiferentes_ChildToParentProperty()
        {
            var origin = new TestClass
            {
                Child = new TestClass
                {
                    Id = Guid.NewGuid(),
                    Number = 1,
                    String = "epa"
                },
                Id = Guid.NewGuid(),
                Number = 1,
                String = "oioi"
            };
            var destination = new TestOtherClass2();

            destination.MapFrom(origin);

            Assert.AreEqual(origin.Id, destination.Id);
            Assert.AreEqual(origin.String, destination.String);
            Assert.AreNotEqual(origin.Number, destination.Number);
            Assert.AreEqual(origin.Child.Id, destination.ChildId);
            Assert.AreEqual(origin.Child.String, destination.ChildString);
        }

        protected class TestClass
        {
            public string String { get; set; }

            public Guid Id { get; set; }

            public TestClass Child { get; set; }

            public float Number { get; set; }
        }

        protected class TestOtherClass
        {
            public string String { get; set; }

            public Guid Id { get; set; }

            public TestClass Child { get; set; }

            public int Number { get; set; }
        }

        protected class TestOtherClass2
        {
            public string String { get; set; }

            public Guid Id { get; set; }

            public Guid ChildId { get; set; }

            public string ChildString { get; set; }

            public int Number { get; set; }
        }
    }
}
