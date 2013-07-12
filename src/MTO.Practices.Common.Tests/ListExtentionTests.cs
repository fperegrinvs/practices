namespace MTO.Practices.Common.Tests
{
    using System;
    using System.Collections.Generic;

    using MTO.Practices.Common.Extensions;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class ListExtentionTests
    {
        [TestMethod]
        public void Partition_0_particoes_TEST()
        {
            var list = new List<int> { 0, 1, 2, 3, 4, 5 };

            try
            {
                list.Partition(0);
            }
            catch (Exception ex)
            {
                Assert.IsInstanceOfType(ex, typeof(ArgumentOutOfRangeException));
                return;
            }
            
            Assert.Fail();
        }

        [TestMethod]
        public void Partition_1_particao_TEST()
        {
            var list = new List<int> { 0, 1, 2, 3, 4, 5 };

            var parts = list.Partition(1);
            Assert.AreEqual(1, parts.Length);
            Assert.AreEqual(list.Count, parts[0].Count);
        }
    }
}
