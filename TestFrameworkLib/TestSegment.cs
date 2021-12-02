using System;
using System.Collections.Generic;
namespace TestFramework
{
    public class TestSegment
    {
        public double Start { get; set; }
        public double End { get; set; }
        public SegmentType Type { get; set; }
        public List<DataRow> Data { get; set; }

        public TestSegment(double start, double end, List<DataRow> testData, SegmentType dt = SegmentType.Distance)
        {
            if (start > end) {
                throw new ArgumentException($"start delimiter ({start}) greater than end delimiter ({end})");
            }
            Start = start;
            End = end;
            Type = dt;
            Data = testData;
        }

        public void print() => Console.Write("{0}", ToString());

        public override string ToString() => $"{Start}, {End}";

        public string TypeMeasuringUnit()
        {
            return Type == SegmentType.Distance ? "m" : "s";
        }

        public string TypePropertyName()
        {
            return Type == SegmentType.Distance ? ModFileHeader.dist : ModFileHeader.dt;
        }
    }
}
