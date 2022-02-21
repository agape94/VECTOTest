using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using NUnit.Framework;

namespace TestFramework
{    public static class Utils
    {
        public static SegmentCondition TC(
            double start, 
            double end, 
            string property, 
            Operator op, 
            double expected_value)
        {
            return SegmentConditionFactoryMethod(start, 0, end, 0, property, op, expected_value);
        }

        public static SegmentCondition TC(
            double start,
            double start_tolerance,
            double end, 
            double end_tolerance,
            string property, 
            Operator op, 
            double expected_value)
        {
            return SegmentConditionFactoryMethod(start, start_tolerance, end, end_tolerance, property, op, expected_value);
        }

        // ===============================================================================================

        public static SegmentCondition TC(
            double start, 
            double end, 
            string property, 
            Operator op, 
            (double min, double max) expected_values)
        {
            return SegmentConditionFactoryMethod(start, 0, end, 0, property, op, new[] {expected_values.min, expected_values.max});
        }

        public static SegmentCondition TC(
            double start,
            double start_tolerance,
            double end, 
            double end_tolerance,
            string property, 
            Operator op, 
            (double min, double max) expected_values)
        {
            return SegmentConditionFactoryMethod(start, start_tolerance, end, end_tolerance, property, op, new[] {expected_values.min, expected_values.max});
        }

        // ===============================================================================================

        public static SegmentCondition TC(
            double start, 
            double end, 
            string property, 
            Operator op, 
            double [] value_set)
        {
            return SegmentConditionFactoryMethod(start, 0, end, 0, property, op, value_set);
        }

        public static SegmentCondition TC(
            double start,
            double start_tolerance,
            double end, 
            double end_tolerance,
            string property, 
            Operator op, 
            double [] value_set)
        {
            return SegmentConditionFactoryMethod(start, start_tolerance, end, end_tolerance, property, op, value_set);
        }

        public static SegmentCondition TC(
            double start, 
            double end, 
            string property, 
            Operator op, 
            int [] value_set)
        {
            return SegmentConditionFactoryMethod(start, 0, end, 0, property, op, Array.ConvertAll<int, double>(value_set, x => x));
        }

        public static SegmentCondition TC(
            double start,
            double start_tolerance,
            double end, 
            double end_tolerance,
            string property, 
            Operator op, 
            int [] value_set)
        {
            return SegmentConditionFactoryMethod(start, start_tolerance, end, end_tolerance, property, op, Array.ConvertAll<int, double>(value_set, x => x));
        }

        private static SegmentCondition SegmentConditionFactoryMethod(
            double start_val,
            double start_tolerance, 
            double end_val,
            double end_tolerance, 
            string property, 
            Operator op, 
            double first_expected_value)
        {
            SegmentCondition sc;
            switch (op) {
                case Operator.Lower:
                    sc = new LowerThanSegmentCondition(start_val, start_tolerance, end_val, end_tolerance, property, new []{first_expected_value});
                    break;
                case Operator.Greater:
                    sc = new GreaterThanSegmentCondition(start_val, start_tolerance, end_val, end_tolerance, property, new []{first_expected_value});
                    break;
                case Operator.Equals:
                    sc = new EqualsToSegmentCondition(start_val, start_tolerance, end_val, end_tolerance, property, new []{first_expected_value});
                    break;
                case Operator.Analyze_Lower:
                    sc = new LowerThanSegmentCondition(start_val, start_tolerance, end_val, end_tolerance, property, new []{first_expected_value}, analyze:true);
                    break;
                case Operator.Analyze_Greater:
                    sc = new GreaterThanSegmentCondition(start_val, start_tolerance, end_val, end_tolerance, property, new []{first_expected_value}, analyze:true);
                    break;
                case Operator.Analyze_Equals:
                    sc = new EqualsToSegmentCondition(start_val, start_tolerance, end_val, end_tolerance, property, new []{first_expected_value}, analyze:true);
                    break;
                default:
                    sc = new NoOpSegmentCondition();
                    break;
            }

            return sc;
        }
        private static SegmentCondition SegmentConditionFactoryMethod(
            double start_val,
            double start_tolerance, 
            double end_val,
            double end_tolerance, 
            string property, 
            Operator op, 
            double [] value_set)
        {
            SegmentCondition sc;
            switch(op)
            {
                case Operator.ValueSet:
                    sc = new ValueSetSegmentCondition(start_val, start_tolerance, end_val, end_tolerance, property, value_set); /* TODO handle segment type on all cases */
                    break;
                case Operator.Analyze_ValueSet:
                    sc = new ValueSetSegmentCondition(start_val, start_tolerance, end_val, end_tolerance, property, value_set, analyze: true);
                    break;
                case Operator.MinMax:
                    sc = new MinMaxSegmentCondition(start_val, start_tolerance, end_val, end_tolerance, property, value_set);
                    break;
                case Operator.Analyze_MinMax:
                    sc = new MinMaxSegmentCondition(start_val, start_tolerance, end_val, end_tolerance, property, value_set, analyze: true);
                    break;
                default:
                    sc = new NoOpSegmentCondition();
                    break;
            }

            return sc;
        }
        public static bool IsValidHeader(string header_to_check)
        {
            var headers = ModFileData.GetModFileHeaders();
            foreach (var header in headers) {
                if (header_to_check == header) {
                    return true;
                }
            }
            return false;
        }
    }
}
