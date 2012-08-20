using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ptsCogo;
using rm21Core;
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

         conditionString = "Verify width is a 12.0 at station 11+50";
         expectedDbl = 12.00;
         TestingFramework.assertEquals<double?>(expectedDbl, actualDbl, conditionString);


      }
   }
}
