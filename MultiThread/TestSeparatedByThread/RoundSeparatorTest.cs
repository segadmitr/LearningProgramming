using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using Separators;

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
        public void RoundSeparatorTest1()
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
        public void RoundSeparatorTest2()
        {
            var separatedResult = _separator.Separate(9, 3).Select(s => s.ToList()).ToList();

            var expectedResut = new List<List<int>>();

            expectedResut.Add(new List<int> { 0, 3, 6 });
            expectedResut.Add(new List<int> { 1, 4, 7 });
            expectedResut.Add(new List<int> { 2, 5, 8 });

            RangeSeparatorTest.EqualsPart(separatedResult, expectedResut);
        }

        [TestMethod]
        public void RoundSeparatorTest3()
        {
            var separatedResult = _separator.Separate(6, 2).Select(s => s.ToList()).ToList();

            var expectedResut = new List<List<int>>();

            expectedResut.Add(new List<int> { 0, 2, 4 });
            expectedResut.Add(new List<int> { 1, 3, 5 });

            RangeSeparatorTest.EqualsPart(separatedResult, expectedResut);
        }

        [TestMethod]
        public void RoundSeparatorTest4()
        {
            var separatedResult = _separator.Separate(3, 1).Select(s => s.ToList()).ToList();

            var expectedResut = new List<List<int>>();

            expectedResut.Add(new List<int> { 0, 1, 2 });

            RangeSeparatorTest.EqualsPart(separatedResult, expectedResut);
        }

        [TestMethod]
        public void RoundSeparatorTest5()
        {
            var separatedResult = _separator.Separate(3, 2).Select(s => s.ToList()).ToList();

            var expectedResut = new List<List<int>>();

            expectedResut.Add(new List<int> { 0, 2 });
            expectedResut.Add(new List<int> { 1 });

            RangeSeparatorTest.EqualsPart(separatedResult, expectedResut);
        }

        [TestMethod]
        public void RoundSeparatorTest6()
        {
            var separatedResult = _separator.Separate(7, 4).Select(s => s.ToList()).ToList();

            var expectedResut = new List<List<int>>();

            expectedResut.Add(new List<int> { 0, 4 });
            expectedResut.Add(new List<int> { 1, 5 });
            expectedResut.Add(new List<int> { 2, 6 });
            expectedResut.Add(new List<int> { 3 });

            RangeSeparatorTest.EqualsPart(separatedResult, expectedResut);
        }

    }
}
