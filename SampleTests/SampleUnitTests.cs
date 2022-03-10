using NUnit.Framework;
using TestFramework;

namespace SampleUnitTests
{
    [TestFixture]
    class UnitTests : VECTOTestCase
    {
        [Test]
        public void Test1() => RunTestCases(@"mod_files/CityBus_AT_PS_RegionalDelivery.vmod",
            
            // No tolerances, no analyisis
            TC(100, 2000, ModFileHeader.v_act, Operator.Equals, 150),
            TC(100, 2000, ModFileHeader.Gear, Operator.MinMax, (100, 250)),
            TC(100, 2000, ModFileHeader.v_act, Operator.MinMax, (100, 250)),
            TC(100, 2000, ModFileHeader.Gear, Operator.ValueSet, new[]{0, 1, 2, 3}),
            TC(200, 1000, ModFileHeader.Gear, Operator.ValueSet, new[]{1, 2, 3}),

            // ==================================================================================

            // No tolerances, doing analysis
            TC(100, 2000, ModFileHeader.v_act, Operator.Analyze_Equals, 150),
            TC(100, 2000, ModFileHeader.Gear, Operator.Analyze_MinMax, (100, 250)),
            TC(100, 2000, ModFileHeader.v_act, Operator.Analyze_MinMax, (100, 250)),
            TC(100, 2000, ModFileHeader.Gear, Operator.Analyze_ValueSet, new[]{0, 1, 2, 3}),
            TC(200, 1000, ModFileHeader.Gear, Operator.Analyze_ValueSet, new[]{1, 2, 3}),

            // ==================================================================================
            
            // With tolerances, no analysis
            TC(100, 50, 2000, 0, ModFileHeader.v_act, Operator.Equals, 150),
            TC(100, 50, 2000, 0, ModFileHeader.Gear, Operator.MinMax, (100, 250)),
            TC(100, 50, 2000, 0, ModFileHeader.v_act, Operator.MinMax, (100, 250)),
            TC(100, 50, 2000, 0, ModFileHeader.Gear, Operator.ValueSet, new[]{0, 1, 2, 3}),
            TC(200, 50, 1000, 0, ModFileHeader.Gear, Operator.ValueSet, new[]{1, 2, 3}),

            // ==================================================================================

            // With tolerances, doing analysis
            TC(100, 50, 2000, 0, ModFileHeader.v_act, Operator.Analyze_Equals, 150),
            TC(100, 50, 2000, 0, ModFileHeader.Gear, Operator.Analyze_MinMax, (100, 250)),
            TC(100, 50, 2000, 0, ModFileHeader.v_act, Operator.Analyze_MinMax, (100, 250)),
            TC(100, 50, 2000, 0, ModFileHeader.Gear, Operator.Analyze_ValueSet, new[]{0, 1, 2, 3}),
            TC(200, 50, 1000, 0, ModFileHeader.Gear, Operator.Analyze_ValueSet, new[]{1, 2, 3})
        );

        [Test]
        public void Test2() => RunTestCases(@"mod_files/CityBus_AT_PS_RegionalDelivery.vmod",
            // No tolerances, no analyisis
            TC(100, 2000, ModFileHeader.v_act, Operator.Equals, 150),
            TC(100, 2000, ModFileHeader.Gear, Operator.MinMax, (100, 250)),
            TC(100, 2000, ModFileHeader.v_act, Operator.MinMax, (100, 250)),
            TC(100, 2000, ModFileHeader.Gear, Operator.ValueSet, new[]{0, 1, 2, 3}),
            TC(200, 1000, ModFileHeader.Gear, Operator.ValueSet, new[]{1, 2, 3}),

            // ==================================================================================

            // No tolerances, doing analysis
            TC(100, 2000, ModFileHeader.v_act, Operator.Analyze_Equals, 150),
            TC(100, 2000, ModFileHeader.Gear, Operator.Analyze_MinMax, (100, 250)),
            TC(100, 2000, ModFileHeader.v_act, Operator.Analyze_MinMax, (100, 250)),
            TC(100, 2000, ModFileHeader.Gear, Operator.Analyze_ValueSet, new[]{0, 1, 2, 3}),
            TC(200, 1000, ModFileHeader.Gear, Operator.Analyze_ValueSet, new[]{1, 2, 3}),

            // ==================================================================================
            
            // With tolerances, no analysis
            TC(100, 50, 2000, 0, ModFileHeader.v_act, Operator.Equals, 150),
            TC(100, 50, 2000, 0, ModFileHeader.Gear, Operator.MinMax, (100, 250)),
            TC(100, 50, 2000, 0, ModFileHeader.v_act, Operator.MinMax, (100, 250)),
            TC(100, 50, 2000, 0, ModFileHeader.Gear, Operator.ValueSet, new[]{0, 1, 2, 3}),
            TC(200, 50, 1000, 0, ModFileHeader.Gear, Operator.ValueSet, new[]{1, 2, 3}),

            // ==================================================================================

            // With tolerances, doing analysis
            TC(100, 50, 2000, 0, ModFileHeader.v_act, Operator.Analyze_Equals, 150),
            TC(100, 50, 2000, 0, ModFileHeader.Gear, Operator.Analyze_MinMax, (100, 250)),
            TC(100, 50, 2000, 0, ModFileHeader.v_act, Operator.Analyze_MinMax, (100, 250)),
            TC(100, 50, 2000, 0, ModFileHeader.Gear, Operator.Analyze_ValueSet, new[]{0, 1, 2, 3}),
            TC(200, 50, 1000, 0, ModFileHeader.Gear, Operator.Analyze_ValueSet, new[]{1, 2, 3})

            /* wihses:
                * tolerances: argument to this analyze method to specify tolerances to generate the rules. 
                (0, 2000, ModFileHeader.v_act, Operator.Analyze.MinMax, apply_tolerances(True/False), ToleranceValue (optional))
                if the tolerance value is not provided, derive the tolerance from vehicle speed. 
                
                new TC(condition(Engine on/off, ... any boolean from the modfile), )
                
            <---- TO BE DISCUSSED ----> 
            */
        );
    }
}
