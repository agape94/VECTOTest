using System;
using NUnit.Framework;
using TestFramework;

namespace TestFramework.UnitTests
{
    [TestFixture]
    class UnitTests : TestCase
    {
        [SetUp]
        public void SetUp()
        {
            Console.WriteLine("Setup..");
        }

        [Test]
        public void CityBus_AT_PS_RegionalDelivery_Cycle5() => RunTestCase();
    }
}
