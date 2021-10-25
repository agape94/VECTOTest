using System;
using NUnit.Framework;
using System.Collections.Generic;
namespace TestFramework
{
    public class TestCase
    {
        List<TestSegment> m_TestSegments;
        public void RunTestCase()
        {
            Console.WriteLine("Running test case");

            var ts = new TestSegment(.5, 20, "v_act", Operator.Greater);

            Assert.True(true);
        }
    }
}