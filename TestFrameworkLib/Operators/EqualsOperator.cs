using NUnit.Framework;

namespace TestFramework
{
    public class EqualsOperator : IOperator
    {
        public EqualsOperator() {}

        public void Apply(double lhs, double rhs, string errorMessage = "")
        {
            Assert.That(lhs, Is.EqualTo(rhs), errorMessage);
        }

        public string Symbol()
        {
            return "==";
        }

        public string InverseSymbol()
        {
            return "!=";
        }

        public override string ToString()
        {
            return "Equals";
        }
    }
}