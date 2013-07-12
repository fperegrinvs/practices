namespace MTO.Practices.Common.Tests
{
    using System;

    using MTO.Practices.Common.Extensions;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class PrettyExtencionTests
    {
        [TestMethod]
        public void DateTime_Com_Trinta_Segundos_TEST()
        {
            DateTime d = DateTime.Now.AddSeconds(-30);
            Assert.AreEqual("agora mesmo", d.ToPrettyString());
        }

        [TestMethod]
        public void DateTime_Com_CentoECinquenta_Segundos_TEST()
        {
            DateTime d = DateTime.Now.AddSeconds(-150);
            Assert.AreEqual("2 minutos atrás", d.ToPrettyString());
        }

        /// <summary>
        /// Timespan não necessariamente se relaciona com o agora.
        /// Pode ser um timespan de 30s ontem, entao tem que ser "30 segundos"
        /// </summary>
        [TestMethod]
        public void TimeSpan_Com_Trinta_Segundos_TEST()
        {
            TimeSpan t = new TimeSpan(0, 0, 30);
            Assert.AreEqual("30 segundos", t.ToPrettyString());
        }

        [TestMethod]
        public void TimeSpan_Com_CentoECinquenta_Segundos_TEST()
        {
            TimeSpan t = new TimeSpan(0, 0, 150);
            Assert.AreEqual("2 minutos", t.ToPrettyString());
        }
    }
}
