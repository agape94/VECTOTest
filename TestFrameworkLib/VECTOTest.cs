using NUnit.Framework;
using System.Collections.Generic;
using System.IO;
using System;
namespace TestFramework
{
    public class VECTOTest
    {
        private List<SegmentCondition> m_Conditions;
        private static ModFileData m_Data;
        private bool m_AllPassed;

        public VECTOTest() {}

        public void RunTestCases(string jobname, params SegmentCondition[] segmentConditions)
        {
            m_Data = new ModFileData();
            m_Conditions = new List<SegmentCondition>();
            m_AllPassed = true;
            Assert.True(m_Data.ParseCsv(jobname));
            foreach (var sc in segmentConditions) {
                try {
                    if(sc.Time_Tolerance != 0)
                    {
                        sc.Start_Tolerance = ComputeTolerance(sc.Start, sc.Time_Tolerance, sc.Type);
                        sc.End_Tolerance = ComputeTolerance(sc.End, sc.Time_Tolerance, sc.Type);
                    }
                    if(sc is LamdaExpressionsSegmentCondition)
                    {
                        sc.Data = m_Data.getWholeData();
                    }
                    else
                    {
                        sc.Data = m_Data.GetTestData(sc.Start + sc.Start_Tolerance, sc.End - sc.End_Tolerance, sc.Property, sc.Type);
                    }
                    m_Conditions.Add(sc);
                    sc.Check();
                    sc.PrintResults();
                    if(!sc.Passed && m_AllPassed)
                    {
                        m_AllPassed = false;
                    }
                } catch (Exception e) {
                    Console.Error.WriteLine(e.ToString());
                }
            }
            PrintResults();
        }

        public static double ComputeTolerance(double position, double toleranceSeconds, SegmentType st = SegmentType.Distance)
        {
            double tolerance_m = double.MinValue;
            double kphToMpsFactor = 0.277777778;

            DataRow dataRow = m_Data.getRowAt(position, st);
            double speedMps = dataRow[ModFileHeader.v_act] * kphToMpsFactor;
            tolerance_m = speedMps * toleranceSeconds;
            return tolerance_m;
        }

        private void PrintResults()
        {
            if(!m_AllPassed)
            {
                Console.WriteLine("✗ Some test cases failed. Corrected test cases: ");
            }
            else
            {
                Console.WriteLine("✔ All test cases passed!");
            }

            var prefix = "TC";
            var suffix = ",";

            foreach (var segmentCondition in m_Conditions) 
            {
                segmentCondition.PrintCorrectConditions(prefix, suffix);
            }
        }


        // Analyze

        public static SegmentCondition TC(
            double start, 
            double end, 
            string property, 
            Operator op, 
            SegmentType sg = SegmentType.Distance)
        {
            return SegmentConditionFactoryMethod(start, 0, end, 0, property, op, sg);
        }

        public static SegmentCondition TC(
            double start, 
            double start_tolerance,
            double end, 
            double end_tolerance,
            string property, 
            Operator op, 
            SegmentType sg = SegmentType.Distance)
        {
            return SegmentConditionFactoryMethod(start, 0, end, 0, property, op, sg);
        }

        public static SegmentCondition TC(
            double start,
            double end, 
            double time_tolerance,
            string property, 
            Operator op, 
            SegmentType sg = SegmentType.Distance)
        {
            return SegmentConditionFactoryMethod(start, 0, end, 0, property, op, sg);
        }

        // ===============================================================================================
        // single expected value

        public static SegmentCondition TC(
            double start, 
            double end, 
            string property, 
            Operator op, 
            double expected_value,
            SegmentType sg = SegmentType.Distance)
        {
            return SegmentConditionFactoryMethod(start, 0, end, 0, property, op, new [] {expected_value}, sg);
        }

        public static SegmentCondition TC(
            double start,
            double end, 
            double time_tolerance_seconds,
            string property, 
            Operator op, 
            double expected_value,
            SegmentType sg = SegmentType.Distance)
        {
            return SegmentConditionFactoryMethod(start, end, time_tolerance_seconds, property, op, new [] {expected_value}, sg);
        }

        public static SegmentCondition TC(
            double start,
            double start_tolerance,
            double end, 
            double end_tolerance,
            string property, 
            Operator op, 
            double expected_value,
            SegmentType sg = SegmentType.Distance)
        {
            return SegmentConditionFactoryMethod(start, start_tolerance, end, end_tolerance, property, op, new [] {expected_value}, sg);
        }

        // ===============================================================================================
        // min/max expected value 
        public static SegmentCondition TC(
            double start, 
            double end, 
            string property, 
            Operator op, 
            (double min, double max) expected_values,
            SegmentType sg = SegmentType.Distance)
        {
            return SegmentConditionFactoryMethod(start, 0, end, 0, property, op, new[] {expected_values.min, expected_values.max}, sg);
        }

        public static SegmentCondition TC(
            double start,
            double end,
            double time_tolerance_seconds,
            string property, 
            Operator op, 
            (double min, double max) expected_values,
            SegmentType sg = SegmentType.Distance)
        {
            return SegmentConditionFactoryMethod(start, end, time_tolerance_seconds, property, op, new[] {expected_values.min, expected_values.max}, sg);
        }

        public static SegmentCondition TC(
            double start,
            double start_tolerance,
            double end, 
            double end_tolerance,
            string property, 
            Operator op, 
            (double min, double max) expected_values,
            SegmentType sg = SegmentType.Distance)
        {
            return SegmentConditionFactoryMethod(start, start_tolerance, end, end_tolerance, property, op, new[] {expected_values.min, expected_values.max}, sg);
        }

        // ===============================================================================================
        // double value set expected value
        public static SegmentCondition TC(
            double start, 
            double end, 
            string property, 
            Operator op, 
            double [] value_set,
            SegmentType sg = SegmentType.Distance)
        {
            return SegmentConditionFactoryMethod(start, 0, end, 0, property, op, value_set, sg);
        }

        public static SegmentCondition TC(
            double start,
            double end,
            double time_tolerance_seconds,
            string property, 
            Operator op, 
            double [] value_set,
            SegmentType sg = SegmentType.Distance)
        {
            return SegmentConditionFactoryMethod(start, end, time_tolerance_seconds, property, op, value_set, sg);
        }

        public static SegmentCondition TC(
            double start,
            double start_tolerance,
            double end, 
            double end_tolerance,
            string property, 
            Operator op, 
            double [] value_set,
            SegmentType sg = SegmentType.Distance)
        {
            return SegmentConditionFactoryMethod(start, start_tolerance, end, end_tolerance, property, op, value_set, sg);
        }

        // ===============================================================================================
        // int value set expected value

        public static SegmentCondition TC(
            double start, 
            double end, 
            string property, 
            Operator op, 
            int [] value_set,
            SegmentType sg = SegmentType.Distance)
        {
            return SegmentConditionFactoryMethod(start, 0, end, 0, property, op, Array.ConvertAll<int, double>(value_set, x => x), sg);
        }

        public static SegmentCondition TC(
            double start,
            double end,
            double time_tolerance_seconds,
            string property, 
            Operator op, 
            int [] value_set,
            SegmentType sg = SegmentType.Distance)
        {
            return SegmentConditionFactoryMethod(start, end, time_tolerance_seconds, property, op, Array.ConvertAll<int, double>(value_set, x => x), sg);
        }

        public static SegmentCondition TC(
            double start,
            double start_tolerance,
            double end, 
            double end_tolerance,
            string property, 
            Operator op, 
            int [] value_set,
            SegmentType sg = SegmentType.Distance)
        {
            return SegmentConditionFactoryMethod(start, start_tolerance, end, end_tolerance, property, op, Array.ConvertAll<int, double>(value_set, x => x), sg);
        }

        public static SegmentCondition TC(
            Func<DataRow, bool> firstExpression,
            Func<DataRow, bool> secondExpression
        )
        {
            return SegmentConditionFactoryMethod(firstExpression, secondExpression);
        }

        // ===============================================================================================
        // factory methods

        private static SegmentCondition SegmentConditionFactoryMethod(
            double start_val,
            double start_tolerance, 
            double end_val,
            double end_tolerance, 
            string property, 
            Operator op,
            SegmentType sg)
        {
            SegmentCondition sc;
            switch(op)
            {
                case Operator.Analyze_Lower:
                    sc = new LowerThanSegmentCondition(start_val, start_tolerance, end_val, end_tolerance, property, new double[]{0}, analyze:true, segmentType:sg);
                    break;
                case Operator.Analyze_Greater:
                    sc = new GreaterThanSegmentCondition(start_val, start_tolerance, end_val, end_tolerance, property, new double[]{0}, analyze:true, segmentType:sg);
                    break;
                case Operator.Analyze_Equals:
                    sc = new EqualsToSegmentCondition(start_val, start_tolerance, end_val, end_tolerance, property, new double[]{0}, analyze:true, segmentType:sg);
                    break;
                case Operator.Analyze_MinMax:
                    sc = new MinMaxSegmentCondition(start_val, start_tolerance, end_val, end_tolerance, property, new double[] {0}, analyze: true, segmentType:sg);
                    break;
                case Operator.Analyze_ValueSet:
                    sc = new ValueSetSegmentCondition(start_val, start_tolerance, end_val, end_tolerance, property, new double[] {0}, analyze: true, segmentType:sg);
                    break;
                default:
                    sc = new NoOpSegmentCondition();
                    break;
            }
            return sc;
        }

        private static SegmentCondition SegmentConditionFactoryMethod(
            double start_val,
            double end_val,
            double time_tolerance, 
            string property, 
            Operator op,
            SegmentType sg)
        {
            SegmentCondition sc;
            switch(op)
            {
                case Operator.Analyze_Lower:
                    sc = new LowerThanSegmentCondition(start_val, end_val, time_tolerance, property, new double[]{0}, analyze:true, segmentType:sg);
                    break;
                case Operator.Analyze_Greater:
                    sc = new GreaterThanSegmentCondition(start_val, end_val, time_tolerance, property, new double[]{0}, analyze:true, segmentType:sg);
                    break;
                case Operator.Analyze_Equals:
                    sc = new EqualsToSegmentCondition(start_val, end_val, time_tolerance, property, new double[]{0}, analyze:true, segmentType:sg);
                    break;
                case Operator.Analyze_MinMax:
                    sc = new MinMaxSegmentCondition(start_val, end_val, time_tolerance, property, new double[] {0}, analyze: true, segmentType:sg);
                    break;
                case Operator.Analyze_ValueSet:
                    sc = new ValueSetSegmentCondition(start_val, end_val, time_tolerance, property, new double[] {0}, analyze: true, segmentType:sg);
                    break;
                default:
                    sc = new NoOpSegmentCondition();
                    break;
            }
            return sc;
        }

        private static SegmentCondition SegmentConditionFactoryMethod(
            double start_val,
            double start_tolerance, 
            double end_val,
            double end_tolerance, 
            string property, 
            Operator op, 
            double [] value_set,
            SegmentType sg)
        {
            SegmentCondition sc;
            switch(op)
            {
                case Operator.Lower:
                    sc = new LowerThanSegmentCondition(start_val, start_tolerance, end_val, end_tolerance, property, value_set, analyze: false, segmentType:sg);
                    break;
                case Operator.Greater:
                    sc = new GreaterThanSegmentCondition(start_val, start_tolerance, end_val, end_tolerance, property, value_set, analyze: false, segmentType:sg);
                    break;
                case Operator.Equals:
                    sc = new EqualsToSegmentCondition(start_val, start_tolerance, end_val, end_tolerance, property, value_set, analyze: false, segmentType:sg);
                    break;
                case Operator.ValueSet:
                    sc = new ValueSetSegmentCondition(start_val, start_tolerance, end_val, end_tolerance, property, value_set, analyze: false, segmentType:sg); /* TODO handle segment type on all cases */
                    break;
                case Operator.MinMax:
                    sc = new MinMaxSegmentCondition(start_val, start_tolerance, end_val, end_tolerance, property, value_set, analyze: false, segmentType:sg);
                    break;
                default:
                    sc = new NoOpSegmentCondition();
                    break;
            }

            return sc;
        }


        private static SegmentCondition SegmentConditionFactoryMethod(
            double start_val,
            double end_val,
            double time_tolerance_seconds, 
            string property, 
            Operator op, 
            double [] value_set,
            SegmentType sg)
        {
            SegmentCondition sc;
            switch (op) {
                case Operator.Lower:
                    sc = new LowerThanSegmentCondition(start_val, end_val, time_tolerance_seconds, property, value_set, analyze: false, segmentType:sg);
                    break;
                case Operator.Greater:
                    sc = new GreaterThanSegmentCondition(start_val, end_val, time_tolerance_seconds, property, value_set, analyze: false, segmentType:sg);
                    break;
                case Operator.Equals:
                    sc = new EqualsToSegmentCondition(start_val, end_val, time_tolerance_seconds, property, value_set, analyze: false, segmentType:sg);
                    break;
                case Operator.ValueSet:
                    sc = new ValueSetSegmentCondition(start_val, end_val, time_tolerance_seconds, property, value_set, analyze: false, segmentType:sg); /* TODO handle segment type on all cases */
                    break;
                case Operator.MinMax:
                    sc = new MinMaxSegmentCondition(start_val, end_val, time_tolerance_seconds, property, value_set, analyze: false, segmentType:sg);
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
