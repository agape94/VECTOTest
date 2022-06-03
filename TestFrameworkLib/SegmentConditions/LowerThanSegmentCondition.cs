using TUGraz.VectoCore.Models.Simulation.Data;
namespace TUGraz.VectoCore.Tests.TestFramework
{
    public class LowerThanSegmentCondition : SegmentCondition
    {
        public LowerThanSegmentCondition(double start, double start_tolerance, double end, double end_tolerance, double time_tolerance, string column_string, ModalResultField column_field, double[] expected, bool analyze=false, SegmentType segmentType = SegmentType.Distance) 
        : base(start, start_tolerance, end, end_tolerance, time_tolerance, column_string, column_field, new LowerOperator(), expected, analyze, segmentType)  {}

    };
}
