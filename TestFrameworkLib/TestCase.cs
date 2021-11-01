using NUnit.Framework;
using System.Collections.Generic;
using System.IO;
using System;
namespace TestFramework
{
    public class TestCase
    {
        private List<SegmentCondition> m_Conditions;
        private ModFileData m_Data;

        public TestCase(string jobname, params (double start, double end, string property, Operator op, double value)[] expected)
        {
            m_Data = new ModFileData();
            m_Conditions = new List<SegmentCondition>();
            Assert.True(m_Data.ParseCsv(jobname));

            foreach(var exp in expected)
            {
                // Console.WriteLine("Start: {0}\nEnd: {1}\nProperty: {2}\nOperator:{3},\nValue to Compare: {4}", exp.start, exp.end, exp.property, exp.op, exp.value);

                List<DataRow> testData = m_Data.GetTestData(exp.start, exp.end, exp.property);
                m_Conditions.Add(new SegmentCondition(new TestSegment(exp.start, exp.end, testData), exp.property, exp.op, exp.value));
            }

            this.CheckAllSegmentConditions();
            this.Results();
        }

        public void CheckAllSegmentConditions()
        {
            foreach(var segmentCondition in m_Conditions)
            {
                try
                {
                // segmentCondition.print();
                segmentCondition.check();
                }
                catch(Exception e)
                {
                    // Console.Error.WriteLine("{0} -> Fail", segmentCondition.ToString());
                    segmentCondition.m_Passed = false;
                    continue;
                }
                // Console.WriteLine("{0} -> Pass", segmentCondition.ToString());
                segmentCondition.m_Passed = true;
            }
        }

        private void Results()
        {
            foreach(var segmentCondition in m_Conditions)
            {
                Console.WriteLine("{0} -> {1}", segmentCondition.ToString(), segmentCondition.m_Passed ? "Pass" : "Fail");
            } 
        } 

    }
}