using NUnit.Framework;
using System.Collections.Generic;
using System.IO;
using System;
using System.Data;
using System.Linq;
using TUGraz.VectoCommon.Models;
using TUGraz.VectoCommon.Utils;
using TUGraz.VectoCore.InputData.FileIO.JSON;
using TUGraz.VectoCore.Models.Simulation.Data;
using TUGraz.VectoCore.Models.Simulation.Impl.SimulatorFactory;
using TUGraz.VectoCore.OutputData;
using TUGraz.VectoCore.OutputData.FileIO;
using TUGraz.VectoCore.Utils;

namespace TUGraz.VectoCore.Tests.TestFramework
{
    public abstract class VECTOTest
    {
        private List<SegmentCondition> m_Conditions;
        private bool m_AllPassed;
        private int m_PassedTestCasesCount;
        private int m_TestCasesCount;

        public abstract string BasePath { get; }

        public VECTOTest() {}

        public void RunTestCases(string jobNameCycleName, params SegmentCondition[] segmentConditions)
        {
            var jobName = jobNameCycleName.Split('_').Slice(0, -2).Join("_");
			var cycleName = jobNameCycleName.Split('_').Reverse().Skip(1).First();

            jobName = Path.Combine(BasePath, jobName + ".vecto");

			var inputData = JSONInputDataFactory.ReadJsonJob(jobName);
			var writer = new FileOutputWriter(Path.Combine(Path.GetDirectoryName(jobName), Path.GetFileName(jobName)));
			var sumContainer = new SummaryDataContainer(writer);
			var factory = SimulatorFactory.CreateSimulatorFactory(ExecutionMode.Engineering, inputData, writer);
			
            factory.WriteModalResults = true;
			factory.Validate = false;
			factory.SumData = sumContainer;

			var run = factory.SimulationRuns().First(r => r.CycleName == cycleName);
            var mod = (run.GetContainer().ModalData as ModalDataContainer).Data;
			run.Run();
			Assert.IsTrue(run.FinishedWithoutErrors);
            
            m_Conditions = new List<SegmentCondition>();
            m_AllPassed = true;
            m_PassedTestCasesCount = 0;
            m_TestCasesCount = segmentConditions.Length;

            Console.WriteLine("\n====== JOB INFORMATION ======");
            Console.WriteLine($"  JOB: {jobName}");
            Console.WriteLine($"CYCLE: {cycleName}");
            Console.WriteLine($" TEST: {jobNameCycleName}");
            Console.WriteLine("\n====== DETAILED RESULTS =====");            
            foreach (var sc in segmentConditions) {
                try {
                    m_Conditions.Add(sc);
                    sc.Check(ref mod);
                    sc.PrintResults();
                    if(sc.Passed)
                    {
                        m_PassedTestCasesCount++;
                    }
                    else
                    {
                        if(m_AllPassed)
                        {
                            m_AllPassed = false;
                        }
                    }
                } catch (Exception e) {
                    Console.Error.WriteLine(e.ToString());
                }
            }         
            PrintResults();
        }

        private void PrintResults()
        {
            string indicator = "✔";
            
            if(!m_AllPassed)
            {
                indicator = "✗";
                Console.WriteLine($"\n{indicator} {m_PassedTestCasesCount}/{m_TestCasesCount} test cases passed");
                Console.WriteLine("\n==================");
                Console.WriteLine("CORRECT TEST CASES\n");
                

                var prefix = "TC";
                var suffix = ",";

                foreach (var segmentCondition in m_Conditions) 
                {
                    segmentCondition.PrintCorrectConditions(prefix, suffix);
                }

                Console.WriteLine("\n==================\n");
            }
            else
            {
                Console.WriteLine($"\n{indicator} {m_PassedTestCasesCount}/{m_TestCasesCount} test cases passed");
            }
        }

        // ===========================================================================================================
        // =========================================== TEST CASE FUNCTIONS ===========================================
        // ===========================================================================================================
        // single expected value

        // ModaResultField column
        public static SegmentCondition TC(double start, double end, ModalResultField column, Operator op, double expected_value, SegmentType sg = SegmentType.Distance)
        {
            return SegmentConditionFactoryMethod(start, 0, end, 0, 0, "", column, op, new double[] {Convert.ToDouble(expected_value)}, analyze:false, sg);
        }

        public static SegmentCondition TC(double start, double end, double time_tolerance_seconds, ModalResultField column, Operator op, double expected_value, SegmentType sg = SegmentType.Distance)
        {
            return SegmentConditionFactoryMethod(start, 0, end, 0, time_tolerance_seconds, "", column, op, new double[] {Convert.ToDouble(expected_value)}, analyze:false, sg);
        }

        public static SegmentCondition TC(double start, double start_tolerance, double end, double end_tolerance, ModalResultField column, Operator op, double expected_value, SegmentType sg = SegmentType.Distance)
        {
            return SegmentConditionFactoryMethod(start, start_tolerance, end, end_tolerance, 0, "", column, op, new double[] {Convert.ToDouble(expected_value)}, analyze:false, sg);
        }

        // string column
        public static SegmentCondition TC(double start, double end, string column, Operator op, double expected_value, SegmentType sg = SegmentType.Distance)
        {
            return SegmentConditionFactoryMethod(start, 0, end, 0, 0, column, ModalResultField.INVALID, op, new double[] {Convert.ToDouble(expected_value)}, analyze:false, sg);
        }

        public static SegmentCondition TC(double start, double end, double time_tolerance_seconds,string column, Operator op, double expected_value, SegmentType sg = SegmentType.Distance)
        {
            return SegmentConditionFactoryMethod(start, 0, end, 0, time_tolerance_seconds, column, ModalResultField.INVALID, op, new double[] {Convert.ToDouble(expected_value)}, analyze:false, sg);
        }

        public static SegmentCondition TC(double start, double start_tolerance, double end, double end_tolerance, string column, Operator op, double expected_value,SegmentType sg = SegmentType.Distance)
        {
            return SegmentConditionFactoryMethod(start, start_tolerance, end, end_tolerance, 0, column, ModalResultField.INVALID, op, new double[] {Convert.ToDouble(expected_value)}, analyze:false, sg);
        }

        // ===========================================================================================================
        // min/max expected value 

        // ModaResultField column
        public static SegmentCondition TC(double start, double end, ModalResultField column, Operator op, (double min, double max) expected_values, SegmentType sg = SegmentType.Distance)
        {
            return SegmentConditionFactoryMethod(start, 0, end, 0, 0, "", column, op, new[] {expected_values.min, expected_values.max}, analyze:false, sg);
        }

        public static SegmentCondition TC(double start, double end, double time_tolerance_seconds, ModalResultField column, Operator op, (double min, double max) expected_values, SegmentType sg = SegmentType.Distance)
        {
            return SegmentConditionFactoryMethod(start, 0, end, 0, time_tolerance_seconds, "", column, op, new[] {expected_values.min, expected_values.max}, analyze:false, sg);
        }

        public static SegmentCondition TC(double start, double start_tolerance, double end, double end_tolerance, ModalResultField column, Operator op, (double min, double max) expected_values, SegmentType sg = SegmentType.Distance)
        {
            return SegmentConditionFactoryMethod(start, start_tolerance, end, end_tolerance, 0, "", column, op, new[] {expected_values.min, expected_values.max}, analyze:false, sg);
        }

        // string column
        public static SegmentCondition TC(double start, double end, string column, Operator op, (double min, double max) expected_values, SegmentType sg = SegmentType.Distance)
        {
            return SegmentConditionFactoryMethod(start, 0, end, 0, 0, column, ModalResultField.INVALID, op, new[] {expected_values.min, expected_values.max}, analyze:false, sg);
        }

        public static SegmentCondition TC(double start, double end, double time_tolerance_seconds, string column, Operator op, (double min, double max) expected_values, SegmentType sg = SegmentType.Distance)
        {
            return SegmentConditionFactoryMethod(start, 0, end, 0, time_tolerance_seconds, column, ModalResultField.INVALID, op, new[] {expected_values.min, expected_values.max}, analyze:false, sg);
        }

        public static SegmentCondition TC(double start, double start_tolerance, double end, double end_tolerance, string column, Operator op, (double min, double max) expected_values, SegmentType sg = SegmentType.Distance)
        {
            return SegmentConditionFactoryMethod(start, start_tolerance, end, end_tolerance, 0, column, ModalResultField.INVALID, op, new[] {expected_values.min, expected_values.max}, analyze:false, sg);
        }

        // ===========================================================================================================
        // double value set expected value

        // ModaResultField column
        public static SegmentCondition TC(double start, double end, ModalResultField column, Operator op, double [] value_set, SegmentType sg = SegmentType.Distance)
        {
            return SegmentConditionFactoryMethod(start, 0, end, 0, 0, "", column, op, value_set, analyze:false, sg);
        }

        public static SegmentCondition TC(double start, double end, double time_tolerance_seconds, ModalResultField column, Operator op, double [] value_set, SegmentType sg = SegmentType.Distance)
        {
            return SegmentConditionFactoryMethod(start, 0, end, 0, time_tolerance_seconds, "", column, op, value_set, analyze:false, sg);
        }

        public static SegmentCondition TC(double start, double start_tolerance, double end, double end_tolerance, ModalResultField column, Operator op, double [] value_set, SegmentType sg = SegmentType.Distance)
        {
            return SegmentConditionFactoryMethod(start, start_tolerance, end, end_tolerance, 0, "", column, op, value_set, analyze:false, sg);
        }

        // string column
        public static SegmentCondition TC(double start, double end, string column, Operator op, double [] value_set, SegmentType sg = SegmentType.Distance)
        {
            return SegmentConditionFactoryMethod(start, 0, end, 0, 0, column, ModalResultField.INVALID, op, value_set, analyze:false, sg);
        }

        public static SegmentCondition TC(double start, double end, double time_tolerance_seconds, string column, Operator op, double [] value_set, SegmentType sg = SegmentType.Distance)
        {
            return SegmentConditionFactoryMethod(start, 0, end, 0, time_tolerance_seconds, column, ModalResultField.INVALID, op, value_set, analyze:false, sg);
        }

        public static SegmentCondition TC(double start, double start_tolerance, double end, double end_tolerance, string column, Operator op, double [] value_set, SegmentType sg = SegmentType.Distance)
        {
            return SegmentConditionFactoryMethod(start, start_tolerance, end, end_tolerance, 0, column, ModalResultField.INVALID, op, value_set, analyze:false, sg);
        }

        // ===========================================================================================================
        // int value set expected value

        // ModaResultField column
        public static SegmentCondition TC(double start, double end, ModalResultField column, Operator op, int [] value_set, SegmentType sg = SegmentType.Distance)
        {
            return SegmentConditionFactoryMethod(start, 0, end, 0, 0, "", column, op, Array.ConvertAll<int, double>(value_set, x => x), analyze:false, sg);
        }

        public static SegmentCondition TC(double start, double end, double time_tolerance_seconds, ModalResultField column, Operator op, int [] value_set, SegmentType sg = SegmentType.Distance)
        {
            return SegmentConditionFactoryMethod(start, 0, end, 0, time_tolerance_seconds, "", column, op, Array.ConvertAll<int, double>(value_set, x => x), analyze:false, sg);
        }

        public static SegmentCondition TC(double start, double start_tolerance, double end, double end_tolerance, ModalResultField column, Operator op, int [] value_set, SegmentType sg = SegmentType.Distance)
        {
            return SegmentConditionFactoryMethod(start, start_tolerance, end, end_tolerance, 0, "", column, op, Array.ConvertAll<int, double>(value_set, x => x), analyze:false, sg);
        }

        // string column
        public static SegmentCondition TC(double start, double end, string column, Operator op, int [] value_set, SegmentType sg = SegmentType.Distance)
        {
            return SegmentConditionFactoryMethod(start, 0, end, 0, 0, column, ModalResultField.INVALID, op, Array.ConvertAll<int, double>(value_set, x => x), analyze:false, sg);
        }

        public static SegmentCondition TC(double start, double end, double time_tolerance_seconds, string column, Operator op, int [] value_set, SegmentType sg = SegmentType.Distance)
        {
            return SegmentConditionFactoryMethod(start, 0, end, 0, time_tolerance_seconds, column, ModalResultField.INVALID, op, Array.ConvertAll<int, double>(value_set, x => x), analyze:false, sg);
        }

        public static SegmentCondition TC(double start, double start_tolerance, double end, double end_tolerance, string column, Operator op, int [] value_set, SegmentType sg = SegmentType.Distance)
        {
            return SegmentConditionFactoryMethod(start, start_tolerance, end, end_tolerance, 0, column, ModalResultField.INVALID, op, Array.ConvertAll<int, double>(value_set, x => x), analyze:false, sg);
        }

        // ===========================================================================================================
        // lambda expressions

        public static SegmentCondition TC(Func<DataRow, bool> firstExpression, Func<DataRow, bool> secondExpression)
        {
            return SegmentConditionFactoryMethod(firstExpression, secondExpression);
        }

        // ===========================================================================================================
        // Analyze TC

        // ModaResultField column
        public static SegmentCondition TC_Analyze(double start, double end, ModalResultField column, Operator op, SegmentType sg = SegmentType.Distance)
        {
            return SegmentConditionFactoryMethod(start, 0, end, 0, 0, "", column, op, new double[] {Convert.ToDouble(0)}, analyze:true, sg);
        }

        public static SegmentCondition TC_Analyze(double start, double end, double time_tolerance_seconds, ModalResultField column, Operator op, SegmentType sg = SegmentType.Distance)
        {
            return SegmentConditionFactoryMethod(start, 0, end, 0, time_tolerance_seconds, "", column, op, new double[] {Convert.ToDouble(0)}, analyze:true, sg);
        }

        public static SegmentCondition TC_Analyze(double start, double start_tolerance, double end, double end_tolerance, ModalResultField column, Operator op, SegmentType sg = SegmentType.Distance)
        {
            return SegmentConditionFactoryMethod(start, start_tolerance, end, end_tolerance, 0, "", column, op, new double[] {Convert.ToDouble(0)}, analyze:true, sg);
        }

        // string column
        public static SegmentCondition TC_Analyze(double start, double end, string column, Operator op, SegmentType sg = SegmentType.Distance)
        {
            return SegmentConditionFactoryMethod(start, 0, end, 0, 0, column, ModalResultField.INVALID, op, new double[] {Convert.ToDouble(0)}, analyze:true, sg);
        }

        public static SegmentCondition TC_Analyze(double start, double end, double time_tolerance_seconds, string column, Operator op, SegmentType sg = SegmentType.Distance)
        {
            return SegmentConditionFactoryMethod(start, 0, end, 0, time_tolerance_seconds, column, ModalResultField.INVALID, op, new double[] {Convert.ToDouble(0)}, analyze:true, sg);
        }

        public static SegmentCondition TC_Analyze(double start, double start_tolerance, double end, double end_tolerance, string column, Operator op, SegmentType sg = SegmentType.Distance)
        {
            return SegmentConditionFactoryMethod(start, start_tolerance, end, end_tolerance, 0, column, ModalResultField.INVALID, op, new double[] {Convert.ToDouble(0)}, analyze:true, sg);
        }

        // ===========================================================================================================
        // ============================================= FACTORY METHODS =============================================
        // ===========================================================================================================

        private static SegmentCondition SegmentConditionFactoryMethod(
            double start_val,
            double start_tolerance, 
            double end_val,
            double end_tolerance, 
            double time_tolerance,
            string property,
            ModalResultField property_field, 
            Operator op, 
            double [] value_set,
            bool analyze,
            SegmentType sg)
        {
            SegmentCondition sc;
            if (value_set.Length == 0)
            {
                value_set = new double[] {0};
            }
            switch(op)
            {
                case Operator.Lower:
                    sc = new LowerThanSegmentCondition(start_val, start_tolerance, end_val, end_tolerance, time_tolerance, property, property_field, value_set, analyze, segmentType:sg);
                    break;
                case Operator.Greater:
                    sc = new GreaterThanSegmentCondition(start_val, start_tolerance, end_val, end_tolerance, time_tolerance, property, property_field, value_set, analyze, segmentType:sg);
                    break;
                case Operator.Equals:
                    sc = new EqualsToSegmentCondition(start_val, start_tolerance, end_val, end_tolerance, time_tolerance, property, property_field, value_set, analyze, segmentType:sg);
                    break;
                case Operator.ValueSet:
                    sc = new ValueSetSegmentCondition(start_val, start_tolerance, end_val, end_tolerance, time_tolerance, property, property_field, value_set, analyze, segmentType:sg);
                    break;
                case Operator.MinMax:
                    if (value_set.Length < 2)
                    {
                        value_set = new double[] {0, 0};
                    }
                    sc = new MinMaxSegmentCondition(start_val, start_tolerance, end_val, end_tolerance, time_tolerance, property, property_field, value_set, analyze, segmentType:sg);
                    break;
                default:
                    sc = new NoOpSegmentCondition();
                    break;
            }

            return sc;
        }

        private static SegmentCondition SegmentConditionFactoryMethod(
            Func<DataRow, bool> firstExpression,
            Func<DataRow, bool> secondExpression
        )
        {
            return new LamdaExpressionsSegmentCondition(firstExpression, secondExpression);
        }
    }
}
