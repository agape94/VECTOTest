using NUnit.Framework;
using TestFramework;

namespace SampleUnitTests
{
    [TestFixture]
    class UnitTests : VECTOTestCase
    {
        [Test]
        public void Test1() => RunTestCases(@"mod_files/CityBus_AT_PS_RegionalDelivery.vmod",
            
            // No tolerances, no analysis
            TC(100, 2000, ModFileHeader.v_act, Operator.Equals, 111),
            TC(100, 2000, ModFileHeader.Gear, Operator.MinMax, (100, 250)),
            TC(100, 2000, ModFileHeader.v_act, Operator.MinMax, (100, 250)),
            TC(100, 2000, ModFileHeader.Gear, Operator.ValueSet, new[]{0, 1, 2, 3}),
            TC(200, 1000, ModFileHeader.Gear, Operator.ValueSet, new[]{1, 2, 3}),

            // ==================================================================================

            // No tolerances, doing analysis
            TC(100, 2000, ModFileHeader.v_act, Operator.Analyze_Equals),
            TC(100, 2000, ModFileHeader.Gear, Operator.Analyze_MinMax),
            TC(100, 2000, ModFileHeader.v_act, Operator.Analyze_MinMax),
            TC(100, 2000, ModFileHeader.Gear, Operator.Analyze_ValueSet),
            TC(200, 1000, ModFileHeader.Gear, Operator.Analyze_ValueSet),


            // ==================================================================================
            
            // With time based tolerance, no analysis
            TC(100, 2000, 1, ModFileHeader.v_act, Operator.Equals, 130),
            TC(100, 2000, 2, ModFileHeader.Gear, Operator.MinMax, (100, 250)),
            TC(100, 2000, 3, ModFileHeader.v_act, Operator.MinMax, (100, 250)),
            TC(100, 2000, 4, ModFileHeader.Gear, Operator.ValueSet, new[]{0, 1, 2, 3}),
            TC(200, 1000, 5, ModFileHeader.Gear, Operator.ValueSet, new[]{1, 2, 3}),

            // ==================================================================================
            
            // With tolerances, no analysis
            TC(100, 50, 2000, 0, ModFileHeader.v_act, Operator.Equals, 140),
            TC(100, 50, 2000, 0, ModFileHeader.Gear, Operator.MinMax, (100, 250)),
            TC(100, 50, 2000, 0, ModFileHeader.v_act, Operator.MinMax, (100, 250)),
            TC(100, 50, 2000, 0, ModFileHeader.Gear, Operator.ValueSet, new[]{0, 1, 2, 3}),
            TC(200, 50, 1000, 0, ModFileHeader.Gear, Operator.ValueSet, new[]{1, 2, 3}),

            // ==================================================================================
            
            // With time based tolerance, no analysis
            TC(100, 2000, 1, ModFileHeader.v_act, Operator.Analyze_Equals),
            TC(100, 2000, 2, ModFileHeader.Gear, Operator.Analyze_MinMax),
            TC(100, 2000, 3, ModFileHeader.v_act, Operator.Analyze_MinMax),
            TC(100, 2000, 4, ModFileHeader.Gear, Operator.Analyze_ValueSet),
            TC(200, 1000, 5, ModFileHeader.Gear, Operator.Analyze_ValueSet),

            // ==================================================================================

            // With tolerances, doing analysis
            TC(100, 50, 2000, 0, ModFileHeader.v_act, Operator.Analyze_Equals),
            TC(100, 50, 2000, 0, ModFileHeader.Gear, Operator.Analyze_MinMax),
            TC(100, 50, 2000, 0, ModFileHeader.v_act, Operator.Analyze_MinMax),
            TC(100, 50, 2000, 0, ModFileHeader.Gear, Operator.Analyze_ValueSet),
            TC(200, 50, 1000, 0, ModFileHeader.Gear, Operator.Analyze_ValueSet),

            TC((dataRow) => { return dataRow[ModFileHeader.Gear] == 0;} , (dataRow) => {return dataRow[ModFileHeader.v_act] == 0;})
        );

        // [Test]
        // public void Test2() => RunTestCases(@"mod_files/CityBus_AT_PS_RegionalDelivery.vmod",
        //     // No tolerances, no analyisis
        //     TC(100, 2000, ModFileHeader.v_act, Operator.Equals, 150),
        //     TC(100, 2000, ModFileHeader.Gear, Operator.MinMax, (100, 250)),
        //     TC(100, 2000, ModFileHeader.v_act, Operator.MinMax, (100, 250)),
        //     TC(100, 2000, ModFileHeader.Gear, Operator.ValueSet, new[]{0, 1, 2, 3}),
        //     TC(200, 1000, ModFileHeader.Gear, Operator.ValueSet, new[]{1, 2, 3}),

        //     // ==================================================================================

        //     // No tolerances, doing analysis
        //     TC(100, 2000, ModFileHeader.v_act, Operator.Analyze_Equals, 150),
        //     TC(100, 2000, ModFileHeader.Gear, Operator.Analyze_MinMax, (100, 250)),
        //     TC(100, 2000, ModFileHeader.v_act, Operator.Analyze_MinMax, (100, 250)),
        //     TC(100, 2000, ModFileHeader.Gear, Operator.Analyze_ValueSet, new[]{0, 1, 2, 3}),
        //     TC(200, 1000, ModFileHeader.Gear, Operator.Analyze_ValueSet, new[]{1, 2, 3}),

        //     // ==================================================================================
            
        //     // With tolerances, no analysis
        //     TC(100, 50, 2000, 0, ModFileHeader.v_act, Operator.Equals, 150),
        //     TC(100, 50, 2000, 0, ModFileHeader.Gear, Operator.MinMax, (100, 250)),
        //     TC(100, 50, 2000, 0, ModFileHeader.v_act, Operator.MinMax, (100, 250)),
        //     TC(100, 50, 2000, 0, ModFileHeader.Gear, Operator.ValueSet, new[]{0, 1, 2, 3}),
        //     TC(200, 50, 1000, 0, ModFileHeader.Gear, Operator.ValueSet, new[]{1, 2, 3}),

        //     // ==================================================================================

        //     // With tolerances, doing analysis
        //     TC(100, 50, 2000, 0, ModFileHeader.v_act, Operator.Analyze_Equals, 150),
        //     TC(100, 50, 2000, 0, ModFileHeader.Gear, Operator.Analyze_MinMax, (100, 250)),
        //     TC(100, 50, 2000, 0, ModFileHeader.v_act, Operator.Analyze_MinMax, (100, 250)),
        //     TC(100, 50, 2000, 0, ModFileHeader.Gear, Operator.Analyze_ValueSet, new[]{0, 1, 2, 3}),
        //     TC(200, 50, 1000, 0, ModFileHeader.Gear, Operator.Analyze_ValueSet, new[]{1, 2, 3})

            /* wihses:
                * tolerances: argument to this analyze method to specify tolerances to generate the rules. 
                (0, 2000, ModFileHeader.v_act, Operator.Analyze.MinMax, apply_tolerances(True/False), ToleranceValue (optional))
                if the tolerance value is not provided, derive the tolerance from vehicle speed. 
                
                new TC(condition(Engine on/off, ... any boolean from the modfile), )
                 

                Tolerances (based on vehicle speed) -> done

                Remove parameters of Analyze functions (make them optional) -> done

                Fix output bug in the exception messages(!) System.Double instead of actual number -> done

                Maybe have a TC function that gets two lambda expressions and whenever the
                first one holds, the second one must hold as well.
                Both get a data row as a parameter and return a bool

                Integration in VECTO (!)
                    Provide a few examples (basically adapt one or two test files to the new framework)

                

                Documentation on how to use the framework

                

            <---- TO BE DISCUSSED ----> 
            */
        // );
    }
}
