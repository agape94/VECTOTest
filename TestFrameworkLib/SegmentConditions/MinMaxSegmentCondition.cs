namespace TestFramework
{
    public class MinMaxSegmentCondition : SegmentCondition
    {
        public MinMaxSegmentCondition(TestSegment testSegment, string property, double value, bool analyze=false) : base(testSegment, property, value) 
        {
            Operator = new LowerOperator();
        }
    };
}
