namespace TestFramework
{
    public class GreaterThanSegmentCondition : SegmentCondition
    {
        public GreaterThanSegmentCondition(double start, double start_tolerance, double end, double end_tolerance, string property, double value, bool analyze=false, SegmentType segmentType = SegmentType.Distance) 
        : base(start, start_tolerance, end, end_tolerance, property, value, analyze, segmentType)   
        {
            Operator = new GreaterOperator();
        }
    };
}
