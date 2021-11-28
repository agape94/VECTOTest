using System;
namespace TestFramework
{
    public class LowerOperator : IOperator
    {
        public LowerOperator(double lhs, double rhs)
        {
            m_Lhs = lhs;
            m_Rhs = rhs;
        }

        public void apply()
        {
            Console.WriteLine("Lower operator...");
        }
    }
}