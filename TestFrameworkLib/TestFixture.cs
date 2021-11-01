using System;
using System.Reflection;
using NUnit.Framework;
namespace TestFramework
{
    public class TestFixture
    {
        public void Test(string jobname, params (double start, double end, string property, Operator op, double value)[] expected)
        {
            var test_case = new TestCase(jobname, expected);
        }
    }
}