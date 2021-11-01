using System;
namespace TestFramework
{
    public class SegmentCondition
    {
        public TestSegment Segment {get; set;}
        public Operator Operator {get; set;}
        public string Property {get; set;}
        public double Value {get; set;}
        public bool Passed {get; set;}

        public SegmentCondition(TestSegment testSegment, string property, Operator op, double value)
        {
            this.Segment = testSegment;
            this.Operator = op;
            this.Property = property;
            this.Value = value;
            this.Passed = false;
        }

        public void check()
        {
            foreach(var dataLine in this.Segment.Data)
            {
                try
                {
                    Utils.ApplyOperator(dataLine[this.Property], this.Operator, this.Value, this.GenerateFailMessage(dataLine[this.Property]));
                }
                catch(Exception e)
                {
                    _ = e;
                    this.Passed = false;
                    return;
                }
            }
            this.Passed = true;
        }

        public void print()
        {
            Console.Write("Segment Condition: {0}\n", this.ToString());
        }

        public override string ToString()
        {
            return string.Format("[{0}, {1}, {2}, {3}]", this.Segment.ToString(), this.Property, this.Operator, this.Value);
        }

        private string GenerateFailMessage(double lhs)
        {
            return string.Format("Fail: Expected '{0}' {1} {2}. Got: {3}", this.Property, Utils.Symbol(this.Operator), this.Value, lhs);
        }

        private string GeneratePassMessage(double lhs)
        {
            return string.Format("Pass: Expected '{0}' {1} {2}. Got: {3}", this.Property, Utils.Symbol(this.Operator), this.Value, lhs);
        }
    }
}
