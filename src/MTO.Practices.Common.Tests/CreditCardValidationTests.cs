namespace MTO.Practices.Common.Tests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using MTO.Practices.Common.Enumerators;
    using MTO.Practices.Common.Helper;

    [TestClass]
    public class CreditCardValidationTests
    {
        [TestMethod]
        public void ValidaVisa16()
        {
            var numbers = new[] { "4111111111111111", "4012888888881881", "4073020000000002 " };

            foreach (var number in numbers)
            {
                CreditCardTypeEnum type;
                var result = CreditCardValidation.ValidateGenercCreditCard(number, out type);
                Assert.AreEqual(CreditCardTypeEnum.Visa, type);
                Assert.IsTrue(result);
            }
        }

        [TestMethod]
        public void ValidaMaster()
        {
            var numbers = new[] { "5105105105105100", "5555555555554444", "5555666677778884  " };

            foreach (var number in numbers)
            {
                CreditCardTypeEnum type;
                var result = CreditCardValidation.ValidateGenercCreditCard(number, out type);
                Assert.AreEqual(CreditCardTypeEnum.Master, type);
                Assert.IsTrue(result);
            }
        }

        [TestMethod]
        public void ValidaVisa13()
        {
            var numbers = new[] { "4222222222222" };

            foreach (var number in numbers)
            {
                CreditCardTypeEnum type;
                var result = CreditCardValidation.ValidateGenercCreditCard(number, out type);
                Assert.AreEqual(CreditCardTypeEnum.Visa, type);
                Assert.IsTrue(result);
            }
        }

        [TestMethod]
        public void ValidaDiners()
        {
            var numbers = new[] { "30111122223331", "38520000023237", "30569309025904" };

            foreach (var number in numbers)
            {
                CreditCardTypeEnum type;
                var result = CreditCardValidation.ValidateGenercCreditCard(number, out type);
                Assert.AreEqual(CreditCardTypeEnum.Diners, type);
                Assert.IsTrue(result);
            }
        }

        [TestMethod]
        public void ValidaDiscover()
        {
            var numbers = new[] { "6011111111111117", "6011000990139424" };

            foreach (var number in numbers)
            {
                CreditCardTypeEnum type;
                var result = CreditCardValidation.ValidateGenercCreditCard(number, out type);
                Assert.AreEqual(CreditCardTypeEnum.Discover, type);
                Assert.IsTrue(result);
            }
        }

        [TestMethod]
        public void ValidaAmex()
        {
            var numbers = new[] { "376411112222331", "378282246310005", "371449635398431", "378734493671000" };

            foreach (var number in numbers)
            {
                CreditCardTypeEnum type;
                var result = CreditCardValidation.ValidateGenercCreditCard(number, out type);
                Assert.AreEqual(CreditCardTypeEnum.Amex, type);
                Assert.IsTrue(result);
            }
        }
    }
}
