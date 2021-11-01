using System;
using NUnit.Framework;
using TestFramework;

namespace SampleUnitTests
{
    [TestFixture]
    class UnitTests : VECTOTestFixture
    {
        [Test]
        public void CityBus_AT_PS_RegionalDelivery_Cycle5() => VECTOTest("/home/alex/tugraz/VECTO/vecto-test-framework/SampleTests/mod_files/CityBus_AT_PS_RegionalDelivery.vmod",
            (0, 1e6, ModFileHeader.v_act, Operator.Lower, 500), 
            (0, 70, ModFileHeader.Gear, Operator.Lower, 4), // (0, 70, "gear", Operator.Greater, 4)
            (0, 70, ModFileHeader.P_eng_full_stat, Operator.Lower, 300) // (0, 70, "P_eng_full_stat", Operator.Equals, 250) -> if it's always the same value
                                                             // (0, 70, "P_eng_full_stat", Operator.Greater, min) -> if there are multiple values
                                                             // (0, 70, "P_eng_full_stat", Operator.Lower, max) 
            
        );

        [Test]
        public void CityBus_AT_PS_RegionalDelivery_Cycle6() => VECTOTest("/home/alex/tugraz/VECTO/vecto-test-framework/SampleTests/mod_files/CityBus_AT_PS_RegionalDelivery.vmod",
            (0, 1e6, ModFileHeader.v_act, Operator.Lower, 90 /*SegmentType.Time*/), 
            (0, 1e6, ModFileHeader.Gear, Operator.Lower, 4 /*, SegmentType.Time*/)
        );
    }
}

//'(int, double, string, TestFramework.Operator, int, TestFramework.SegmentType)' to '(double start, double end, string property, Operator op, double value)'

