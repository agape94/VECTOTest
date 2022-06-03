using NUnit.Framework;

namespace TUGraz.VectoCore.Tests.TestFramework
{
    public class EqualsOperator : IOperator
    {
        public EqualsOperator() {}

        public void Apply(double lhs, double[] rhs, string errorMessage = "")
        {
            Assert.That(rhs.Length, Is.EqualTo(1));
            Assert.That(lhs, Is.EqualTo(rhs[0]), errorMessage);
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