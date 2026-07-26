using NUnit.Framework;
using System;
using MathLibraryApp;

namespace MathLibraryTests
{
    public class Tests
    {
        private MathLibrary math;

        [SetUp]
        public void Setup()
        {
            math = new MathLibrary();
        }

        [TestCase(10, 5, 5)]
        [TestCase(20, 10, 10)]
        [TestCase(15, 3, 12)]
        public void TestSubtraction(double a, double b, double expected)
        {
            Assert.AreEqual(expected, math.Subtract(a, b));
        }

        [TestCase(2, 3, 6)]
        [TestCase(5, 5, 25)]
        public void TestMultiplication(double a, double b, double expected)
        {
            Assert.AreEqual(expected, math.Multiply(a, b));
        }

        [TestCase(10, 2, 5)]
        [TestCase(20, 4, 5)]
        public void TestDivision(double a, double b, double expected)
        {
            Assert.AreEqual(expected, math.Divide(a, b));
        }

        [Test]
        public void TestDivisionByZero()
        {
            try
            {
                math.Divide(10, 0);
                Assert.Fail("Division by zero");
            }
            catch (ArgumentException ex)
            {
                Assert.AreEqual("Division by zero", ex.Message);
            }
        }

        [Test]
        public void TestAddAndClear()
        {
            math.Add(10, 20);
            Assert.AreEqual(30, math.GetResult);

            math.AllClear();
            Assert.AreEqual(0, math.GetResult);
        }
    }
}