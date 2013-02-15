using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using NUnit.Framework;
using ptsCogo;
using rm21Core;
using rm21Core.CorridorTypes;
using rm21Core.Ribbons;


namespace NUnitTestingLibrary
{
   [TestFixture]
   public class TestClassForCorridors
   {
      private rm21Corridor testCorridor = null;
      private PGLGrouping testPGLgrouping = null;
      private IRibbonLike testRibbon = null;

      public void driveTestsFromConsole()
      {
         //CorridorTestsSetup();
         setupTest2();
         //OffsetOutsideOfRibbon_RightPGLOutside_plus12ft();
         //OffsetOutsideOfRibbon_RightPGLOutside_plus39ft();
         //OffsetOutsideOfRibbon_RightPGLInside_plus9ft();
         //OffsetOutsideOfRibbon_LeftPGLOutside_neg39ft();
         //OffsetOutsideOfRibbon_LeftPGLInside_neg9ft();
      }

      [SetUp]
      public void CorridorTestsSetup()
      {
         setupTest1();
      }

      private void setupTest1()
      {
         testCorridor = new rm21RoadwayCorridor("L");

         testCorridor.Alignment.BeginStation = 1000.0;
         testCorridor.Alignment.EndStation = 10000.0;

         PGLGrouping pglGrLT = new PGLGrouping(-1);
         PGLGrouping pglGrRT = new PGLGrouping(1);

         PGLoffset pgloRT = new PGLoffset((CogoStation)1000.0, (CogoStation)10000, 0.0, 0.0);  /* PGL RT */
         pgloRT.addWidenedSegment((CogoStation)2555.0, (CogoStation)2955.0, 15.0,
            (CogoStation)8050.0, (CogoStation)8050.001);
         pglGrRT.thePGLoffsetRibbon = pgloRT;

         PGLoffset pgloLT = new PGLoffset((CogoStation)1000.0, (CogoStation)10000, 0.0, 0.0);  /* PGL LT */
         pgloLT.addWidenedSegment((CogoStation)2555.0, (CogoStation)2955.0, 15.0,
            (CogoStation)8050.0, (CogoStation)8050.001);
         pglGrLT.thePGLoffsetRibbon = pgloLT;

         /* Back Thru Lane, Inner */
         RoadwayLane rdyLane = new RoadwayLane((CogoStation)1000, (CogoStation)10000, 0.0, -0.02);
         rdyLane.addWidenedSegment((CogoStation)2235, (CogoStation)2555, 12.0,
            (CogoStation)8050, (CogoStation)8050.001);
         rdyLane.addCrossSlopeChangedSegment((CogoStation)2200, (CogoStation)2300, 0.08,
            (CogoStation)2500, (CogoStation)2600);
         pglGrLT.addOutsideRibbon(rdyLane);
         /* Back Thru Lane, Outer */
         rdyLane = new RoadwayLane((CogoStation)1000, (CogoStation)10000, 12.0, -0.02);
         rdyLane.addCrossSlopeChangedSegment((CogoStation)2200, (CogoStation)2300, 0.08,
            (CogoStation)2500, (CogoStation)2600);
         pglGrLT.addOutsideRibbon(rdyLane);

         Shoulder aShldr = new Shoulder((CogoStation)1000, (CogoStation)10000, 10.0, -0.08);
         aShldr.addWidenedSegment((CogoStation)2000.0, (CogoStation)2040.0, 17.0,
            (CogoStation)2250.0, (CogoStation)2290.00);
         aShldr.addCrossSlopeChangedSegment((CogoStation)2200, (CogoStation)2300, 0.02,
            (CogoStation)2500, (CogoStation)2600);

         pglGrLT.addOutsideRibbon(aShldr);

         /* Outside Cut Ditch, LT */
         pglGrLT.addOutsideRibbon(new FrontSlopeCutDitch((CogoStation)1000, (CogoStation)10000, 15.0, -1.0 / 4.0));

         /* Final Ray Sheet, LT */
         //var newCutSlope = new newCutSlope((CogoStation 1000, (CogoStation) 10000, null, 1.0 / 2.0));
         //pglGrLT.addOutsideRibbon(newCutSlope);

         /* Median Shoulder LT */
         aShldr = new Shoulder((CogoStation)1000, (CogoStation)10000, 0.0, -0.04);
         aShldr.addWidenedSegment((CogoStation)2555.0, (CogoStation)2715.0, 6.0,
            (CogoStation)8050.0, (CogoStation)8050.001);
         pglGrLT.addInsideRibbon(aShldr);

         /* Median Ditch Slope LT */
         FrontSlopeCutDitch ditchFS = new FrontSlopeCutDitch((CogoStation)1000, (CogoStation)10000, 0.0, -1.0 / 4.0);
         ditchFS.addWidenedSegment((CogoStation)2715.0, (CogoStation)2955.0, 9.0,
            (CogoStation)8050.0, (CogoStation)8050.001);
         pglGrLT.addInsideRibbon(ditchFS);


         /* Ahead Thru Lane, Inner */
         rdyLane = new RoadwayLane((CogoStation)1000, (CogoStation)10000, 0.0, -0.02);
         rdyLane.addWidenedSegment((CogoStation)2235, (CogoStation)2555, 12.0,
            (CogoStation)8050, (CogoStation)8050.001);
         rdyLane.addCrossSlopeChangedSegment((CogoStation)2240, (CogoStation)2300, -0.08,
            (CogoStation)2500, (CogoStation)2560);
         pglGrRT.addOutsideRibbon(rdyLane);

         /* Ahead Thru Lane, Outer */
         rdyLane = new RoadwayLane((CogoStation)1000, (CogoStation)10000, 12.0, -0.02);
         rdyLane.addCrossSlopeChangedSegment((CogoStation)2240, (CogoStation)2300, -0.08,
            (CogoStation)2500, (CogoStation)2560);
         pglGrRT.addOutsideRibbon(rdyLane);
         pglGrRT.addOutsideRibbon(new Shoulder((CogoStation)1000, (CogoStation)10000, 10.0, -0.08));
         pglGrRT.addOutsideRibbon(new FrontSlopeCutDitch((CogoStation)1000, (CogoStation)10000, 15.0, -1.0 / 4.0));

         /* Final Ray Sheet, RT */
         var newCutSlope = new RaySheet((CogoStation) 1000, (CogoStation) 10000, 1.0 / 2.0);
         //pglGrRT.addOutsideRibbon(newCutSlope);

         /* Median Shoulder RT */
         aShldr = new Shoulder((CogoStation)1000, (CogoStation)10000, 0.0, -0.04);
         aShldr.addWidenedSegment((CogoStation)2555.0, (CogoStation)2715.0, 6.0,
            (CogoStation)8050.0, (CogoStation)8050.001);
         pglGrRT.addInsideRibbon(aShldr);

         /* Median Ditch Slope RT */
         ditchFS = new FrontSlopeCutDitch((CogoStation)1000, (CogoStation)10000, 0.0, -1.0 / 4.0);
         ditchFS.addWidenedSegment((CogoStation)2715.0, (CogoStation)2955.0, 9.0,
            (CogoStation)8050.0, (CogoStation)8050.001);
         pglGrRT.addInsideRibbon(ditchFS);


         testCorridor.addPGLgrouping(pglGrLT);
         testCorridor.addPGLgrouping(pglGrRT);

         Profile existingGroundSurface = new Profile();
         existingGroundSurface.addStationAndElevation((CogoStation) (-200.0), 1.0);
         existingGroundSurface.addStationAndElevation((CogoStation) 200, 2.5);

         List<Profile> targetSurfaceProfiles = new List<Profile>();
         targetSurfaceProfiles.Insert(0, existingGroundSurface);

         testCorridor.TargetSurfaceXSProfiles = targetSurfaceProfiles;

      }

      private void setupTest2()
      {
         testCorridor = new rm21RoadwayCorridor("L");

         testCorridor.Alignment.BeginStation = 1000.0;
         testCorridor.Alignment.EndStation = 10000.0;

         PGLGrouping pglGrLT = new PGLGrouping(-1);
 
         PGLoffset pgloLT = new PGLoffset((CogoStation)1000.0, (CogoStation)10000, 0.0, 0.0);  /* PGL LT */
         pgloLT.addWidenedSegment((CogoStation)2555.0, (CogoStation)2955.0, 15.0,
            (CogoStation)8050.0, (CogoStation)8050.001);
         pglGrLT.thePGLoffsetRibbon = pgloLT;

         /* Back Thru Lane, Inner */
         RoadwayLane rdyLane = new RoadwayLane((CogoStation)1000, (CogoStation)10000, 0.0, -0.02);
         rdyLane.addWidenedSegment((CogoStation)2235, (CogoStation)2555, 12.0,
            (CogoStation)8050, (CogoStation)8050.001);
         rdyLane.addCrossSlopeChangedSegment((CogoStation)2200, (CogoStation)2300, 0.08,
            (CogoStation)2500, (CogoStation)2600);
         pglGrLT.addOutsideRibbon(rdyLane);

         /* Median Shoulder LT */
         Shoulder aShldr = new Shoulder((CogoStation)1000, (CogoStation)10000, 0.0, -0.04);
         aShldr.addWidenedSegment((CogoStation)2555.0, (CogoStation)2715.0, 6.0,
            (CogoStation)8050.0, (CogoStation)8050.001);
         pglGrLT.addInsideRibbon(aShldr);

         testCorridor.addPGLgrouping(pglGrLT);

      }

      [Test]
      public void OffsetOutsideOfRibbon_RightPGLOutside_plus12ft()
      {
         testAribbonOffset(1, true, 1, 1200.0, 12.0);
      }

      [Test]
      public void OffsetOutsideOfRibbon_RightPGLOutside_plus39ft()
      {
         testAribbonOffset(1, true, 1, 3200.0, 39.0);
      }

      [Test]
      public void OffsetOutsideOfRibbon_RightPGLInside_plus9ft()
      {
         testAribbonOffset(1, false, 0, 3200.0, 9.0);
      }

      [Test]
      public void OffsetOutsideOfRibbon_LeftPGLOutside_neg39ft()
      {
         testAribbonOffset(0, true, 1, 3200.0, -39.0);
      }

      [Test]
      public void OffsetOutsideOfRibbon_LeftPGLInside_neg9ft()
      {
         testAribbonOffset(0, false, 0, 3200.0, -9.0);
      }

      private void testAribbonOffset(int pglGroupingIndex, bool outsideRibbonOrNot, int ribbonIndex, double station, double expectedValue)
      {
         Profile offsetProfile = null;
         double? testDbl = 0.0;
         double actualValue;

         testPGLgrouping = testCorridor.allPGLgroupings[pglGroupingIndex];
         if (outsideRibbonOrNot == true)
            testRibbon = testPGLgrouping.outsideRibbons.ToList()[ribbonIndex];
         else
            testRibbon = testPGLgrouping.insideRibbons.ToList()[ribbonIndex];

         offsetProfile = testRibbon.getOffsetProfile();

         testDbl = offsetProfile.getElevation((CogoStation) station);

         if (testDbl == null)
            throw new NullReferenceException();
         else
            actualValue = (double) testDbl;

         Assert.AreEqual(expectedValue, actualValue, 0.00001);
      }
   }
}
