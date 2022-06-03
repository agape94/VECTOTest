using TUGraz.VectoCore.Models.Simulation.Data;
namespace TUGraz.VectoCore.Tests.TestFramework
{
    public class NoOpSegmentCondition : SegmentCondition
    {
        public NoOpSegmentCondition() 
        : base(0, 0, 0, 0, 0, "", ModalResultField.INVALID, new NoOperator(), new double[]{}, false, SegmentType.Distance)  
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
