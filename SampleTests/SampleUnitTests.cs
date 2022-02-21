using NUnit.Framework;
using TestFramework;

namespace SampleUnitTests
{
    [TestFixture]
    class UnitTests
    {
        // [Test]
        // public void CityBus_AT_PS_RegionalDelivery_Cycle5() => VECTOTestCase(@"mod_files/CityBus_AT_PS_RegionalDelivery.vmod",
        //     (1000, 1e6, ModFileHeader.v_act, Operator.Greater, 50), 
        //     (0, 1e6, ModFileHeader.v_act, Operator.Lower, 500),
        //     (0, 70, ModFileHeader.Gear, Operator.Lower, 4) // (0, 70, "gear", Operator.Greater, 4)
        //     //((0, /* tolerance*/5), (70, /* tolerance*/15), ModFileHeader.P_eng_full_stat, Operator.Lower, 300) // (0, 70, "P_eng_full_stat", Operator.Equals, 250) -> if it's always the same value
        //                                                      // (0, 70, "P_eng_full_stat", Operator.Greater, min) -> if there are multiple values
        //                                                      // (0, 70, "P_eng_full_stat", Operator.Lower, max) 
            
        // );

        [Test]
        public void CityBus_AT_PS_RegionalDelivery_Cycle6() => new VECTOTestCase(@"mod_files/CityBus_AT_PS_RegionalDelivery.vmod",
            // Utils.TC(0, 100, ModFileHeader.v_act, Operator.Lower, 100),
            // Utils.TC(0, 10, 1000, 0, ModFileHeader.Gear, Operator.Greater, 3),
            Utils.TC(100, 50, 2000, 0, ModFileHeader.v_act, Operator.Equals, 150),
            Utils.TC(100, 50, 2000, 0, ModFileHeader.Gear, Operator.MinMax, (100, 250)),
            Utils.TC(100, 50, 2000, 0, ModFileHeader.v_act, Operator.MinMax, (100, 250))

            // (0, 1e6, ModFileHeader.v_act, Operator.Analyze_Greater, 100)
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

            // [Test]
            // public void TestCase1() => RunTestCase("test_data.csv",
            //     // from_m, to_m, parameter_to_test, operator,          expected_values
            //     new TC(  1000,   1e6,  "vehicle_speed",   Operator.Greater,  50.0           // <-- double), 
            //     TC(.....)
            //     new MinMax(  0,      1e6,  "vehicle_acc",     Operator.MinMax,   (500.0, 700.0) // <-- double[]),
            //     (  0,      70,   "vehicle_gear",    Operator.ValueSet, new []{1, 2, 3, 4}   // <-- int []),
            //     (  0,      10,   "vehicle_gear",    Operator.Gear,     1              // <-- int)
            //     (...),
            //      new TC(condition(Engine on/off, ... any boolean from the modfile), )
            // );

            // new int[] {1, 2, 3} instead of just {1, 2, 3}
            // (1, 2) (int first, double second)
            // new int[] {1}
            // [2,3,5,0] -> this will not work

            // implicit tuple cast operator (construct and deconstruct)
        );
    }
}
