namespace TestFramework
{
    public class NoOpSegmentCondition : SegmentCondition
    {
        public NoOpSegmentCondition() 
        : base(0, 0, 0, 0, "", new double[]{})  
        {
            throw new System.Exception("Invalid segment condition!");
        }
    

        public override string GenerateFailMessage(double lhs)
        {
            return "";
        }

        public override string ToString()
        {
            return "Invalid Segment Condition!";
        }
    };
}
