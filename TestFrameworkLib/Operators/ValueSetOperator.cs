using NUnit.Framework;
using System;

namespace TestFramework
{
    public class ValueSetOperator : IOperator
    {
        private double DoubleCompareTolerance;
        public ValueSetOperator() 
        {
            DoubleCompareTolerance = 1e-6;
        }

        public ValueSetOperator(double tolerance) 
        {
            DoubleCompareTolerance = tolerance;
        }

        public void Apply(double lhs, double[] rhs, string errorMessage = "")
        {
            Assert.That(rhs.Length, Is.GreaterThan(0));
            Assert.True(Array.Exists(rhs, element => Math.Abs(lhs - element) <= DoubleCompareTolerance), errorMessage);
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