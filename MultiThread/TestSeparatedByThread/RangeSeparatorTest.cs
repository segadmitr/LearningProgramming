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
            var separatedResult = _separator.Separate(10, 10);
            var expectedResut = new List<List<int>>();
            for (var i = 0; i < 10; i++)
            {
                expectedResut.Add(new List<int>() { i });
            }
            int indexCounter = 0;
            foreach (var sepResItem in separatedResult)    
            {
                int innerIndexCounter = 0;
                var expectedItem = expectedResut[indexCounter];
                foreach (var item in sepResItem)
                {
                    Assert.AreEqual(item,expectedItem[innerIndexCounter]);
                    innerIndexCounter++;
                }
                indexCounter++;
            }
        }
    }
}
