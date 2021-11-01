using System.Reflection;
using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace TestFramework
{
    public static class Utils
    {
        public static bool IsValidHeader(string header_to_check)
        {
            var headers = GetHeaders();
            foreach(var header in headers)
            {
                if(header_to_check == header)
                {
                    return true;
                }
            }
            return false;
        }

        public static List<string> GetHeaders()
        {
            List<string> headers = new List<string>();

            Type type = typeof (ModFileHeader);
            var fields = type.GetFields(BindingFlags.Static | BindingFlags.Public);

            foreach (FieldInfo field in fields)
            {
                var fieldValue = (string)field.GetValue(null);
                headers.Add(fieldValue);
            }
            return headers;
        }

        public static void ApplyOperator(double lhs, Operator op, double rhs, string errorMessage = "")
        {
            switch(op)
            {
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

        public static string Symbol(Operator op, bool inverse=false)
        {
            switch(op)
            {
                case Operator.Lower:
                    return inverse ? ">=" : "<";
                case Operator.Greater:
                    return inverse ? "<=" : ">";
                case Operator.Equals:
                    return inverse ? "!=" : "=";
            }
            return "";
        }
    }
}