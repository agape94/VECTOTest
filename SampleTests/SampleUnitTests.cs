using NUnit.Framework;
using TestFramework;

namespace SampleUnitTests
{
    [TestFixture]
    class UnitTests : VECTOTestFixture
    {
        [Test]
        public void CityBus_AT_PS_RegionalDelivery_Cycle5() => VECTOTestCase(@"mod_files/CityBus_AT_PS_RegionalDelivery.vmod",
            (0, 1e6, ModFileHeader.v_act, Operator.Greater, 100), 
            (0, 1e6, ModFileHeader.v_act, Operator.Lower, 500),
            (0, 70, ModFileHeader.Gear, Operator.Lower, 4) // (0, 70, "gear", Operator.Greater, 4)
            //((0, /* tolerance*/5), (70, /* tolerance*/15), ModFileHeader.P_eng_full_stat, Operator.Lower, 300) // (0, 70, "P_eng_full_stat", Operator.Equals, 250) -> if it's always the same value
                                                             // (0, 70, "P_eng_full_stat", Operator.Greater, min) -> if there are multiple values
                                                             // (0, 70, "P_eng_full_stat", Operator.Lower, max) 
            
        );

        [Test]
        public void CityBus_AT_PS_RegionalDelivery_Cycle6() => VECTOTestCase(@"mod_files/CityBus_AT_PS_RegionalDelivery.vmod",
            (0, 1e6, ModFileHeader.v_act, Operator.Greater, 100)
            // (50, 80, (ModFileHeader.v_act, ModFileHeader.Gear ...), Operator.Analyze) -> 
            // Ouput should be :
            //     (50, 60, ModFileHeader.v_act, Operator.Greater, 90),
            //     (0, 1e6, ModFileHeader.v_act, Operator.Lower, 100),
            //     (0, 70, ModFileHeader.Gear, Operator.Lower, 4),

            // (0, 2000, ModFileHeader.v_act, Operator.Analyze.MinMax) -> (0, 2000, ModFileHeader.v_act, Operator.Greater, 90),
            //                                                            (0, 2000, ModFileHeader.v_act, Operator.Lower, 100),

            // (0, 2000, ModFileHeader.v_act, Operator.Analyze.Lower, Greater, Equals)...


            // (0, 2000, ModFileHeader.Gear, Operator.Analyze.Equals) -> (0, 10, ModFileHeader.Gear, Operator.Equals, 2),
                                                                                // ...
            //                                                                  (10, 70, ModFileHeader.Gear, Operator.Equals, 5),

            // (0, 2000, ModFileHeader.Gear, Operator.Analyze.ValueSet) -> (0, 2000, ModFileHeader.Gear, Operator.ValueSet, [2,3,5,0]) -> this can also be an operator

            // add as a comment which rule generated which output 

            // wihses:
            //  * tolerances: argument to this analyze method to specify tolerances to generate the rules. 
            // (0, 2000, ModFileHeader.v_act, Operator.Analyze.MinMax, apply_tolerances(True/False), ToleranceValue (optional))
            // if the tolerance value is not provided, derive the tolerance from vehicle speed.
        );
    }
}
