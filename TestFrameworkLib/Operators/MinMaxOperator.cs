using NUnit.Framework;

namespace TUGraz.VectoCore.Tests.TestFramework
{
    public class MinMaxOperator : IOperator
    {
        public MinMaxOperator() {}

        public void Apply(double lhs, double[] rhs, string errorMessage = "")
        {
            Assert.That(rhs.Length, Is.EqualTo(2));
            if(rhs[0] > rhs[1])
            {
                double tmp = rhs[0];
                rhs[0] = rhs[1];
                rhs[1] = tmp;
            }

            double epsilon = 0.0000001;

            Assert.That(lhs, Is.GreaterThanOrEqualTo(rhs[0] - epsilon), errorMessage);
            Assert.That(lhs, Is.LessThanOrEqualTo(rhs[1] + epsilon), errorMessage);
        }

        public string Symbol()
        {
            return "Between";
        }

        public string InverseSymbol()
        {
            return "Outside";
        }

        public override string ToString()
        {
            return "MinMax";
        }
    }
}