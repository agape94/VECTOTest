using NUnit.Framework;

namespace TUGraz.VectoCore.Tests.TestFramework
{
    public class GreaterOperator : IOperator
    {
        public GreaterOperator() {}

        public void Apply(double lhs, double[] rhs, string errorMessage = "")
        {
            double epsilon = 0.0000001;
            Assert.That(rhs.Length, Is.EqualTo(1));
            Assert.That(lhs, Is.GreaterThan(rhs[0] - epsilon), errorMessage);
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