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
            dynamic first_expected_value, 
            dynamic second_expected_value=null)
        {
            return SegmentConditionFactoryMethod(start, 0, end, 0, property, op, first_expected_value, second_expected_value);
        }

        public static SegmentCondition TC(
            double start,
            double start_tolerance,
            double end, 
            double end_tolerance,
            string property, 
            Operator op, 
            dynamic first_expected_value, 
            dynamic second_expected_value=null)
        {
            return SegmentConditionFactoryMethod(start, start_tolerance, end, end_tolerance, property, op, first_expected_value, second_expected_value);
        }

        // ===============================================================================================

        public static SegmentCondition TC(
            double start, 
            double end, 
            string property, 
            Operator op, 
            dynamic [] value_set)
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
            dynamic [] value_set)
        {
            return SegmentConditionFactoryMethod(start, start_tolerance, end, end_tolerance, property, op, value_set);
        }

        private static SegmentCondition SegmentConditionFactoryMethod(
            double start_val,
            double start_tolerance, 
            double end_val,
            double end_tolerance, 
            string property, 
            Operator op, 
            dynamic first_expected_value, 
            dynamic second_expected_value=null)
        {
            SegmentCondition sc;
            switch (op) {
                case Operator.Lower:
                    sc = new LowerThanSegmentCondition(start_val, start_tolerance, end_val, end_tolerance, property, first_expected_value);
                    break;
                case Operator.Greater:
                    sc = new GreaterThanSegmentCondition(start_val, start_tolerance, end_val, end_tolerance, property, first_expected_value);
                    break;
                case Operator.Equals:
                    sc = new EqualsToSegmentCondition(start_val, start_tolerance, end_val, end_tolerance, property, first_expected_value);
                    break;
                case Operator.Analyze_Lower:
                    sc = new LowerThanSegmentCondition(start_val, start_tolerance, end_val, end_tolerance, property, first_expected_value, analyze:true);
                    break;
                case Operator.Analyze_Greater:
                    sc = new GreaterThanSegmentCondition(start_val, start_tolerance, end_val, end_tolerance, property, first_expected_value, analyze:true);
                    break;
                case Operator.Analyze_Equals:
                    sc = new EqualsToSegmentCondition(start_val, start_tolerance, end_val, end_tolerance, property, first_expected_value, analyze:true);
                    break;
                default:
                    sc = new EqualsToSegmentCondition(start_val, start_tolerance, end_val, end_tolerance, property, first_expected_value); // TODO Place holder for now
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
            dynamic [] value_set)
        {
            SegmentCondition sc;
            switch(op)
            {
                // case Operator.ValueSet:
                //     sc = new ValueSetSegmentCondition(start_val, end_val, property, first_expected_value);
                //     break;
                // case Operator.Analyze_ValueSet:
                //     sc = new ValueSetSegmentCondition(start_val, end_val, property, first_expected_value, analyze: true);
                //     break;
                default:
                    sc = new EqualsToSegmentCondition(start_val, start_tolerance, end_val, end_tolerance, property, value_set[0]); // TODO Place holder for now
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
