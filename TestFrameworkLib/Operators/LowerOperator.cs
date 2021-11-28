using NUnit.Framework;

namespace TestFramework
{
    public class LowerOperator : IOperator
    {
        public LowerOperator() {}

        public void Apply(double lhs, double rhs, string errorMessage = "")
        {
            Assert.That(lhs, Is.LessThan(rhs), errorMessage);
        }

        public string Symbol()
        {
            return "<";
        }

        public string InverseSymbol()
        {
            return ">=";
        }

        public override string ToString()
        {
            return "Lower";
        }
    }
}