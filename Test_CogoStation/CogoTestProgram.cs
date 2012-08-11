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
         Console.Read();
      }

      private static void testCogoProfile()
      {
         throw new NotImplementedException();
      }

      //private static void testCogoProfile()
      //{
        // ptsCogo.CogoProfile aProfile = new ptsCogo.CogoProfile();
      //}

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
