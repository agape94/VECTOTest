using System;
using System.Collections.Generic;

namespace TestFramework
{
    public class SegmentCondition
    {
        protected TestSegment Segment { get; set; }
        protected IOperator Operator { get; set; }
        protected string Property { get; set; }
        protected double Value { get; set; }
        public bool ToAnalyze { get; }
        protected List<SegmentCondition> TrueConditions;
        protected double FailPoint { get; set; }

        public bool Passed { get; set; }

        protected SegmentCondition(TestSegment testSegment, string property, double value, bool analyze=false)
        {
            Segment = testSegment;
            Property = property;
            Value = value;
            ToAnalyze = analyze;
            Passed = true;
            TrueConditions = new List<SegmentCondition>();
            FailPoint = double.MinValue;
        }

        public virtual void Check()
        {
            foreach (var dataLine in Segment.Data) {
                try {
                    FailPoint = dataLine[Segment.TypeColumnName()];
                    Operator.Apply(dataLine[Property], Value, GenerateFailMessage(dataLine[Property]));
                } catch (Exception) {
                    Passed = false;
                    Analyze();
                    break;
                }
            }
        }

        public virtual void Print() => Console.Write(ToString());

        public virtual void Analyze()
        {
            TrueConditions.Clear();

            bool areAllEqual = true;
            double maxValue = double.MinValue;
            double minValue = double.MaxValue;
            double oldValue = Segment.Data[0][Property];

            foreach (var dataLine in Segment.Data)
            {
                double actualValue = dataLine[Property];
                if(oldValue != actualValue && areAllEqual)
                {
                    areAllEqual = false;
                }

                if(actualValue > maxValue)
                {
                    maxValue = actualValue;
                }
                else if(actualValue < minValue)
                {
                    minValue = actualValue;
                }
                oldValue = actualValue;
            }

            if(areAllEqual)
            {
                TrueConditions.Add(new EqualsToSegmentCondition(Segment, Property, oldValue));
            }
            else
            {
                TrueConditions.Add(new GreaterThanSegmentCondition(Segment, Property, minValue));
                TrueConditions.Add(new LowerThanSegmentCondition(Segment, Property, maxValue));            
            }
        }

        public virtual void PrintTrueConditions()
        {
            if(!ToAnalyze)
            {
                Console.Write("Condition `{0}` failed. Correct conditions:\n", ToString());
            }else
            {
                Console.Write("Analysis results for condition: `{0}`:\n", ToString());
            }
            
            foreach(var segmentCondition in TrueConditions)
            {
                Console.WriteLine("\t{0},", segmentCondition.ToString());
            }
        } 

        public virtual void PrintResults()
        {
            if(!ToAnalyze)
            {
                Console.WriteLine("{0} -> {1}", ToString(), Passed ? "✔ Pass" : "✗ Fail");
                
            }
            if(!Passed || ToAnalyze)
            {
                PrintTrueConditions();
            }
        }

        public override string ToString() => $"({Segment}, {ModFileData.GetModFileHeaderVariableNameByValue(Property)}, Operator.{Operator}, {Value})";

        public virtual string GenerateFailMessage(double lhs) =>
            $"Fail ({FailPoint}{Segment.TypeMeasuringUnit()}): Expected '{Property}' {Operator.Symbol()} {Value}, Got: {lhs}";

        public virtual string GeneratePassMessage(double lhs) =>
            $"Pass: Expected '{Property}' {Operator.Symbol()} {Value}. Got: {lhs}";
    }
}
