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
        public bool Analyze { get; }
        protected List<SegmentCondition> TrueConditions;

        public bool Passed { get; set; }

        protected SegmentCondition(TestSegment testSegment, string property, double value, bool analyze=false)
        {
            Segment = testSegment;
            Property = property;
            Value = value;
            Analyze = analyze;
            Passed = true;
            TrueConditions = new List<SegmentCondition>();
        }

        public virtual void check()
        {
            if (!Analyze)
            {
                foreach (var dataLine in Segment.Data) {
                    try {
                        Operator.Apply(dataLine[Property], Value, GenerateFailMessage(dataLine[Property]));
                    } catch (Exception) {
                        Passed = false;
                        AnalyzeCondition();
                        // PrintTrueConditions();
                        break;
                    }
                }
            }
            else
            {
                Passed = false;
                AnalyzeCondition();
                // PrintTrueConditions();
            }
        }

        public virtual void print() => Console.Write("Segment Condition: {0}\n", ToString());

        public virtual void AnalyzeCondition()
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
            if(!Analyze)
            {
                Console.Write("Condition `{0}` failed. Correct conditions:\n", this.ToString());
            }else
            {
                Console.Write("Analysis results for condition: `{0}`:\n", this.ToString());
            }
            
            foreach(var segmentCondition in TrueConditions)
            {
                Console.WriteLine("\t{0},", segmentCondition.ToString());
            }
        } 

        public override string ToString() => $"[{Segment}, {Property}, {Operator}, {Value}]";

        public virtual string GenerateFailMessage(double lhs) =>
            $"Fail: Expected '{Property}' {Operator.Symbol()} {Value}. Got: {lhs}";

        public virtual string GeneratePassMessage(double lhs) =>
            $"Pass: Expected '{Property}' {Operator.Symbol()} {Value}. Got: {lhs}";
    }
}
