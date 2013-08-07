namespace MTO.Practices.Common.Tests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using MTO.Practices.Common.Helper;

    [TestClass]
    public class ValidationTests
    {
        [TestMethod]
        public void ValidaCPF()
        {
            var numbers = new[] { "34873777305", "983613672-09", "683.130.335-84 " };

            foreach (var number in numbers)
            {
                var result = Validation.ValidaCpf(number);
                Assert.IsTrue(result);
            }
        }

        [TestMethod]
        public void ValidaCNPJ()
        {
            var numbers = new[] { "48534259000149", "45.514.165/0001-10", "59.878.072/0001-89  " };

            foreach (var number in numbers)
            {
                var result = Validation.ValidaCNPJ(number);
                Assert.IsTrue(result);
            }
        }
    }
}
