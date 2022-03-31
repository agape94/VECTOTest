using System;
using NUnit.Framework;

namespace TestFramework
{
    public class LamdaExpressionsSegmentCondition : SegmentCondition
    {
        Func<DataRow, bool> m_FirstExpression;
        Func<DataRow, bool> m_SecondExpression;
        public LamdaExpressionsSegmentCondition(Func<DataRow, bool> expression_1, Func<DataRow, bool> expression_2) : base()
        {
            Operator = new NoOperator();
            m_FirstExpression = expression_1;
            m_SecondExpression = expression_2;
        }

        public override void Check()
        {
            foreach (var dataLine in Data) {
                try {
                    FailPoint = dataLine[TypeColumnName()];
                    if(m_FirstExpression(dataLine) == true)
                    {
                        Assert.That(m_SecondExpression(dataLine) == true);
                    }
                } catch (Exception) {
                    Passed = false;
                    Analyze();
                    break;
                }
            }
        }

        public override void Analyze()
        {
            return;
        }

        public override string ToString()
        {
            return "Lambda expression";
        }
    };
}
