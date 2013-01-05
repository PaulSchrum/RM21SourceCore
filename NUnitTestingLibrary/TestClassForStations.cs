using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using ptsCogo;

namespace NUnitTestingLibrary
{
   [TestFixture]
   public class TestClassForStations
   {
      public CogoStation aStation { get; set; }
      public double expectedDouble1 { get; set; }

      [SetUp]
      public void StationTestSetup()
      {

      }

      [Test]
      public void createNewStation_doubleOnly_noAlignment()
      {
         expectedDouble1 = 1234.0;
         aStation = new CogoStation(1234.0);
         Assert.AreEqual(expectedDouble1, aStation.trueStation, 0.0);
      }

      [Test]
      public void createNewStation_stringNoPlus_noAlignment()
      {
         expectedDouble1 = 1235.0;
         aStation = new CogoStation("1235.0");
         Assert.AreEqual(expectedDouble1, aStation.trueStation, 0.0);
      }

      [Test]
      public void createNewStation_string_noAlignment()
      {
         expectedDouble1 = 1236.0;
         aStation = new CogoStation("12+36.00");
         Assert.AreEqual(expectedDouble1, aStation.trueStation, 0.0);
      }

   }
}
