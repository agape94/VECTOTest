using CsvHelper;
using CsvHelper.Configuration.Attributes;
using NUnit.Framework;
using System.Collections.Generic;
using System.IO;
namespace TestFramework
{
    public class TestCase
    {
        List<SegmentCondition> m_Conditions;
        public TestCase(string jobname, params (double start, double end, string property, Operator op, double value)[] expected)
        {
            // Console.WriteLine("Running test case");

            // TODO extract information from expected and create TestSegments with SegmentConditions

            Assert.True(read_Csv(jobname));
        }

        public bool check()
        {
            // TODO go through all conditions and apply them
            return true;
        }

        private bool read_Csv(string path)
        {
            using (var reader = new StreamReader(path))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                var records = csv.GetRecords<dynamic>();
                Console.WriteLine(records);
            }
            return true;
        }
    }
}