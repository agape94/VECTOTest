using System;
using System.Linq;
using System.Collections.Generic;

namespace TestFramework
{
    public class ValueSetSegmentCondition : SegmentCondition
    {
        private double DoubleCompareTolerance;
        public ValueSetSegmentCondition(double start, double start_tolerance, double end, double end_tolerance, string property, double[] expected, bool analyze=false, SegmentType segmentType = SegmentType.Distance) 
        : base(start, start_tolerance, end, end_tolerance, property, expected, analyze, segmentType)  
        {
            DoubleCompareTolerance = 1e-6;
            Operator = new ValueSetOperator(DoubleCompareTolerance);
        }

        public override string GenerateFailMessage(double lhs)
        {
            var expected_values = "[" + string.Join(", ", Expected) + "]";
            return $"Fail ({FailPoint}{TypeMeasuringUnit()}): Expected '{Property}' {Operator.Symbol()} {expected_values}, Got: {lhs}";
        }

        public override string ToString()
        {
            var operator_string = ToAnalyze ? "Analyze_" + Operator.ToString() : Operator.ToString();
            var expected_values = "new[]{" + string.Join(", ", Expected) + "}";
            if(Start_Tolerance == 0 && End_Tolerance == 0)
            {
                return $"({Start}, {End}, {ModFileData.GetModFileHeaderVariableNameByValue(Property)}, Operator.{operator_string}, {expected_values})";   
            }

            return $"({Start}, {Start_Tolerance}, {End}, {End_Tolerance}, {ModFileData.GetModFileHeaderVariableNameByValue(Property)}, Operator.{operator_string}, {expected_values})";
        }

        public override void Analyze()
        {
            TrueConditions.Clear();
            var actual_values = new List<double> (); 
            
            foreach (var dataLine in Data)
            {
                double value = dataLine[Property];
                bool contains = actual_values.Any(item => Math.Abs(item - value) <= DoubleCompareTolerance);
                if(!contains)
                {
                    actual_values.Add(value);
                }
            }
            actual_values.Sort();
            TrueConditions.Add(new ValueSetSegmentCondition(Start, Start_Tolerance, End, End_Tolerance, Property, actual_values.ToArray(), analyze: false, segmentType: Type));
        }
    };
}
