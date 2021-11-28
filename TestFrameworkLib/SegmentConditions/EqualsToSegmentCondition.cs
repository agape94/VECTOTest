namespace TestFramework
{
    public class EqualsToSegmentCondition : SegmentCondition
    {
        public EqualsToSegmentCondition(TestSegment testSegment, string property, double value, bool analyze=false) : base(testSegment, property, value, analyze) 
        {
            Operator = new EqualsOperator();
        }
    };
}
