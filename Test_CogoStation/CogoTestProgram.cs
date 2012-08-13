using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ptsCogo;
using PaulsTestingFramework;

namespace Test_CogoStation
{
   class CogoTestProgram
   {
      private static GenericAlignment anAlignment;
      private static GenericAlignment secondaryAlignment;
      private static CogoStation sta1;
      private static CogoStation station2;
      private static CogoStation stationOnOtherAlignment;

      static void Main(string[] args)
      {
         //testBuildAgenericAlignment();
         //setupSomeStations();
         //testStationArithmetic();
         testCogoProfile();
         Console.Read();
      }

      private static void testCogoProfile()
      {
         vpiList aVpiList = new vpiList();
         aVpiList.add(1062.50, 2178.23);
         aVpiList.add(1120.00, 2173.973, 115.0);
         aVpiList.add(1220.00, 2173.140,  85.0);
         aVpiList.add(1315.00, 2168.2265, 90.0);
         aVpiList.add(1365.00, 2167.8765);

         Profile aProfile = new Profile(aVpiList);

         string conditionString;
         bool expectedBl; bool actualBl;
         double? expectedDbl; double? actualDbl;
         tupleNullableDoubles result;

         ///////////////////
         System.Console.Write("Test on a vertical curve, station 11+20");
         conditionString = "Verify tupleNullableDoubles isSingleValue is true";
         result.back = result.ahead = 0.0;
         result.isSingleValue = false;
         aProfile.getElevation((CogoStation)1120.00, out result);
         expectedBl = true;
         actualBl = result.isSingleValue;
         TestingFramework.assertEquals<bool>(expectedBl, actualBl, conditionString);

         conditionString = "Verify back elevation is 2174.9175";
         expectedDbl = 2174.9175;
         actualDbl = Math.Round((double)result.back, 4);
         TestingFramework.assertEquals<double?>(expectedDbl, actualDbl, conditionString);

         conditionString = "Verify ahead elevation is 2174.9175";
         expectedDbl = 2174.9175;
         actualDbl = Math.Round((double)result.ahead, 4);
         TestingFramework.assertEquals<double?>(expectedDbl, actualDbl, conditionString);

         ///////////////////
         System.Console.Write("Test on a vertical tangent, station 12+65");
         conditionString = "Verify tupleNullableDoubles isSingleValue is true";
         result.back = result.ahead = 0.0;
         result.isSingleValue = false;
         aProfile.getElevation((CogoStation)1265.00, out result);
         expectedBl = true;
         actualBl = result.isSingleValue;
         TestingFramework.assertEquals<bool>(expectedBl, actualBl, conditionString);

         conditionString = "Verify back elevation is 2170.8126";
         expectedDbl = 2170.8126;
         actualDbl = Math.Round((double)result.back, 4);
         TestingFramework.assertEquals<double?>(expectedDbl, actualDbl, conditionString);

         conditionString = "Verify ahead elevation is 2170.8126";
         expectedDbl = 2170.8126;
         actualDbl = Math.Round((double)result.ahead, 4);
         TestingFramework.assertEquals<double?>(expectedDbl, actualDbl, conditionString);

         ///////////////////
         System.Console.Write("Test the beginning of the profile, station 10+62.50");
         conditionString = "Verify tupleNullableDoubles isSingleValue is false";
         result.back = result.ahead = 0.0;
         result.isSingleValue = false;
         aProfile.getElevation((CogoStation)1062.50, out result);
         expectedBl = false;
         actualBl = result.isSingleValue;
         TestingFramework.assertEquals<bool>(expectedBl, actualBl, conditionString);

         conditionString = "Verify back elevation is null";
         expectedDbl = null;
         actualDbl = result.back;
         TestingFramework.assertEquals<double?>(expectedDbl, actualDbl, conditionString);

         conditionString = "Verify ahead elevation is 2178.23";
         expectedDbl = 2178.23;
         actualDbl = Math.Round((double)result.ahead, 4);
         TestingFramework.assertEquals<double?>(expectedDbl, actualDbl, conditionString);
         

      }

      private static void testBuildAgenericAlignment()
      {
         anAlignment = new GenericAlignment(1000.00, 2755.23);
         anAlignment.addRegion(1822.93, 5500.00);
         anAlignment.addRegion(10000.00, 10200.00);

         // specification: 
         anAlignment.Name = "L";

         // specification: 
         secondaryAlignment = new GenericAlignment(anAlignment);
         // specification: 
         //secondaryAlignment.Parent = anAlignment;

      }

      private static void setupSomeStations()
      {
         sta1 = anAlignment.newStation(1123.00, 1);
         //Console.WriteLine(sta1);
         station2 = anAlignment.newStation(2000, 2);
         //Console.WriteLine(station2);
         stationOnOtherAlignment = secondaryAlignment.newStation(2000, 2);
      }

      private static void testStationArithmetic()
      {
         TestingFramework.assertEquals<double>(station2.trueStation, stationOnOtherAlignment.trueStation,
            "Verify two stations really refer to same alignment"); 

         var station3 = sta1 + 25.25;
         Console.WriteLine(station3);

         station3 = sta1 - 856.23;
         Console.WriteLine(station3);

         double aDistance = station2 - sta1;
         Console.WriteLine(aDistance);

         station2 -= 200;
         stationOnOtherAlignment -= 200;
         // specification:
         TestingFramework.assertEquals<double>(station2.trueStation, stationOnOtherAlignment.trueStation,
            "Two stations on diff alignments but parent related have same value after subtraction");

         station3 = anAlignment.newStation(5490.70, 2);
         Console.WriteLine(station3);
         station3 += 151;
         Console.WriteLine(station3);

         //while (station3.trueStation < 7000)
         //{
         //   station3 += 50;
         //}

         //while (station3.trueStation > 200)
         //{
         //   Console.WriteLine(station3);
         //   station3 -= 50;
         //}
      }

   }
}
