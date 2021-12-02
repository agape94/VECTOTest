using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using NUnit.Framework;

namespace TestFramework
{
    public static class Utils
    {
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

        public static SegmentCondition SegmentConditionFactoryMethod(Operator op, TestSegment testSegment, string property, double value)
        {
            SegmentCondition sc;
            switch (op) {
                case Operator.Lower:
                    sc = new LowerThanSegmentCondition(testSegment, property, value);
                    break;
                case Operator.Greater:
                    sc = new GreaterThanSegmentCondition(testSegment, property, value);
                    break;
                case Operator.Equals:
                    sc = new EqualsToSegmentCondition(testSegment, property, value);
                    break;
                case Operator.MinMax:
                    sc = new MinMaxSegmentCondition(testSegment, property, value);
                    break;
                case Operator.ValueSet:
                    sc = new ValueSetSegmentCondition(testSegment, property, value);
                    break;
                case Operator.Analyze_Lower:
                    sc = new LowerThanSegmentCondition(testSegment, property, value, analyze:true);
                    break;
                case Operator.Analyze_Greater:
                    sc = new GreaterThanSegmentCondition(testSegment, property, value, analyze:true);
                    break;
                case Operator.Analyze_Equals:
                    sc = new EqualsToSegmentCondition(testSegment, property, value, analyze:true);
                    break;
                case Operator.Analyze_MinMax:
                    sc = new MinMaxSegmentCondition(testSegment, property, value, analyze:true);
                    break;
                case Operator.Analyze_ValueSet:
                    sc = new ValueSetSegmentCondition(testSegment, property, value, analyze:true);
                    break;
                default:
                    sc = new EqualsToSegmentCondition(testSegment, property, value); // TODO Place holder for now
                    break;
            }

            return sc;
        }
    }
}
