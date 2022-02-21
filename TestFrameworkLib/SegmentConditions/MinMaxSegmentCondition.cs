namespace TestFramework
{
    public class MinMaxSegmentCondition : SegmentCondition
    {
        public MinMaxSegmentCondition(double start, double start_tolerance, double end, double end_tolerance, string property, double[] expected, bool analyze=false, SegmentType segmentType = SegmentType.Distance) 
        : base(start, start_tolerance, end, end_tolerance, property, expected, analyze, segmentType)   
        {
            Operator = new MinMaxOperator();
        }

        public override string GenerateFailMessage(double lhs)
        {
            return $"Fail ({FailPoint}{TypeMeasuringUnit()}): Expected '{Property}' {Operator.Symbol()} ({Expected[0]}, {Expected[1]}), Got: {lhs}";
        }

        public override string ToString()
        {
            var operator_string = ToAnalyze ? "Analyze_" + Operator.ToString() : Operator.ToString();
            var values = "(" + string.Join(", ", Expected) + ")";
            if(Start_Tolerance == 0 && End_Tolerance == 0)
            {
                return $"({Start}, {End}, {ModFileData.GetModFileHeaderVariableNameByValue(Property)}, Operator.{operator_string}, {values})";   
            }

            return $"({Start}, {Start_Tolerance}, {End}, {End_Tolerance}, {ModFileData.GetModFileHeaderVariableNameByValue(Property)}, Operator.{operator_string}, {values})";
        }
    };
}
