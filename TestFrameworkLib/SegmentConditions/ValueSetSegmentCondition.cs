namespace TestFramework
{
    public class ValueSetSegmentCondition : SegmentCondition
    {
        public ValueSetSegmentCondition(double start, double end, string property, double value, bool analyze=false, SegmentType segmentType = SegmentType.Distance) 
        : base(start, end, property, value, analyze, segmentType)  
        {
            Operator = new LowerOperator(); // TODO currently placeholder
        }
    };
}
