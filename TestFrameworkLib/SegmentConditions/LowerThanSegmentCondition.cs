namespace TestFramework
{
    public class LowerThanSegmentCondition : SegmentCondition
    {
        public LowerThanSegmentCondition(TestSegment testSegment, string property, double value, bool analyze=false) : base(testSegment, property, value) 
        {
            Operator = new LowerOperator();
        }
    };
}
