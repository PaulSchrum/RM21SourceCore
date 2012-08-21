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
         
         Console.WriteLine();
         Console.ReadLine();
      }

      private static void validateSimpleSingleRibbon()
      {
         System.Console.WriteLine("Validate a simple single ribbon");
         RoadwayLane aLane = new RoadwayLane((CogoStation)1000.0, (CogoStation)2000.0, 12.0, -0.02);

         conditionString = "Verify width is a single value at station 11+50";
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
