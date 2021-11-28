using NUnit.Framework;

namespace TestFramework
{
    public class GreaterOperator : IOperator
    {
        public GreaterOperator() {}

        public void Apply(double lhs, double rhs, string errorMessage = "")
        {
            Assert.That(lhs, Is.GreaterThan(rhs), errorMessage);
        }

        public string Symbol()
        {
            return ">";
        }

        public string InverseSymbol()
        {
            return "<=";
        }

        public override string ToString()
        {
            return "Greater";
        }
    }
}