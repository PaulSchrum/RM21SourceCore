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
         testBuildAgenericAlignment();
         setupSomeStations();
         testStationArithmetic();
         testCogoProfile();
         Console.WriteLine("Testing Concluded.");
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
         System.Console.WriteLine("Test on a vertical curve, station 11+20");
         conditionString = "Verify tupleNullableDoubles isSingleValue is true when getting elevation";
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

         expectedDbl = 2174.9175;
         conditionString = "Verify ahead elevation is " + expectedDbl.ToString();
         actualDbl = Math.Round((double)result.ahead, 4);
         TestingFramework.assertEquals<double?>(expectedDbl, actualDbl, conditionString);

         conditionString = "Verify tupleNullableDoubles isSingleValue is true when getting slope";
         aProfile.getSlope((CogoStation)1120.00, out result);
         expectedBl = true;
         actualBl = result.isSingleValue;
         TestingFramework.assertEquals<bool>(expectedBl, actualBl, conditionString);

         conditionString = "Verify back slope is -4.1182%";
         expectedDbl = -0.041182;
         actualDbl = Math.Round((double)result.back, 6);
         TestingFramework.assertEquals<double?>(expectedDbl, actualDbl, conditionString);

         conditionString = "Verify ahead slope is -4.1182%";
         expectedDbl = -0.041182;
         actualDbl = Math.Round((double)result.ahead, 6);
         TestingFramework.assertEquals<double?>(expectedDbl, actualDbl, conditionString);

         conditionString = "Verify tupleNullableDoubles isSingleValue is true when getting K Value";
         aProfile.getKvalue((CogoStation)1120.00, out result);
         expectedBl = true;
         actualBl = result.isSingleValue;
         TestingFramework.assertEquals<bool>(expectedBl, actualBl, conditionString);

         conditionString = "Verify back K value is +17.5";
         expectedDbl = 17.5;
         actualDbl = Math.Round((double)result.back, 1);
         TestingFramework.assertEquals<double?>(expectedDbl, actualDbl, conditionString);

         conditionString = "Verify ahead K value is +17.5";
         expectedDbl = 17.5;
         actualDbl = Math.Round((double)result.ahead, 1);
         TestingFramework.assertEquals<double?>(expectedDbl, actualDbl, conditionString);

         ///////////////////
         System.Console.WriteLine("Test on a vertical tangent, station 12+65");
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

         aProfile.getSlope((CogoStation)1265.00, out result);
         conditionString = "Verify back slope is -5.1721%";
         expectedDbl = -0.051721;
         actualDbl = Math.Round((double)result.back, 6);
         TestingFramework.assertEquals<double?>(expectedDbl, actualDbl, conditionString);

         conditionString = "Verify ahead slope is -5.1721%";
         expectedDbl = -0.051721;
         actualDbl = Math.Round((double)result.ahead, 6);
         TestingFramework.assertEquals<double?>(expectedDbl, actualDbl, conditionString);

         System.Console.WriteLine("Test K value a vertical tangent, station 12+65");
         aProfile.getKvalue((CogoStation)1265.00, out result);
         conditionString = "Verify back K Value is infintiy";
         expectedDbl = double.PositiveInfinity;
         actualDbl = result.back;
         TestingFramework.assertEquals<double?>(expectedDbl, actualDbl, conditionString);

         conditionString = "Verify ahead slope is infintiy";
         expectedDbl = double.PositiveInfinity;
         actualDbl = result.ahead;
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

         aProfile.getSlope((CogoStation)1062.50, out result);
         expectedBl = false;
         actualBl = result.isSingleValue;
         TestingFramework.assertEquals<bool>(expectedBl, actualBl, conditionString);

         conditionString = "Verify back slope is null";
         expectedDbl = null;
         actualDbl = result.back;
         TestingFramework.assertEquals<double?>(expectedDbl, actualDbl, conditionString);

         conditionString = "Verify ahead slope is -7.4035%";
         expectedDbl = -0.074035;
         actualDbl = Math.Round((double)result.ahead, 6);
         TestingFramework.assertEquals<double?>(expectedDbl, actualDbl, conditionString);

         ///////////////////
         System.Console.Write("Test the end of the profile, station 13+65");
         conditionString = "Verify tupleNullableDoubles isSingleValue is false";
         result.back = result.ahead = 0.0;
         result.isSingleValue = false;
         aProfile.getElevation((CogoStation)1365.0, out result);
         expectedBl = false;
         actualBl = result.isSingleValue;
         TestingFramework.assertEquals<bool>(expectedBl, actualBl, conditionString);

         conditionString = "Verify back elevation is 2167.8765";
         expectedDbl = 2167.8765;
         actualDbl = Math.Round((double)result.back, 4);
         TestingFramework.assertEquals<double?>(expectedDbl, actualDbl, conditionString);

         conditionString = "Verify ahead elevation is null";
         expectedDbl = null;
         actualDbl = result.ahead;
         TestingFramework.assertEquals<double?>(expectedDbl, actualDbl, conditionString);

         ///////////////////
         System.Console.Write("Test the vertical PRC station 11+75.50");
         conditionString = "Verify tupleNullableDoubles isSingleValue is true for slopes";
         result.back = result.ahead = 0.0;
         result.isSingleValue = true;
         aProfile.getSlope((CogoStation)1177.5, out result);
         expectedBl = true;
         actualBl = result.isSingleValue;
         TestingFramework.assertEquals<bool>(expectedBl, actualBl, conditionString);

         conditionString = "Verify back slope is -0.8330%";
         expectedDbl = -0.00833;
         actualDbl = Math.Round((double)result.back, 6);
         TestingFramework.assertEquals<double?>(expectedDbl, actualDbl, conditionString);

         conditionString = "Verify ahead slope is -0.8330%";
         expectedDbl = -0.00833;
         actualDbl = Math.Round((double)result.ahead, 6);
         TestingFramework.assertEquals<double?>(expectedDbl, actualDbl, conditionString);

         conditionString = "Verify tupleNullableDoubles isSingleValue is false for K Values";
         result.back = result.ahead = 0.0;
         result.isSingleValue = false;
         aProfile.getKvalue((CogoStation)1177.5, out result);
         expectedBl = false;
         actualBl = result.isSingleValue;
         TestingFramework.assertEquals<bool>(expectedBl, actualBl, conditionString);

         conditionString = "Verify back K value is 17.5";
         expectedDbl = 17.5;
         actualDbl = Math.Round((double)result.back, 1);
         TestingFramework.assertEquals<double?>(expectedDbl, actualDbl, conditionString);

         conditionString = "Verify ahead K value is -19.6";
         expectedDbl = -19.6;
         actualDbl = Math.Round((double)result.ahead, 1);
         TestingFramework.assertEquals<double?>(expectedDbl, actualDbl, conditionString);

         conditionString = "Verify we know we are not on a PINC at station 11+77.50";
         expectedBl = false;
         actualBl = aProfile.isOnPINC((CogoStation)1177.50);
         TestingFramework.assertEquals<bool>(expectedBl, actualBl, conditionString);

         aVpiList = new vpiList();
         aVpiList.add(1000.00, 100.0);
         aVpiList.add(1100.00, 110.0);
         aVpiList.add(1200.00, 102.0);

         aProfile = new Profile(aVpiList);

         ///////////////////
         System.Console.WriteLine("Test the elevation, slope, and K values at a VPI with no VC.");
         conditionString = "Verify tupleNullableDoubles isSingleValue is true for elevation";
         result.back = result.ahead = 0.0;
         result.isSingleValue = true;
         aProfile.getElevation((CogoStation)1100.0, out result);
         expectedBl = true;
         actualBl = result.isSingleValue;
         TestingFramework.assertEquals<bool>(expectedBl, actualBl, conditionString);

         conditionString = "Verify we know we are on a PINC at station 11+00";
         expectedBl = true;
         actualBl = aProfile.isOnPINC((CogoStation)1100.0);
         TestingFramework.assertEquals<bool>(expectedBl, actualBl, conditionString);

         conditionString = "Verify we know we are not on a PINC at station 11+50";
         expectedBl = false;
         actualBl = aProfile.isOnPINC((CogoStation)1150.0);
         TestingFramework.assertEquals<bool>(expectedBl, actualBl, conditionString);

         conditionString = "Verify back elevation is 110.0";
         expectedDbl = 110.0;
         actualDbl = Math.Round((double)result.back, 6);
         TestingFramework.assertEquals<double?>(expectedDbl, actualDbl, conditionString);

         conditionString = "Verify ahead elevation is 110.0";
         expectedDbl = 110.0;
         actualDbl = Math.Round((double)result.ahead, 6);
         TestingFramework.assertEquals<double?>(expectedDbl, actualDbl, conditionString);

         conditionString = "Verify tupleNullableDoubles isSingleValue is false for slope";
         aProfile.getSlope((CogoStation)1100.0, out result);
         expectedBl = false;
         actualBl = result.isSingleValue;
         TestingFramework.assertEquals<bool>(expectedBl, actualBl, conditionString);

         conditionString = "Verify back slope is +10.0%";
         expectedDbl = 0.10;
         actualDbl = Math.Round((double)result.back, 6);
         TestingFramework.assertEquals<double?>(expectedDbl, actualDbl, conditionString);

         conditionString = "Verify ahead slope is -8.0%";
         expectedDbl = -0.080;
         actualDbl = Math.Round((double)result.ahead, 6);
         TestingFramework.assertEquals<double?>(expectedDbl, actualDbl, conditionString);

         conditionString = "Verify tupleNullableDoubles isSingleValue is true for K value";
         aProfile.getKvalue((CogoStation)1100.0, out result);
         expectedBl = true;
         actualBl = result.isSingleValue;
         TestingFramework.assertEquals<bool>(expectedBl, actualBl, conditionString);

         conditionString = "Verify back K value is 0.0";
         expectedDbl = 0.0;
         actualDbl = result.back;
         TestingFramework.assertEquals<double?>(expectedDbl, actualDbl, conditionString);

         conditionString = "Verify ahead K value is 0.0";
         expectedDbl = 0.0;
         actualDbl = result.ahead;
         TestingFramework.assertEquals<double?>(expectedDbl, actualDbl, conditionString);


         aVpiList = new vpiList();
         aVpiList.add(1000.00, 12.0);
         aVpiList.add(1100.00, 12.0);

         aProfile = new Profile(aVpiList);

         System.Console.WriteLine("Test adding a VPINC to a profile past the end.");
         aProfile.addStationAndElevation((CogoStation)1200.00, 14.0);
         conditionString = "Verify elevation == 13.0";
         aProfile.getElevation((CogoStation)1150.0, out result);
         expectedDbl = 13.0;
         actualDbl = result.ahead;
         TestingFramework.assertEquals<double?>(expectedDbl, actualDbl, conditionString);

         System.Console.WriteLine("Test adding a VPINC to a profile before the beginning.");
         aProfile.addStationAndElevation((CogoStation)900.00, 14.0);
         conditionString = "Verify elevation == 13.0";
         aProfile.getElevation((CogoStation)950.0, out result);
         expectedDbl = 13.0;
         actualDbl = result.ahead;
         TestingFramework.assertEquals<double?>(expectedDbl, actualDbl, conditionString);

         System.Console.WriteLine("Test adding a VPINC to the interior of a profile.");
         aProfile.addStationAndElevation((CogoStation)1050.00, -8.0);
         conditionString = "Verify elevation == -8.0 at 10+50";
         aProfile.getElevation((CogoStation)1050.0, out result);
         expectedDbl = -8.0;
         actualDbl = result.ahead;
         TestingFramework.assertEquals<double?>(expectedDbl, actualDbl, conditionString);

         conditionString = "Verify elevation == +2.0 at 10+25";
         aProfile.getElevation((CogoStation)1025.0, out result);
         expectedDbl = 2.0;
         actualDbl = result.ahead;
         TestingFramework.assertEquals<double?>(expectedDbl, actualDbl, conditionString);

         conditionString = "Verify elevation == +2.0 at 10+75";
         aProfile.getElevation((CogoStation)1075.0, out result);
         expectedDbl = 2.0;
         actualDbl = result.ahead;
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
