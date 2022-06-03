using System;
using System.Data;
using System.Linq;
using System.Collections.Generic;
using TUGraz.VectoCore.Models.Simulation.Data;
using TUGraz.VectoCore.Utils;
using TUGraz.VectoCommon.Utils;
using NUnit.Framework;

namespace TUGraz.VectoCore.Tests.TestFramework
{
    public class SegmentCondition
    {
        public double Start { get; set; }
        public double Start_Tolerance { get; set; }
        protected double Start_Adjusted { get; set; }
        protected string Start_String { get; set;}

        public double End { get; set; }
        public double End_Tolerance { get; set; }
        protected double End_Adjusted { get; set; }
        protected string End_String { get; set;}

        public double Time_Tolerance { get; set; }

        public EnumerableRowCollection<DataRow> Data { get; set; }

        public SegmentType Type { get; set; }
        protected IOperator Operator { get; set; }
        protected string Operator_To_Print { get; set; }
        public string Column_String { get; set; }
        public ModalResultField Column_Field { get; set; }
        protected string Column_To_Print{ get; set; }

        protected double[] Expected { get; set; }

        public bool ToAnalyze { get; set;}
        protected List<SegmentCondition> TrueConditions;
        protected double FailPoint { get; set; }
        protected bool DonePreCheck { get; set; }

        public bool Passed { get; set; }
        protected string FormattingDoubles { get; set;}

        protected SegmentCondition() 
        {
            Start = double.NegativeInfinity;
            End = double.PositiveInfinity;
            Start_Adjusted = Start;
            End_Adjusted = End;
            Passed = true;
        }

        protected SegmentCondition( double start, 
                                    double start_tolerance, 
                                    double end, 
                                    double end_tolerance, 
                                    double time_tolerance, 
                                    string column_string, 
                                    ModalResultField column_field, 
                                    IOperator op, 
                                    double[] expected, 
                                    bool analyze, 
                                    SegmentType segmentType)
        {
            Start = start;
            Start_Tolerance = start_tolerance;
            Start_Adjusted = Start;
            if(double.IsInfinity(Start))
            {
                Start_String = Start == double.NegativeInfinity? "double.NegativeInfinity" : "double.PositiveInfinity";
            }
            else
            {
                Start_String = Start.ToString();
            }

            End = end;
            End_Tolerance = end_tolerance;
            End_Adjusted = End;
            if(double.IsInfinity(End))
            {
                End_String = End == double.NegativeInfinity? "double.NegativeInfinity" : "double.PositiveInfinity";
            }
            else
            {
                End_String = End.ToString();
            }

            Time_Tolerance = time_tolerance;
            Expected = expected;
            ToAnalyze = analyze;

            Operator = op;
            Operator_To_Print = ToAnalyze ? Operator.ToString() + "_Analyze" : Operator.ToString();

            Passed = true;
            TrueConditions = new List<SegmentCondition>();
            FailPoint = double.MinValue;
            Type = segmentType;

            if (start > end) {
                throw new ArgumentException($"start delimiter ({start}) greater than end delimiter ({end})");
            }
            
            Column_Field = column_field;

            if(Column_Field != ModalResultField.INVALID)
            {
                Column_String = Column_Field.GetName();
            }
            else
            {
                Column_String = column_string;
            }
            Column_To_Print = Column_Field != ModalResultField.INVALID ? $"{typeof(ModalResultField).Name}.{Column_Field.ToString()}" : $"\"{Column_String}\"";
            DonePreCheck = false;
            FormattingDoubles = "0.00";
        }

        protected virtual void PreCheck(ref ModalResults data)
        {
            if(DonePreCheck)
            {
                return;
            }

            Assert.That(Start_Adjusted, Is.LessThanOrEqualTo(End_Adjusted));

            ComputeAndApplyTolerances(ref data);

            if(Column_String == "")
            {
                throw new Exception($"Please provide a valid column name to test.");
            }

            Data = (from row in data.AsEnumerable()
                                where (row.Field<SI>(TypeColumnName()).Value() >= Start_Adjusted && row.Field<SI>(TypeColumnName()).Value() <= End_Adjusted)
                                select row);

            DonePreCheck = true;
        }

        public virtual void Check(ref ModalResults data)
        {
            PreCheck(ref data);
            
            foreach (var dataLine in Data) {
                try {
                    FailPoint = dataLine.Field<SI>(TypeColumnName()).Value();
                    if(!ToAnalyze)
                    {
                        var lhs = GetValueToCheck(dataLine);
                        Operator.Apply(lhs, Expected, GenerateFailMessage(lhs));
                    }
                    else
                    {
                        Assert.That(false);
                    }
                }catch (AssertionException) 
                {
                    Passed = false;
                    Analyze(ref data);
                    break;
                }
            }
        }

        public virtual void Analyze(ref ModalResults data)
        {
            TrueConditions.Clear();

            PreCheck(ref data);

            bool areAllEqual = true;
            bool first = true;
            double maxValue = double.MinValue;
            double minValue = double.MaxValue;
            double oldValue = 0;

            if(Operator is not EqualsOperator)
            {
                foreach (var dataLine in Data)
                {
                    double actualValue = GetValueToCheck(dataLine);
                    if(first)
                    {
                        oldValue = actualValue;
                        first = false;
                    }
                    if(oldValue != actualValue && areAllEqual)
                    {
                        areAllEqual = false;
                    }

                    if(actualValue > maxValue)
                    {
                        maxValue = actualValue;
                    }
                    
                    if(actualValue < minValue)
                    {
                        minValue = actualValue;
                    }
                    oldValue = actualValue;
                }

                if(areAllEqual)
                {
                    TrueConditions.Add(new EqualsToSegmentCondition(Start, Start_Tolerance, End, End_Tolerance, Time_Tolerance, Column_String, Column_Field, new[] {oldValue}, analyze:false, Type));
                }
                else
                {
                    TrueConditions.Add(new MinMaxSegmentCondition(Start, Start_Tolerance, End, End_Tolerance, Time_Tolerance, Column_String, Column_Field, new[] {minValue, maxValue}, analyze:false, Type));
                }
            }
            else
            {
                double sectionStart = double.PositiveInfinity;
                double sectionEnd = double.NegativeInfinity;

                foreach (var dataLine in Data)
                {
                    double actualValue = GetValueToCheck(dataLine);
                    if(first)
                    {
                        oldValue = actualValue;
                        first = false;
                        sectionStart = dataLine.Field<SI>(TypeColumnName()).Value();
                        sectionEnd = dataLine.Field<SI>(TypeColumnName()).Value();
                        continue;
                    }

                    if(actualValue == oldValue)
                    {
                        sectionEnd = dataLine.Field<SI>(TypeColumnName()).Value();
                    }
                    else
                    {
                        sectionEnd = dataLine.Field<SI>(TypeColumnName()).Value();
                        TrueConditions.Add(new EqualsToSegmentCondition(Math.Round(sectionStart, 2), 0, Math.Round(sectionEnd, 2), 0, 0, Column_String, Column_Field, new[] {oldValue}, analyze:false, Type));
                        sectionStart = dataLine.Field<SI>(TypeColumnName()).Value();
                    }
                    oldValue = actualValue;
                }
            }
        }

        protected virtual double GetValueToCheck(DataRow dr)
        {
            double valueToCheck;
            var value_data_type = dr.Table.Columns[Column_String].DataType;
            if(value_data_type == typeof(SI))
            {
                valueToCheck = dr.Field<SI>(Column_String).Value();
            }
            else
            {
                valueToCheck = Convert.ToDouble(dr[Column_String]);
            }
            return valueToCheck;
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
            
        }

        public override string ToString()
        {
            var segment_type_string = Type == SegmentType.Distance ? "" : ", SegmentType.Time";
            if(Time_Tolerance != 0)
            {
                return $"({Start_String}, {End_String}, {Time_Tolerance}, {Column_To_Print}, Operator.{Operator_To_Print}, {Expected[0]:0.00}{segment_type_string})";
            }
            else if (Start_Tolerance == 0 && End_Tolerance == 0)
            {
                return $"({Start_String}, {End_String}, {Column_To_Print}, Operator.{Operator_To_Print}, {Expected[0]:0.00}{segment_type_string})";
            }
            return $"({Start_String}, {Start_Tolerance:0.00}, {End_String}, {End_Tolerance:0.00}, {Column_To_Print}, Operator.{Operator_To_Print}, {Expected[0]:0.00}{segment_type_string})";
        }

        public string TypeColumnName()
        {
            return Type == SegmentType.Distance ? ModalResultField.dist.GetName() : ModalResultField.time.GetName();
        }

        public string TypeMeasuringUnit()
        {
            return Type == SegmentType.Distance ? "m" : "s";
        }

        public virtual string GenerateFailMessage(double lhs) =>
            $"Fail ({FailPoint}{TypeMeasuringUnit()}): Expected '{Column_String}' {Operator.Symbol()} {Expected[0]}, Got: {lhs}";

        public virtual string GeneratePassMessage(double lhs) =>
            $"Pass: Expected '{Column_String}' {Operator.Symbol()} {Expected[0]}. Got: {lhs}";

        protected void ComputeAndApplyTolerances(ref ModalResults data)
        {
            if(Time_Tolerance != 0)
            {
                DataRow StartRow = (from row in data
                                where (row.Field<SI>(TypeColumnName()).Value() >= Start_Adjusted)
                                select row).FirstOrDefault();

                DataRow EndRow = (from row in data
                                where (row.Field<SI>(TypeColumnName()).Value() <= End_Adjusted)
                                select row).LastOrDefault();
                
                Assert.NotNull(StartRow);
                Assert.NotNull(EndRow);

                var StartSpeedMeterPerSecond = StartRow.Field<SI>(ModalResultField.v_act.GetName()).Value();
                var EndSpeedMeterPerSecond = EndRow.Field<SI>(ModalResultField.v_act.GetName()).Value();

                Start_Tolerance = Time_Tolerance * StartSpeedMeterPerSecond;
                End_Tolerance = Time_Tolerance * EndSpeedMeterPerSecond;
            }

            if(Start_Adjusted + Start_Tolerance >= End_Adjusted - End_Tolerance)
            {
                Start_Tolerance = 0;
                End_Tolerance = 0;
            }

            Start_Adjusted += Start_Tolerance;
            End_Adjusted -= End_Tolerance;
        }
    
    }
}
