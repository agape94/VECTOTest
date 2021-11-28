namespace TestFramework
{
    public class ValueSetSegmentCondition : SegmentCondition
    {
        public ValueSetSegmentCondition(TestSegment testSegment, string property, double value, bool analyze=false) : base(testSegment, property, value) 
        {
            Operator = new LowerOperator();
        }
    };
}
