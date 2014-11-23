using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using Task2;

namespace TestSeparatedByThread
{
    [TestClass]
    public class RoundSeparatorTest
    {
        ISeparator _separator;

        [TestInitialize]
        public void TestInitialize()
        {
            _separator = new RoundSeparator();
        }
        [TestMethod]
        public void TestMethod1()
        {
            var separatedResult = _separator.Separate(10, 10).Select(s => s.ToList()).ToList();

            var expectedResut = new List<List<int>>();
            for (var i = 0; i < 10; i++)
            {
                expectedResut.Add(new List<int> { i });
            }
            RangeSeparatorTest.EqualsPart(separatedResult, expectedResut);
        }

        [TestMethod]
        public void TestMethod2()
        {
            var separatedResult = _separator.Separate(9, 3).Select(s => s.ToList()).ToList();

            var expectedResut = new List<List<int>>();

            expectedResut.Add(new List<int> { 0, 3, 6 });
            expectedResut.Add(new List<int> { 1, 4, 7 });
            expectedResut.Add(new List<int> { 2, 5, 8 });

            RangeSeparatorTest.EqualsPart(separatedResult, expectedResut);
        }

        [TestMethod]
        public void TestMethod3()
        {
            var separatedResult = _separator.Separate(6, 2).Select(s => s.ToList()).ToList();

            var expectedResut = new List<List<int>>();

            expectedResut.Add(new List<int> { 0, 2, 4, 6 });
            expectedResut.Add(new List<int> { 1, 3, 5 });

            RangeSeparatorTest.EqualsPart(separatedResult, expectedResut);
        }
    }
}
