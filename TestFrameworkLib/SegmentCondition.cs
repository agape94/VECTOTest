namespace TestFramework
{
    public class SegmentCondition
    {
        TestSegment m_Segment;
        Operator m_Operator;
        string m_Property;
        double m_Value;

        public SegmentCondition(TestSegment testSegment, Operator op, string property, double value)
        {
            m_Segment = testSegment;
            m_Operator = op;
            m_Property = property;
            m_Value = value;
        }

        public bool check()
        {
            // TODO Check the condition on the segment
            return true;
        }

    }
}