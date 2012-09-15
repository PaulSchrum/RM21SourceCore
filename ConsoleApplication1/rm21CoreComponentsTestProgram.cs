using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ptsCogo;
using rm21Core;
using ptsCogo.coordinates.CurvilinearCoordinates;
using PaulsTestingFramework;

namespace ConsoleApplication1
{
   class rm21CoreComponentsTestProgram
   {
      private static string conditionString;
      static double? expectedDbl, actualDbl;
      static bool expectedBl, actualBl;
      static tupleNullableDoubles result;

      static void Main(string[] args)
      {
         validateSimpleSingleRibbon();
         validateSingleRibbonWithVaryingWidthsAndCrossSlopes();

         validateSimplePGLgrouping();
         
         Console.WriteLine();
         Console.ReadLine();
      }

      private static void validateSimplePGLgrouping()
      {
         PGLGrouping aPGLgrouping;
         aPGLgrouping = buildNewPGLgrouping(1);

         rm21Corridor aRoad = new rm21Corridor();
         aRoad.addPGLgrouping(aPGLgrouping);

         aPGLgrouping = buildNewPGLgrouping(-1);
         aRoad.addPGLgrouping(aPGLgrouping);

         // at this point we now have the lanes and shoulders of a
         // two-lane road.  Let's test it.

         expectedDbl = 0.0;
         StationOffsetElevation soe1 = new StationOffsetElevation(2020.0, 0.0, 0.0);
         aRoad.getElevation(ref soe1);
         actualDbl = soe1.elevation;
         conditionString = "Verify elevation is 0.0 at station 20+20, offset 0.0";
         TestingFramework.assertEquals<double?>(expectedDbl, actualDbl, conditionString);


         expectedDbl = -0.76;
         soe1 = new StationOffsetElevation(2020.0, 18.5, 0.0);
         aRoad.getElevation(ref soe1);
         actualDbl = Math.Round(soe1.elevation, 6);
         conditionString = "Verify elevation is -0.76 at station 20+20, offset 18.5 feet right";
         TestingFramework.assertEquals<double?>(expectedDbl, actualDbl, conditionString);

         expectedDbl = -0.68;
         soe1 = new StationOffsetElevation(2020.0, -17.5, 0.0);
         aRoad.getElevation(ref soe1);
         actualDbl = soe1.elevation;
         conditionString = "Verify elevation is -0.68 at station 20+20, offset 17.5 feet left";
         TestingFramework.assertEquals<double?>(expectedDbl, actualDbl, conditionString);



      }

      private static PGLGrouping buildNewPGLgrouping(int whichSide)
      {
         PGLGrouping returnableGrouping = new PGLGrouping(whichSide);

         IRibbonLike newRibbon;
         newRibbon = new RoadwayLane((CogoStation)1000.0, (CogoStation)9000.0, 12.0, -0.02);
         //newRibbon.setMyIndex(whichSide);
         returnableGrouping.addOutsideRibbon(newRibbon);

         newRibbon = new RoadwayLane((CogoStation)1000.0, (CogoStation)9000.0, 8.0, -0.08);
         returnableGrouping.addOutsideRibbon(newRibbon);

         return returnableGrouping;
      }

      private static void validateSingleRibbonWithVaryingWidthsAndCrossSlopes()
      {
         System.Console.WriteLine("Validate a nontrivial single ribbon");
         RoadwayLane aLane = new RoadwayLane((CogoStation)1000.0, (CogoStation)2000.0, 12.0, -0.02);

         aLane.addWidenedSegment((CogoStation)1200.0, (CogoStation)1400.00, 16.0, (CogoStation)1600.0, (CogoStation)1800.00);

         conditionString = "Verify width is 12.0 value at station 11+50";
         actualDbl = aLane.getActualWidth((CogoStation)1150.0, out result);
         expectedDbl = 12.00;
         TestingFramework.assertEquals<double?>(actualDbl, expectedDbl, conditionString);

         conditionString = "Verify width is 13.0 at station 12+50";
         actualDbl = aLane.getActualWidth((CogoStation)1250.0, out result);
         expectedDbl = 13.00;
         TestingFramework.assertEquals<double?>(expectedDbl, actualDbl, conditionString);

         conditionString = "Verify width is 16.0 at station 14+50";
         actualDbl = aLane.getActualWidth((CogoStation)1450.0, out result);
         expectedDbl = 16.00;
         TestingFramework.assertEquals<double?>(expectedDbl, actualDbl, conditionString);

         conditionString = "Verify width is 15.0 at station 16+50";
         actualDbl = aLane.getActualWidth((CogoStation)1650.0, out result);
         expectedDbl = 15.00;
         TestingFramework.assertEquals<double?>(expectedDbl, actualDbl, conditionString);

         conditionString = "Verify width is 12.0 at station 18+50";
         actualDbl = aLane.getActualWidth((CogoStation)1850.0, out result);
         expectedDbl = 12.00;
         TestingFramework.assertEquals<double?>(expectedDbl, actualDbl, conditionString);

         aLane.addCrossSlopeChangedSegment((CogoStation)1100.0, (CogoStation)1110.00, 0.08, (CogoStation)1182.0, (CogoStation)1192.00);
         conditionString = "Verify cross slope is -0.02 value at station 10+50";
         actualDbl = aLane.getCrossSlope((CogoStation)1050.0, out result);
         expectedDbl = -0.02;
         TestingFramework.assertEquals<double?>(actualDbl, expectedDbl, conditionString);

         conditionString = "Verify cross slope is 0.05 value at station 11+07";
         actualDbl = aLane.getCrossSlope((CogoStation)1107.0, out result);
         expectedDbl = 0.05;
         TestingFramework.assertEquals<double?>(actualDbl, expectedDbl, conditionString);

         conditionString = "Verify cross slope is 0.05 value at station 11+50";
         actualDbl = aLane.getCrossSlope((CogoStation)1150.0, out result);
         expectedDbl = 0.08;
         TestingFramework.assertEquals<double?>(actualDbl, expectedDbl, conditionString);

         conditionString = "Verify cross slope is 0.05 value at station 11+85";
         actualDbl = aLane.getCrossSlope((CogoStation)1185.0, out result);
         expectedDbl = 0.05;
         TestingFramework.assertEquals<double?>(actualDbl, expectedDbl, conditionString);

         aLane.addCrossSlopeChangedSegment((CogoStation)1500.0, (CogoStation)1520.00, 0.08, (CogoStation)1560.0, (CogoStation)1580.00);
         conditionString = "Verify elevation change is +1.28 when 18.0' rt of station 15+50 . . .";
         StationOffsetElevation soe1 = new StationOffsetElevation();
         soe1.station = 1550.00;
         soe1.offset = 18.0;
         soe1.elevation = 0.0;
         aLane.accumulateRibbonTraversal(ref soe1);
         expectedDbl = 1.28;
         actualDbl = soe1.elevation;
         TestingFramework.assertEquals<double?>(expectedDbl, actualDbl, conditionString);

         conditionString = ". . . and offset has remainder value of 2.0.";
         expectedDbl = 2.0;
         actualDbl = soe1.offset;
         TestingFramework.assertEquals<double?>(expectedDbl, actualDbl, conditionString);

      }

      private static void validateSimpleSingleRibbon()
      {
         System.Console.WriteLine("Validate a simple single ribbon");
         RoadwayLane aLane = new RoadwayLane((CogoStation)1000.0, (CogoStation)2000.0, 12.0, -0.02);

         conditionString = "Verify width is a single value at station 11+20";
         actualDbl = aLane.getActualWidth((CogoStation)1120.0, out result);
         expectedBl = true;
         actualBl = result.isSingleValue;
         TestingFramework.assertEquals<bool>(expectedBl, actualBl, conditionString);

         conditionString = "Verify width is 12.0 at station 11+50";
         expectedDbl = 12.00;
         TestingFramework.assertEquals<double?>(expectedDbl, actualDbl, conditionString);

         conditionString = "Verify cross slope is a single value at station 11+50";
         actualDbl = aLane.getCrossSlope((CogoStation)1120.0, out result);
         expectedBl = true;
         actualBl = result.isSingleValue;
         TestingFramework.assertEquals<bool>(expectedBl, actualBl, conditionString);

         conditionString = "Verify cross slope is -0.02 at station 11+50";
         expectedDbl = -0.02;
         TestingFramework.assertEquals<double?>(expectedDbl, actualDbl, conditionString);

         conditionString = "Verify elevation drop is -0.14 when 7.0' rt of station 11+50";
         StationOffsetElevation soe1 = new StationOffsetElevation();
         soe1.station = 1150.00; 
         soe1.offset = 7.0; 
         soe1.elevation = 0.0;
         aLane.accumulateRibbonTraversal(ref soe1);
         expectedDbl = -0.14;
         actualDbl = soe1.elevation;
         TestingFramework.assertEquals<double?>(expectedDbl, actualDbl, conditionString);

         conditionString = "Verify elevation drop is -0.24 when 12.0' rt of station 11+50 . . .";
         soe1.station = 1150.00; soe1.offset = 14.0; soe1.elevation = 0.0;
         aLane.accumulateRibbonTraversal(ref soe1);
         expectedDbl = -0.24;
         actualDbl = soe1.elevation;
         TestingFramework.assertEquals<double?>(expectedDbl, actualDbl, conditionString);

         conditionString = ". . . and offset has remainder value of 2.0.";
         expectedDbl = 2.0;
         actualDbl = soe1.offset;
         TestingFramework.assertEquals<double?>(expectedDbl, actualDbl, conditionString);

         
      }
   }
}
