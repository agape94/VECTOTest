using System;
using NUnit.Framework;
namespace TestFramework
{
    public class TestCase
    {
        public TestCase()
        {
            Console.WriteLine("Test Case!");
        }

        public void RunTestCase()
        {
            Console.WriteLine("Running test case");
            Assert.True(true);
        }
    }
}