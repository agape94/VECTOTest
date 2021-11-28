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

        public static List<SegmentCondition> GetCorrectTestSegments(SegmentCondition condition)
        {
            bool areAllEqual = true;
            double maxValue = double.MinValue;
            double minValue = double.MaxValue;
            double oldValue = condition.Segment.Data[0][condition.Property];

            foreach (var dataLine in condition.Segment.Data)
            {
                double actualValue = dataLine[condition.Property];
                if(oldValue != actualValue && areAllEqual)
                {
                    areAllEqual = false;
                }

                if(actualValue > maxValue)
                {
                    maxValue = actualValue;
                }
                else if(actualValue < minValue)
                {
                    minValue = actualValue;
                }
                oldValue = actualValue;
            }

            if(areAllEqual)
            {
                // return (start, end, Property, Equals, oldValue)
                SegmentCondition sc = new SegmentCondition(
                    condition.Segment,
                    condition.Property,
                    Operator.Equals,
                    oldValue
                    );

                return new List<SegmentCondition>(){sc}; 
            }
            else
            {
                // return (start, end, Property, Greater, minValue), (start, end, Property, Lower, maxValue)
                SegmentCondition sc_greater = new SegmentCondition(
                    condition.Segment,
                    condition.Property,
                    Operator.Greater,
                    minValue
                    );
                SegmentCondition sc_lower = new SegmentCondition(
                    condition.Segment,
                    condition.Property,
                    Operator.Lower,
                    maxValue
                    );

                return new List<SegmentCondition>(){sc_greater, sc_lower}; 
            
            }

        }

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
