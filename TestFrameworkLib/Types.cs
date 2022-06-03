using System.Collections.Generic;

namespace TUGraz.VectoCore.Tests.TestFramework
{
    // public class DataRow : Dictionary<string, double> {}

    public enum Operator
    {
        Lower,
        Greater,
        Equals,
        MinMax,
        ValueSet
    }

    public enum SegmentType
    {
        Distance,
        Time
    }
}
