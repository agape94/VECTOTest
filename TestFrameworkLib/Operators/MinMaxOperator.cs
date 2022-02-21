using NUnit.Framework;

namespace TestFramework
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

            Assert.That(lhs, Is.GreaterThan(rhs[0]), errorMessage);
            Assert.That(lhs, Is.LessThan(rhs[1]), errorMessage);
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