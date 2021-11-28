using System;
namespace TestFramework
{
    public class GreaterOperator : IOperator
    {
        public GreaterOperator(double lhs, double rhs)
        {
            m_Lhs = lhs;
            m_Rhs = rhs;
        }

        public void apply()
        {
            Console.WriteLine("Greater operator...");
        }
    }
}