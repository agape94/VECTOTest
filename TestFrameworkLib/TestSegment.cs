using System;
using System.Collections.Generic;
namespace TestFramework
{
    public class TestSegment
    {
        public double Start {get; set;}
        public double End {get; set;}
        public SegmentType Type {get; set;}
        public List<DataRow> Data {get; set;}

        public TestSegment(double start, double end, List<DataRow> testData, SegmentType dt = SegmentType.Distance)
        {
            if (start > end)
            {
                throw new System.ArgumentException(string.Format("start delimeter ({0}) greater than end delimeter ({1})", start, end));
            }
            this.Start = start;
            this.End = end;
            this.Type = dt;
            this.Data = testData;
        }
        public void print()
        {
            Console.Write("{0}", this.ToString());
        }

        public override string ToString() 
        {
            return string.Format("({0}, {1}, {2})", this.Start, this.End, this.Type);
        }
    }
}
