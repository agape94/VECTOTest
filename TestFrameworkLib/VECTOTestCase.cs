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

        public VECTOTestCase(string jobname, params SegmentCondition[] segmentConditions)
        {
            m_Data = new ModFileData();
            m_Conditions = new List<SegmentCondition>();
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
            }
        }

        private void PrintResults()
        {
            foreach (var segmentCondition in m_Conditions) {
                segmentCondition.PrintResults();
            }
        }

    }
}
