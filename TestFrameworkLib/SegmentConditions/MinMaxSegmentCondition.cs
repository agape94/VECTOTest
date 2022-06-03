using TUGraz.VectoCore.Models.Simulation.Data;
namespace TUGraz.VectoCore.Tests.TestFramework
{
    public class MinMaxSegmentCondition : SegmentCondition
    {
        public MinMaxSegmentCondition(double start, double start_tolerance, double end, double end_tolerance, double time_tolerance, string column_string, ModalResultField column_field, double[] expected, bool analyze=false, SegmentType segmentType = SegmentType.Distance) 
        : base(start, start_tolerance, end, end_tolerance, time_tolerance, column_string, column_field, new MinMaxOperator(), expected, analyze, segmentType)   {}

        public override string GenerateFailMessage(double lhs)
        {
            return $"Fail ({FailPoint}{TypeMeasuringUnit()}): Expected '{Column_String}' {Operator.Symbol()} ({Expected[0]}, {Expected[1]}), Got: {lhs}";
        }

        public override string ToString()
        {
            var segment_type_string = Type == SegmentType.Distance ? "" : ", SegmentType.Time";
            var values = "(" + Expected[0].ToString("0.00") + ", " + Expected[1].ToString("0.00") + ")";
            if(Time_Tolerance != 0)
            {
                return $"({Start_String}, {End_String}, {Time_Tolerance}, {Column_To_Print}, Operator.{Operator_To_Print}, {values}{segment_type_string})";   
            }
            else if (Start_Tolerance == 0 && End_Tolerance == 0)
            {
                return $"({Start_String}, {End_String}, {Column_To_Print}, Operator.{Operator_To_Print}, {values}{segment_type_string})";
            }
            return $"({Start_String}, {Start_Tolerance:0.00}, {End_String}, {End_Tolerance:0.00}, {Column_To_Print}, Operator.{Operator_To_Print}, {values}{segment_type_string})";
        }
    };
}
