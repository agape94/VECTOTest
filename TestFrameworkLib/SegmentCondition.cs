using System;
namespace TestFramework
{
    public class SegmentCondition
    {
        public TestSegment Segment { get; set; }
        public Operator Operator { get; set; }
        public string Property { get; set; }
        public double Value { get; set; }
        public bool Passed { get; set; }

        public SegmentCondition(TestSegment testSegment, string property, Operator op, double value)
        {
            Segment = testSegment;
            Operator = op;
            Property = property;
            Value = value;
            Passed = false;
        }

        public void check()
        {
            foreach (var dataLine in Segment.Data) {
                try {
                    Utils.ApplyOperator(dataLine[Property], Operator, Value, GenerateFailMessage(dataLine[Property]));
                } catch (Exception) {
                    Passed = false;

                    Console.Write("Condition `{0}` failed. Correct conditions:\n", this.ToString());

                    foreach(var segmentCondition in Utils.GetCorrectTestSegments(this))
                    {
                        Console.WriteLine("\t{0},", segmentCondition.ToString());
                    }

                    // TODO
                    // In case of failure, output the actual(found or true) conditions
                    // Ex: (0, 70, v_act, Lower/Greater/Equals, 50) fails => 
                    // => the framework should output:
                    // * In case v_act is always the same value in that interval:
                    //      (0, 70, v_act, Equals, 70)
                    // * In case there are multiple values, output the min/max values:
                    //      (0, 70, v_act, Greater, 21)
                    //      (0, 70, v_act, Lower, 120)

                    return;
                }
            }
            Passed = true;
        }

        public void print() => Console.Write("Segment Condition: {0}\n", ToString());

        public override string ToString() => $"[{Segment}, {Property}, {Operator}, {Value}]";

        private string GenerateFailMessage(double lhs) =>
            $"Fail: Expected '{Property}' {Utils.Symbol(Operator)} {Value}. Got: {lhs}";

        private string GeneratePassMessage(double lhs) =>
            $"Pass: Expected '{Property}' {Utils.Symbol(Operator)} {Value}. Got: {lhs}";
    }
}
