
namespace TestFramework
{
    public enum DelimiterType
    {
        Distance,
        Time
    }

    public enum Operator
    {
        Lower,
        Greater,
        Equals
    }

    public class TestSegment
    {
        double m_start;
        double m_end;
        Operator m_operator;
        DelimiterType m_type;
        string m_property;


        public TestSegment(double start, double end, string property, Operator op, DelimiterType dt = DelimiterType.Distance)
        {
            if (start >= end)
            {
                throw new System.ArgumentException(string.Format("start delimeter ({0}) greater than end delimeter ({1})", start, end));
            }
            m_start = start;
            m_end = end;
            m_property = property;
            m_operator = op;
            m_type = dt;
        }

    }
}