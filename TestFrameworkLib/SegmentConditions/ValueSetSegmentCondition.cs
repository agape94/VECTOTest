namespace TestFramework
{
    public class ValueSetSegmentCondition : SegmentCondition
    {
        public ValueSetSegmentCondition(double start, double start_tolerance, double end, double end_tolerance, string property, double[] expected, bool analyze=false, SegmentType segmentType = SegmentType.Distance) 
        : base(start, start_tolerance, end, end_tolerance, property, expected, analyze, segmentType)  
        {
            Operator = new LowerOperator(); // TODO currently placeholder
        }
    };
}
