namespace TestFramework
{
    public class LowerThanSegmentCondition : SegmentCondition
    {
        public LowerThanSegmentCondition(double start, double start_tolerance, double end, double end_tolerance, string property, double[] expected, bool analyze=false, SegmentType segmentType = SegmentType.Distance) 
        : base(start, start_tolerance, end, end_tolerance, property, expected, analyze, segmentType)  
        {
            Operator = new LowerOperator();
        }

        public LowerThanSegmentCondition(double start, double end, double time_tolerance, string property, double[] expected, bool analyze=false, SegmentType segmentType = SegmentType.Distance) 
        : base(start, end, time_tolerance, property, expected, analyze, segmentType)  
        {
            Operator = new LowerOperator();
        }
    };
}
