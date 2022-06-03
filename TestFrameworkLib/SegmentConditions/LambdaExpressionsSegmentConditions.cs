using System;
using System.Data;
using NUnit.Framework;
using TUGraz.VectoCore.Models.Simulation.Data;
using TUGraz.VectoCommon.Utils;

namespace TUGraz.VectoCore.Tests.TestFramework
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

        protected override void PreCheck(ref ModalResults data)
        {
            if(DonePreCheck)
            {
                return;
            }
            Assert.That(Start_Adjusted, Is.LessThanOrEqualTo(End_Adjusted));
            Data = data.AsEnumerable();
            DonePreCheck = true;
        }


        public override void Check(ref ModalResults data)
        {
            PreCheck(ref data);
            foreach (var dataLine in Data) {
                FailPoint = dataLine.Field<SI>(ModalResultField.dist.GetName()).Value();
                try {
                    if(m_FirstExpression(dataLine) == true)
                    {
                        Assert.That(m_SecondExpression(dataLine) == true);
                    }
                } catch (AssertionException) 
                {
                    Passed = false;
                    break;
                }
            }
        }

        public override void Analyze(ref ModalResults data)
        {
            return;
        }

        public override void PrintCorrectConditions(string prefix = "", string suffix = "")
        {
            return;
        }

        public override void PrintResults(string prefix = "\t*", string suffix = "")
        {
            Console.WriteLine("{0} -> {1}", ToString(), Passed ? "✔ Pass" : "✗ Fail");
            if(!Passed)
            {
                Print(prefix:"\t* ");
            }
        }

        public override void Print(string prefix = "", string suffix = "")
        {
            Console.WriteLine($"{prefix}Lambda expression failed at position: {FailPoint} m{suffix}");
        }

        public override string ToString()
        {
            return "Lambda expression";
        }
    }}
