using System.Reflection;
using System.Collections.Generic;
using NUnit.Framework;

namespace TestFramework
{
    public static class Utils
    {
        public static bool IsValidHeader(string header_to_check)
        {
            var headers = GetHeaders();
            foreach (var header in headers) {
                if (header_to_check == header) {
                    return true;
                }
            }
            return false;
        }

        public static List<string> GetHeaders()
        {
            var headers = new List<string>();

            var type = typeof(ModFileHeader);
            var fields = type.GetFields(BindingFlags.Static | BindingFlags.Public);

            foreach (var field in fields) {
                var fieldValue = (string)field.GetValue(null);
                headers.Add(fieldValue);
            }
            return headers;
        }

        public static void ApplyOperator(double lhs, Operator op, double rhs, string errorMessage = "")
        {
            switch (op) {
                case Operator.Lower:
                    Assert.That(lhs, Is.LessThan(rhs), errorMessage);
                    break;
                case Operator.Greater:
                    Assert.That(lhs, Is.GreaterThan(rhs), errorMessage);
                    break;
                case Operator.Equals:
                    Assert.That(lhs, Is.EqualTo(rhs), errorMessage);
                    break;
            }
        }

        public static IOperator OperatorFactoryMethod(Operator op)
        {
            IOperator concreteOperator;
            switch (op) {
                case Operator.Lower:
                    concreteOperator = new LowerOperator();
                    break;
                case Operator.Greater:
                    concreteOperator = new GreaterOperator();
                    break;
                case Operator.Equals:
                    concreteOperator = new EqualsOperator();
                    break;
                default:
                    return new EqualsOperator(); // TODO Place holder for now
            }

            return concreteOperator;
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
                    return new EqualsToSegmentCondition(testSegment, property, value); // TODO Place holder for now
            }

            return sc;
        }

        // public static List<SegmentCondition> GetCorrectTestSegments(SegmentCondition condition)
        // {
        //     bool areAllEqual = true;
        //     double maxValue = double.MinValue;
        //     double minValue = double.MaxValue;
        //     double oldValue = condition.Segment.Data[0][condition.Property];

        //     foreach (var dataLine in condition.Segment.Data)
        //     {
        //         double actualValue = dataLine[condition.Property];
        //         if(oldValue != actualValue && areAllEqual)
        //         {
        //             areAllEqual = false;
        //         }

        //         if(actualValue > maxValue)
        //         {
        //             maxValue = actualValue;
        //         }
        //         else if(actualValue < minValue)
        //         {
        //             minValue = actualValue;
        //         }
        //         oldValue = actualValue;
        //     }

        //     if(areAllEqual)
        //     {
        //         // return (start, end, Property, Equals, oldValue)
        //         SegmentCondition sc = new SegmentCondition(
        //             condition.Segment,
        //             condition.Property,
        //             new EqualsOperator(),
        //             oldValue
        //             );

        //         return new List<SegmentCondition>(){sc}; 
        //     }
        //     else
        //     {
        //         // return (start, end, Property, Greater, minValue), (start, end, Property, Lower, maxValue)
        //         SegmentCondition sc_greater = new SegmentCondition(
        //             condition.Segment,
        //             condition.Property,
        //             new GreaterOperator(),
        //             minValue
        //             );
        //         SegmentCondition sc_lower = new SegmentCondition(
        //             condition.Segment,
        //             condition.Property,
        //             new LowerOperator(),
        //             maxValue
        //             );

        //         return new List<SegmentCondition>(){sc_greater, sc_lower}; 
            
        //     }

        // }

        public static string Symbol(Operator op, bool inverse = false)
        {
            switch (op) {
                case Operator.Lower:
                    return inverse ? ">=" : "<";
                case Operator.Greater:
                    return inverse ? "<=" : ">";
                case Operator.Equals:
                    return inverse ? "!=" : "=";
            }
            return "";
        }

        // // usage: WriteColor("This is my [message] with inline [color] changes.", ConsoleColor.Yellow);
        // public static void WriteColor(string message, ConsoleColor color)
        // {
        //     var pieces = Regex.Split(message, @"(\[[^\]]*\])");

        //     for(int i=0;i<pieces.Length;i++)
        //     {
        //         string piece = pieces[i];

        //         if (piece.StartsWith("[") && piece.EndsWith("]"))
        //         {
        //             Console.ForegroundColor = color;
        //             piece = piece.Substring(1,piece.Length-2);          
        //         }

        //         Console.Write(piece);
        //         Console.ResetColor();
        //     }

        //     Console.WriteLine();
        // }
    }
}
