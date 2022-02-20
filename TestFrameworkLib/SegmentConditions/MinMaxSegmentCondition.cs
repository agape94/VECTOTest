namespace TestFramework
{
    public class MinMaxSegmentCondition : SegmentCondition
    {
        public MinMaxSegmentCondition(double start, double end, string property, double value, bool analyze=false, SegmentType segmentType = SegmentType.Distance) 
        : base(start, end, property, value, analyze, segmentType)  
        {
            Operator = new LowerOperator(); // TODO currently placeholder
        }
    };
}
