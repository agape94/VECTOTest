using NUnit.Framework;
using System.Collections.Generic;
using System.IO;
using System;
namespace TestFramework
{
    public class TestCase
    {
        List<SegmentCondition> m_Conditions;
        ModFileData m_Data;
        public TestCase(string jobname, params (double start, double end, string property, Operator op, double value)[] expected)
        {
            // Console.WriteLine("Running test case");

            // TODO extract information from expected and create TestSegments with SegmentConditions

            m_Data = new ModFileData();
            Assert.True(m_Data.ParseCsv(jobname));
        }

        public bool check()
        {
            // TODO go through all conditions and apply them
            return true;
        }

    }
}