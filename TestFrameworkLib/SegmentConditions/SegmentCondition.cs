using System;
using System.Collections.Generic;

namespace TestFramework
{
    public class SegmentCondition
    {
        public double Start { get; set; }
        public double Start_Tolerance { get; set; }
        public double End { get; set; }
        public double End_Tolerance { get; set; }
        public List<DataRow> Data { get; set; }
        public SegmentType Type { get; set; }
        protected IOperator Operator { get; set; }
        public string Property { get; set; }
        protected double[] Expected { get; set; }
        public bool ToAnalyze { get; }
        protected List<SegmentCondition> TrueConditions;
        protected double FailPoint { get; set; }

        public bool Passed { get; set; }

        protected SegmentCondition(double start, double start_tolerance, double end, double end_tolerance, string property, double[] expected, bool analyze=false, SegmentType segmentType = SegmentType.Distance)
        {
            Start = start;
            Start_Tolerance = start_tolerance;
            End = end;
            End_Tolerance = end_tolerance;
            
            if (start > end) {
                throw new ArgumentException($"start delimiter ({start}) greater than end delimiter ({end})");
            }
            Data = new List<DataRow>();
            Type = segmentType;
            Property = property;
            Expected = expected;
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
                    Operator.Apply(dataLine[Property], Expected, GenerateFailMessage(dataLine[Property]));
                } catch (Exception) {
                    Passed = false;
                    Analyze();
                    break;
                }
            }
        }

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
                TrueConditions.Add(new EqualsToSegmentCondition(Start, Start_Tolerance, End, End_Tolerance, Property, new[] {oldValue}, analyze:false, Type));
            }
            else
            {
                if(Operator is MinMaxOperator)
                {
                    TrueConditions.Add(new MinMaxSegmentCondition(Start, Start_Tolerance, End, End_Tolerance, Property, new[] {minValue, maxValue}, analyze:false, Type));
                }  
                else
                {
                    TrueConditions.Add(new GreaterThanSegmentCondition(Start, Start_Tolerance, End, End_Tolerance, Property, new[] {minValue}, analyze:false, Type));
                    TrueConditions.Add(new LowerThanSegmentCondition(Start, Start_Tolerance, End, End_Tolerance, Property, new[] {maxValue}, analyze:false, Type));
                }          
            }
        }

        public virtual void Print(string prefix="", string suffix="")
        {
            Console.WriteLine(prefix + ToString() + suffix);
        }

        public virtual void PrintCorrectConditions(string prefix="", string suffix="")
        {
            if(Passed && !ToAnalyze)
            {
                Print(prefix, suffix);
            }
            else
            {
                foreach(var segmentCondition in TrueConditions)
                {
                    Console.WriteLine(prefix + segmentCondition.ToString() + suffix);
                }
            }
        } 

        public virtual void PrintResults(string prefix="\t*", string suffix="")
        {
            
            if(!ToAnalyze)
            {
                Console.WriteLine("{0} -> {1}", ToString(), Passed ? "✔ Pass" : "✗ Fail");
                if(!Passed)
                {
                    PrintCorrectConditions(prefix, suffix);
                }
            }
            else
            {
                Print(suffix: " analysis: ");
                PrintCorrectConditions(prefix);
            }
            Console.WriteLine();
            
        }

        public override string ToString()
        {
            var operator_string = ToAnalyze ? "Analyze_" + Operator.ToString() : Operator.ToString();
            if(Start_Tolerance == 0 && End_Tolerance == 0)
            {
                return $"({Start}, {End}, {ModFileData.GetModFileHeaderVariableNameByValue(Property)}, Operator.{operator_string}, {Expected[0]})";   
            }

            return $"({Start}, {Start_Tolerance}, {End}, {End_Tolerance}, {ModFileData.GetModFileHeaderVariableNameByValue(Property)}, Operator.{operator_string}, {Expected[0]})";
        }

        public string TypeColumnName()
        {
            return Type == SegmentType.Distance ? ModFileHeader.dist : ModFileHeader.dt;
        }

        public string TypeMeasuringUnit()
        {
            return Type == SegmentType.Distance ? "m" : "s";
        }

        public virtual string GenerateFailMessage(double lhs) =>
            $"Fail ({FailPoint}{TypeMeasuringUnit()}): Expected '{Property}' {Operator.Symbol()} {Expected}, Got: {lhs}";

        public virtual string GeneratePassMessage(double lhs) =>
            $"Pass: Expected '{Property}' {Operator.Symbol()} {Expected}. Got: {lhs}";
    }
}
