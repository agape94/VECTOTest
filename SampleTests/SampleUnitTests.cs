using System;
using NUnit.Framework;
using TestFramework;

namespace SampleUnitTests
{
    [TestFixture]
    class UnitTests : TestFixture
    {
        [SetUp]
        public void SetUp()
        {
            Console.WriteLine("Setup..");
        }

        [Test]
        public void CityBus_AT_PS_RegionalDelivery_Cycle5() => Test("/home/alex/tugraz/VECTO/vecto-test-framework/SampleTests/mod_files/CityBus_AT_PS_RegionalDelivery.vmod",
            (0, 1e6, "v_act [km/h]", Operator.Greater, 50), 
            (0, 70, "gear", Operator.Lower, 4), // (0, 70, "gear", Operator.Greater, 4)
            (0, 70, "P_eng_full_stat", Operator.Equals, 300) // (0, 70, "P_eng_full_stat", Operator.Equals, 250) -> if it's always the same value
                                                             // (0, 70, "P_eng_full_stat", Operator.Greater, min) -> if there are multiple values
                                                             // (0, 70, "P_eng_full_stat", Operator.Lower, max) 
            
        );

        [Test]
        public void CityBus_AT_PS_RegionalDelivery_Cycle6() => Test("/home/alex/tugraz/VECTO/vecto-test-framework/SampleTests/mod_files/CityBus_AT_PS_RegionalDelivery.vmod",
            (0, 1e6, "v_act", Operator.Greater, 50, DelimiterType.Time), 
            (0, 1e6, "gear", Operator.Lower, 4, DelimiterType.Time)
        );
    }
}

