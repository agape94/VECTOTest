namespace TestFramework
{
    public class GreaterThanSegmentCondition : SegmentCondition
    {
        public GreaterThanSegmentCondition(TestSegment testSegment, string property, double value, bool analyze=false) : base(testSegment, property, value) 
        {
            Operator = new GreaterOperator();
        }
    };
}
