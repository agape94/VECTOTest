using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using NUnit.Framework;

namespace TestFramework
{
    // TC(Greater, 50)
    // TC(MinMax, 50, 70)
    public static class Utils
    {
        public static SegmentCondition TC(
            (double val, double? tolerance) start, 
            (double val, double? tolerance) end, 
            string property, 
            Operator op, 
            dynamic first_expected_value, 
            dynamic second_expected_value=null)
        {
            SegmentCondition sc;
            switch (op) {
                case Operator.Lower:
                    sc = new LowerThanSegmentCondition(start.val, end.val, property, first_expected_value);
                    break;
                case Operator.Greater:
                    sc = new GreaterThanSegmentCondition(start.val, end.val, property, first_expected_value);
                    break;
                case Operator.Equals:
                    sc = new EqualsToSegmentCondition(start.val, end.val, property, first_expected_value);
                    break;
                // case Operator.MinMax:
                //     sc = new MinMaxSegmentCondition(testSegment, property, value);
                //     break;
                case Operator.Analyze_Lower:
                    sc = new LowerThanSegmentCondition(start.val, end.val, property, first_expected_value, analyze:true);
                    break;
                case Operator.Analyze_Greater:
                    sc = new GreaterThanSegmentCondition(start.val, end.val, property, first_expected_value, analyze:true);
                    break;
                case Operator.Analyze_Equals:
                    sc = new EqualsToSegmentCondition(start.val, end.val, property, first_expected_value, analyze:true);
                    break;
                // case Operator.Analyze_MinMax:
                //     sc = new MinMaxSegmentCondition(testSegment, property, value, analyze:true);
                //     break;
                default:
                    sc = new EqualsToSegmentCondition(start.val, end.val, property, first_expected_value); // TODO Place holder for now
                    break;
            }

            return sc;
        }
        public static SegmentCondition TC(
            (double val, double? tolerance) start, 
            (double val, double? tolerance) end, 
            string property, 
            Operator op, 
            dynamic [] value_set)
        {
            SegmentCondition sc;
            switch(op)
            {
                // case Operator.ValueSet:
                //     sc = new ValueSetSegmentCondition(testSegment, property, value);
                //     break;
                // case Operator.Analyze_ValueSet:
                //     sc = new ValueSetSegmentCondition(testSegment, property, value, analyze:true);
                //     break;
                default:
                    sc = new EqualsToSegmentCondition(start.val, end.val, property, value_set[0]); // TODO Place holder for now
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
