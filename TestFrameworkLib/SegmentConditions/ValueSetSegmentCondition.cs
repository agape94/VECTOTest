using System;
using System.Data;
using System.Linq;
using System.Collections.Generic;
using TUGraz.VectoCore.Models.Simulation.Data;
using TUGraz.VectoCommon.Utils;

namespace TUGraz.VectoCore.Tests.TestFramework
{
    public class ValueSetSegmentCondition : SegmentCondition
    {
        public ValueSetSegmentCondition(double start, double start_tolerance, double end, double end_tolerance, double time_tolerance, string column_string, ModalResultField column_field, double[] expected, bool analyze=false, SegmentType segmentType = SegmentType.Distance) 
        : base(start, start_tolerance, end, end_tolerance, time_tolerance, column_string, column_field, new ValueSetOperator(), expected, analyze, segmentType)  {}

        public override string GenerateFailMessage(double lhs)
        {
            var expected_values = "[" + string.Join(", ", Expected) + "]";
            return $"Fail ({FailPoint}{TypeMeasuringUnit()}): Expected '{Column_String}' {Operator.Symbol()} {expected_values}, Got: {lhs}";
        }

        public override string ToString()
        {
            var segment_type_string = Type == SegmentType.Distance ? "" : ", SegmentType.Time";
            var expected_values = "new []{" + string.Join(", ", Expected) + "}";
            if(Time_Tolerance != 0)
            {
                return $"({Start_String}, {End_String}, {Time_Tolerance}, {Column_To_Print}, Operator.{Operator_To_Print}, {expected_values}{segment_type_string})";   
            }
            else if (Start_Tolerance == 0 && End_Tolerance == 0)
            {
                return $"({Start_String}, {End_String}, {Column_To_Print}, Operator.{Operator_To_Print}, {expected_values}{segment_type_string})";
            }

            return $"({Start_String}, {Start_Tolerance:0.00}, {End_String}, {End_Tolerance:0.00}, {Column_To_Print}, Operator.{Operator_To_Print}, {expected_values}{segment_type_string})";
        }

        public override void Analyze(ref ModalResults data)
        {
            TrueConditions.Clear();
            PreCheck(ref data);
            var actual_values = new List<double> (); 
            
            foreach (var dataLine in Data)
            {
                double value = GetValueToCheck(dataLine);
                bool contains = actual_values.Any(item => Math.Abs(item - value) <= double.Epsilon);
                if(!contains)
                {
                    actual_values.Add(value);
                }
            }
            actual_values.Sort();
            TrueConditions.Add(new ValueSetSegmentCondition(Start, Start_Tolerance, End, End_Tolerance, Time_Tolerance, Column_String, Column_Field, actual_values.ToArray(), analyze: false, segmentType: Type));
        }
    };
}
