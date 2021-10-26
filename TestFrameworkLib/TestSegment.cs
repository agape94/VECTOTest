namespace TestFramework
{
    public class TestSegment
    {
        double m_start;
        double m_end;
        DelimiterType m_type;

        public TestSegment(double start, double end, DelimiterType dt = DelimiterType.Distance)
        {
            if (start >= end)
            {
                throw new System.ArgumentException(string.Format("start delimeter ({0}) greater than end delimeter ({1})", start, end));
            }
            m_start = start;
            m_end = end;
            m_type = dt;
        }

    }
}