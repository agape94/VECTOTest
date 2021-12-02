using NUnit.Framework;
using System.Collections.Generic;
using System.IO;
using System;
namespace TestFramework
{
    public class VECTOTestCase
    {
        private List<SegmentCondition> m_Conditions;
        private ModFileData m_Data;

        public VECTOTestCase(string jobname, params (double start, double end, string property, Operator op, double value)[] expected)
        {
            m_Data = new ModFileData();
            m_Conditions = new List<SegmentCondition>();
            Assert.True(m_Data.ParseCsv(jobname));

            foreach (var exp in expected) {
                try {
                    
                    var testData = m_Data.GetTestData(exp.start, exp.end, exp.property);
                    var testSegment = new TestSegment(exp.start, exp.end, testData);
                    var segmentCondition = Utils.SegmentConditionFactoryMethod(exp.op, testSegment, exp.property, exp.value);
                    m_Conditions.Add(segmentCondition);

                } catch (Exception e) {
                    Console.Error.WriteLine(e.ToString());
                }
            }

            CheckAllSegmentConditions();
            PrintResults();
        }

        public void CheckAllSegmentConditions()
        {
            foreach (var segmentCondition in m_Conditions) {
                segmentCondition.check();
            }
        }

        private void PrintResults()
        {
            foreach (var segmentCondition in m_Conditions) {
                Console.WriteLine("{0} -> {1}", segmentCondition, segmentCondition.Passed ? "✔ Pass" : "✗ Fail");
                if(!segmentCondition.Passed || segmentCondition.Analyze)
                {
                    segmentCondition.PrintTrueConditions();
                }
                // string message = string.Format("{0} -> {1}", segmentCondition.ToString(), segmentCondition.Passed ? "[✔ Pass]" : "[✗ Fail]");
                // Utils.WriteColor(message, ConsoleColor.Green);
            }
        }

    }
}
