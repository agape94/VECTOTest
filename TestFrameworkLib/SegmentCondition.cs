using System;
namespace TestFramework
{
    public class SegmentCondition
    {
        public TestSegment m_Segment {get; set;}
        public Operator m_Operator {get; set;}
        public string m_Property {get; set;}
        public double m_Value {get; set;}
        public bool m_Passed {get; set;}

        public SegmentCondition(TestSegment testSegment, string property, Operator op, double value)
        {
            m_Segment = testSegment;
            m_Operator = op;
            m_Property = property;
            m_Value = value;
            m_Passed = false;
        }

        public bool check()
        {
            foreach(var dataLine in m_Segment.m_Data)
            {
                Utils.ApplyOperator(dataLine[m_Property], m_Operator, m_Value, this.GenerateFailMessage(dataLine[m_Property]));
            }
            return true;
        }

        public void print()
        {
            Console.Write("Segment Condition: {0}\n", this.ToString());
        }

        public string ToString()
        {
            return string.Format("[{0}, {1}, {2}, {3}]", m_Segment.ToString(), m_Property, m_Operator, m_Value);
        }

        private string GenerateFailMessage(double lhs)
        {
            return string.Format("Fail: Expected '{0}' {1} {2}. Got: {3}", m_Property, Utils.Symbol(m_Operator), m_Value, lhs);
        }

        private string GeneratePassMessage(double lhs)
        {
            return string.Format("Pass: Expected '{0}' {1} {2}. Got: {3}", m_Property, Utils.Symbol(m_Operator), m_Value, lhs);
        }
    }
}