using NUnit.Framework;
using System;

namespace TUGraz.VectoCore.Tests.TestFramework
{
    public class ValueSetOperator : IOperator
    {
        public ValueSetOperator() {}

        public void Apply(double lhs, double[] rhs, string errorMessage = "")
        {
            double epsilon = 0.0000001;
            Assert.That(rhs.Length, Is.GreaterThan(0));
            Assert.True(Array.Exists(rhs, element => Math.Abs(lhs - element) <= epsilon), errorMessage);
        }

        public string Symbol()
        {
            return "Any Of";
        }

        public string InverseSymbol()
        {
            return "Outside";
        }

        public override string ToString()
        {
            return "ValueSet";
        }
    }
}