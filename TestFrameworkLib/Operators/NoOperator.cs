using NUnit.Framework;

namespace TestFramework
{
    public class NoOperator : IOperator
    {
        public NoOperator() {}

        public void Apply(double lhs, double[] rhs, string errorMessage = "")
        {
            // We should never reach this point
            Assert.That(false);
        }

        public string Symbol()
        {
            return "N/A";
        }

        public string InverseSymbol()
        {
            return "N/A";
        }

        public override string ToString()
        {
            return "No Op";
        }
    }
}