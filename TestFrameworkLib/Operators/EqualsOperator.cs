using System;
namespace TestFramework
{
    public class EqualsOperator : IOperator
    {
        public EqualsOperator(double lhs, double rhs)
        {
            m_Lhs = lhs;
            m_Rhs = rhs;
        }

        public void apply()
        {
            Console.WriteLine("Equals operator...");
        }
    }
}