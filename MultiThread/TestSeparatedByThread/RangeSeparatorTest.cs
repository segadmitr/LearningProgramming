using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using Task2;

namespace TestSeparatedByThread
{
    [TestClass]
    public class RangeSeparatorTest
    {
        ISeparator _separator;
        
        [TestInitialize]
        public void TestInitialize()
        {
            _separator = new RangeSeparator();
        }

        [TestMethod]
        public void TestMethod1()
        {
            _separator.Separate(10, 10);
            var separatedResult =  _separator.ToList();
            var expectedResut = new List<IEnumerable<int>>();
            for (var i = 0; i < 10; i++)
            {
                expectedResut.Add(new List<int>() {i});
            }

            CollectionAssert.AreEqual(expectedResut, separatedResult);
        }
    }
}
