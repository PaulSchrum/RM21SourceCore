using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using NUnit.Framework;
using ptsCogo;
using ptsCogo.CorridorTypes;
using ptsCogo.Ribbons;
using ptsCogo.Horizontal;
using NUnitTestingLibrary.Mocks;
using ptsCogo.Angle;
using ptsCogo.coordinates.CurvilinearCoordinates;


namespace NUnitTestingLibrary
{
   [TestFixture]
   class TestClassForRibbons
   {
      private RoadwayLane simpleRibbon;
      private RoadwayLane complexRibbon;

      [SetUp]
      public void setupTestClassForRibbons()
      {
         simpleRibbon = new RoadwayLane((CogoStation)1000.0, (CogoStation)2000.0, 12.0, -0.02);
         complexRibbon = new RoadwayLane((CogoStation)1000.0, (CogoStation)2000.0, 12.0, -0.02);
         complexRibbon.addWidenedSegment(
            (CogoStation)1200.0, (CogoStation)1400.00, 16.0, 
            (CogoStation)1600.0, (CogoStation)1800.00);
         complexRibbon.addCrossSlopeChangedSegment(
            (CogoStation)1100.0, (CogoStation)1110.00, 0.08, 
            (CogoStation)1182.0, (CogoStation)1192.00);
         complexRibbon.addCrossSlopeChangedSegment(
            (CogoStation)1500.0, (CogoStation)1520.00, 0.08, 
            (CogoStation)1560.0, (CogoStation)1580.00);
      }

      [Test]
      public void RibbonSingleSimple_station1120_widthIsSingleValue()
      {
         tupleNullableDoubles result;
         String conditionString = "Width is a single value at station 11+20";
         Boolean expectedBl = true;
         simpleRibbon.getActualWidth((CogoStation) 1120.0, out result);
         Boolean actualBl = result.isSingleValue;
         Assert.AreEqual(expected: expectedBl, actual: actualBl, message: conditionString);
      }

      [Test]
      public void RibbonSingleSimple_station1150_widthIs12()
      {
         tupleNullableDoubles result;
         String conditionString = "Width is 12.0 at station 11+50";
         Double expectedDbl = 12.0;
         simpleRibbon.getActualWidth((CogoStation)1150.0, out result);
         Double actualDbl = (Double) result.ahead;
         Assert.AreEqual(expected: expectedDbl, actual: actualDbl, message: conditionString);
      }

      [Test]
      public void RibbonSingleSimple_station1120_crossSlopeIsSingleValue()
      {
         tupleNullableDoubles result;
         String conditionString = "Cross Slope is a single value at station 11+20";
         Boolean expectedBl = true;
         simpleRibbon.getCrossSlope((CogoStation)1120.0, out result);
         Boolean actualBl = result.isSingleValue;
         Assert.AreEqual(expected: expectedBl, actual: actualBl, message: conditionString);
      }

      [Test]
      public void RibbonSingleSimple_station1150_crossSlopeIsNeg0p02()
      {
         tupleNullableDoubles result;
         String conditionString = "Cross Slope is -2% at station 11+50";
         Double expectedDbl = -0.02;
         simpleRibbon.getCrossSlope((CogoStation)1150.0, out result);
         Double actualDbl = (Double)result.ahead;
         Assert.AreEqual(expected: expectedDbl, actual: actualDbl, message: conditionString);
      }

      [Test]
      public void RibbonSingleSimple_WhenAtStation1150offset7RT_ElevationDropIsNeg0p14()
      {
         String conditionString = "Verify elevation drop is -0.14 when 7.0' rt of station 11+50";
         Double expectedDbl = -0.14;
         var soe1 = new StationOffsetElevation();
         soe1.station = 1150.00;
         soe1.offset = 7.0;
         soe1.elevation = 0.0;
         simpleRibbon.accumulateRibbonTraversal(ref soe1);
         Double actualDbl = soe1.elevation;
         Assert.AreEqual(expected: expectedDbl, actual: actualDbl, message: conditionString);
      }

      [Test]
      public void RibbonSingleSimple_WhenAtStation1150offset14_ElevationDropIsNeg0p24()
      {
         String conditionString = "Verify elevation drop is -0.24 when 12.0' rt of station 11+50";
         Double expectedDbl = -0.24;
         var soe1 = new StationOffsetElevation();
         soe1.station = 1150.00;
         soe1.offset = 14.0;
         soe1.elevation = 0.0;
         simpleRibbon.accumulateRibbonTraversal(ref soe1);
         Double actualDbl = soe1.elevation;
         Assert.AreEqual(expected: expectedDbl, actual: actualDbl, message: conditionString);
      }

      [Test]
      public void RibbonSingleSimple_WhenAtStation1150offset14_OffsetRemainderIs2()
      {
         String conditionString = "At 14 ft right of 11+50, offset has remainder value of 2.0.";
         Double expectedDbl = 2.0;
         var soe1 = new StationOffsetElevation();
         soe1.station = 1150.00;
         soe1.offset = 14.0;
         soe1.elevation = 0.0;
         simpleRibbon.accumulateRibbonTraversal(ref soe1);
         Double actualDbl = soe1.offset;
         Assert.AreEqual(expected: expectedDbl, actual: actualDbl, message: conditionString);
      }

      [Test]
      public void ComplexRibbon_WhenAtStation1150_actualWidthIs12()
      {
         String conditionString = "Verify width is 12.0 value at station 11+50";
         Double expectedDbl = 12.0;
         Double actualDbl = (Double)this.complexRibbon.getActualWidth((CogoStation)1150.0);
         Assert.AreEqual(expected: expectedDbl, actual: actualDbl, message: conditionString);
      }

      [Test]
      public void ComplexRibbon_WhenAtStation1250_actualWidthIs13()
      {
         String conditionString = "Verify width is 13.0 value at station 12+50";
         Double expectedDbl = 13.0;
         Double actualDbl = (Double)this.complexRibbon.getActualWidth((CogoStation)1250.0);
         Assert.AreEqual(expected: expectedDbl, actual: actualDbl, message: conditionString);
      }

      [Test]
      public void ComplexRibbon_WhenAtStation1450_actualWidthIs16()
      {
         String conditionString = "Verify width is 16.0 value at station 14+50";
         Double expectedDbl = 16.0;
         Double actualDbl = (Double)this.complexRibbon.getActualWidth((CogoStation)1450.0);
         Assert.AreEqual(expected: expectedDbl, actual: actualDbl, message: conditionString);
      }

      [Test]
      public void ComplexRibbon_WhenAtStation1650_actualWidthIs15()
      {
         String conditionString = "Verify width is 15.0 value at station 16+50";
         Double expectedDbl = 15.0;
         Double actualDbl = (Double)this.complexRibbon.getActualWidth((CogoStation)1650.0);
         Assert.AreEqual(expected: expectedDbl, actual: actualDbl, message: conditionString);
      }

      [Test]
      public void ComplexRibbon_WhenAtStation1850_actualWidthIs12()
      {
         String conditionString = "Verify width is 12.0 value at station 18+50";
         Double expectedDbl = 12.0;
         Double actualDbl = (Double)this.complexRibbon.getActualWidth((CogoStation)1850.0);
         Assert.AreEqual(expected: expectedDbl, actual: actualDbl, message: conditionString);
      }

      [Test]
      public void ComplexRibbon_WhenAtStation1050_CrossSlopeIsNeg0p02()
      {
         String conditionString = "Cross slope is -0.02 value at station 10+50";
         Double expectedDbl = -0.02;
         Double actualDbl = (Double)this.complexRibbon.getCrossSlope((CogoStation)1050.0);
         Assert.AreEqual(expected: expectedDbl, actual: actualDbl, message: conditionString);
      }

      [Test]
      public void ComplexRibbon_WhenAtStation1107_CrossSlopeIs0p05()
      {
         String conditionString = "Cross slope is +0.05 value at station 11+07";
         Double expectedDbl = 0.05;
         Double actualDbl = (Double)this.complexRibbon.getCrossSlope((CogoStation)1107.0);
         Assert.AreEqual(expected: expectedDbl, actual: actualDbl, message: conditionString);
      }

      [Test]
      public void ComplexRibbon_WhenAtStation1550_CrossSlopeIs0p08()
      {
         String conditionString = "Cross slope is +0.08 value at station 15+50";
         Double expectedDbl = 0.08;
         Double actualDbl = Math.Round((Double)this.complexRibbon.getCrossSlope((CogoStation)1550.0), 6);
         Assert.AreEqual(expected: expectedDbl, actual: actualDbl, message: conditionString);
      }

      [Test]
      public void ComplexRibbon_WhenAtStation1550offset18_ElevationChangeIsPlus1p28()
      {
         String conditionString = "Elevation change is +1.28 when 18.0' rt of station 15+50";
         Double expectedDbl = 1.28;
         var soe1 = new StationOffsetElevation();
         soe1.station = 1550.00;
         soe1.offset = 18.0;
         soe1.elevation = 0.0;
         complexRibbon.accumulateRibbonTraversal(ref soe1);
         Double actualDbl = (Double)soe1.elevation;
         Assert.AreEqual(expected: expectedDbl, actual: actualDbl, message: conditionString);
      }

      [Test]
      public void ComplexRibbon_WhenAtStation1550offset18_remainderOffsetIs2()
      {
         String conditionString = "Offset remainder is 2.0 when 18.0' rt of station 15+50";
         Double expectedDbl = 2.0;
         var soe1 = new StationOffsetElevation();
         soe1.station = 1550.00;
         soe1.offset = 18.0;
         soe1.elevation = 0.0;
         complexRibbon.accumulateRibbonTraversal(ref soe1);
         Double actualDbl = (Double)soe1.offset;
         Assert.AreEqual(expected: expectedDbl, actual: actualDbl, message: conditionString);
      }

   }
}
