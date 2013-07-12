using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MTO.Practices.Common.Tests
{
    [TestClass]
    public class GeoLocationTests
    {
        [TestMethod]
        public void TestGeolocation()
        {
            //var ip = "201.68.177.190";
            //var local = GeoLocation.GeoLocation.ReturnLocationString(ip);

            //Assert.AreEqual(local[0], "São Paulo");
            //Assert.AreEqual(local[1], "27");
        }

        [TestMethod]
        public void TestGeolocationNegative()
        {
            //var ip = "201.68.177.19A";
            //var local = GeoLocation.GeoLocation.ReturnLocationString(ip);

            //Assert.IsNull(local);
        }
    }
}
