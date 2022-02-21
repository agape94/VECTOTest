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
        private bool m_Passed;

        public VECTOTestCase(string jobname, params SegmentCondition[] segmentConditions)
        {
            m_Data = new ModFileData();
            m_Conditions = new List<SegmentCondition>();
            m_Passed = true;

            Assert.True(m_Data.ParseCsv(jobname));

            foreach (var sc in segmentConditions) {
                try {
                    sc.Data = m_Data.GetTestData(sc.Start, sc.End, sc.Property, sc.Type);
                    m_Conditions.Add(sc);
                } catch (Exception e) {
                    Console.Error.WriteLine(e.ToString());
                }
            }

            CheckOrAnalyzeSegmentConditions();
            PrintResults();
        }

        public void CheckOrAnalyzeSegmentConditions()
        {
            foreach (var segmentCondition in m_Conditions) {
                if(!segmentCondition.ToAnalyze)
                {
                    segmentCondition.Check();
                }
                else
                {
                    segmentCondition.Analyze();
                }

                segmentCondition.PrintResults();

                if(!segmentCondition.Passed && m_Passed)
                {
                    m_Passed = false;
                }
            }
        }

        private void PrintResults()
        {
            if(!m_Passed)
            {
                Console.WriteLine("✗ Some test cases failed. Corrected test cases: ");
            }
            else
            {
                Console.WriteLine("✔ All test cases passed!");
            }

            var prefix = "Utils.TC";
            var suffix = ",";

            foreach (var segmentCondition in m_Conditions) 
            {
                segmentCondition.PrintCorrectConditions(prefix, suffix);
            }
        }
    }
}
