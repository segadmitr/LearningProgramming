using System;
using System.Collections;
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
        public void RangeSeparatorTest1()
        {
            var separatedResult = _separator.Separate(10, 10).Select(s => s.ToList()).ToList();
                                   
            var expectedResut = new List<List<int>>();
            for (var i = 0; i < 10; i++)
            {
                expectedResut.Add(new List<int> { i });
            }
            EqualsPart(separatedResult, expectedResut);
        }

        [TestMethod]
        public void RangeSeparatorTest2()
        {
            var separatedResult = _separator.Separate(10, 3).Select(s => s.ToList()).ToList();

            var expectedResut = new List<List<int>>();
            expectedResut.Add(new List<int> {0, 1, 2});
            expectedResut.Add(new List<int> {3, 4, 5});
            expectedResut.Add(new List<int> {6, 7, 8});
            expectedResut.Add(new List<int> {9});
            EqualsPart(separatedResult, expectedResut);
        }

        public static void EqualsPart(List<List<int>> separatedResult, List<List<int>> expectedResut)
        {
            Assert.AreEqual(separatedResult.Count(), expectedResut.Count(),"Количество частей не совпадает");
            for (var i = 0; i < separatedResult.Count(); i++)
            {
                CollectionAssert.AreEqual(separatedResult[i], expectedResut[i]);
            }
        }
    }
}
