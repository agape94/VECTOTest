using System;
using System.Collections.Generic;

namespace TestFramework
{
    public class SegmentCondition
    {
        // protected TestSegment Segment { get; set; }
        public double Start { get; set; }
        public double End { get; set; }
        public List<DataRow> Data { get; set; }
        public SegmentType Type { get; set; }
        protected IOperator Operator { get; set; }
        public string Property { get; set; }
        protected double Value { get; set; }
        public bool ToAnalyze { get; }
        protected List<SegmentCondition> TrueConditions;
        protected double FailPoint { get; set; }

        public bool Passed { get; set; }

        protected SegmentCondition(double start, double end, string property, double value, bool analyze=false, SegmentType segmentType = SegmentType.Distance)
        {
            // Segment = testSegment;
            Start = start;
            End = end;
            
            if (start > end) {
                throw new ArgumentException($"start delimiter ({start}) greater than end delimiter ({end})");
            }
            Data = new List<DataRow>();
            Type = segmentType;
            Property = property;
            Value = value;
            ToAnalyze = analyze;
            Passed = true;
            TrueConditions = new List<SegmentCondition>();
            FailPoint = double.MinValue;
        }

        public virtual void Check()
        {
            foreach (var dataLine in Data) {
                try {
                    FailPoint = dataLine[TypeColumnName()];
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
            double oldValue = Data[0][Property];

            foreach (var dataLine in Data)
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
                TrueConditions.Add(new EqualsToSegmentCondition(Start, End, Property, oldValue, analyze:false, Type));
            }
            else
            {
                TrueConditions.Add(new GreaterThanSegmentCondition(Start, End, Property, minValue, analyze:false, Type));
                TrueConditions.Add(new LowerThanSegmentCondition(Start, End, Property, maxValue, analyze:false, Type));            
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

        public override string ToString() => $"({Start}, {End}, {ModFileData.GetModFileHeaderVariableNameByValue(Property)}, Operator.{Operator}, {Value})";

        public string TypeColumnName()
        {
            return Type == SegmentType.Distance ? ModFileHeader.dist : ModFileHeader.dt;
        }

        public string TypeMeasuringUnit()
        {
            return Type == SegmentType.Distance ? "m" : "s";
        }

        public virtual string GenerateFailMessage(double lhs) =>
            $"Fail ({FailPoint}{TypeMeasuringUnit()}): Expected '{Property}' {Operator.Symbol()} {Value}, Got: {lhs}";

        public virtual string GeneratePassMessage(double lhs) =>
            $"Pass: Expected '{Property}' {Operator.Symbol()} {Value}. Got: {lhs}";
    }
}
