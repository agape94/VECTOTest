using System;
using System.Collections.Generic;
namespace TestFramework
{
    public class TestSegment
    {
        public double m_start {get; set;}
        public double m_end {get; set;}
        public SegmentType m_type {get; set;}
        public List<DataRow> m_Data {get; set;}

        public TestSegment(double start, double end, List<DataRow> testData, SegmentType dt = SegmentType.Distance)
        {
            if (start >= end)
            {
                throw new System.ArgumentException(string.Format("start delimeter ({0}) greater than end delimeter ({1})", start, end));
            }
            m_start = start;
            m_end = end;
            m_type = dt;
            m_Data = testData;
        }
        public void print()
        {
            Console.Write("{0}", this.ToString());
        }

        public string ToString()
        {
            return string.Format("({0}, {1}, {2})", m_start, m_end, m_type);
        }
    }
}