namespace TestFramework
{
    public class GreaterThanSegmentCondition : SegmentCondition
    {
        public GreaterThanSegmentCondition(double start, double end, string property, double value, bool analyze=false, SegmentType segmentType = SegmentType.Distance) 
        : base(start, end, property, value, analyze, segmentType)  
        {
            Operator = new GreaterOperator();
        }
    };
}
