namespace TestFramework
{
    public class VECTOTestFixture
    {
        public void VECTOTestCase(string jobname, params (/*dynamic*/double start, /*dynamic*/double end, string property, Operator op, double value)[] expected)
        {
            var test_case = new VECTOTestCase(jobname, expected);
        }
    }
}
