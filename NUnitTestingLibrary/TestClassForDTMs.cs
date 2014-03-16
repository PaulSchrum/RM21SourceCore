using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using ptsCogo;
using ptsDigitalTerrainModel;
using ptsCogo.Angle;
using ptsCogo.coordinates;
using System.IO;
using System.Diagnostics;

namespace NUnitTestingLibrary
{
   [TestFixture]
   class TestClassForDTMs
   {
      private ptsDTM GardenParkwayDTM;
      private ptsDTM aDTM;
      private string pathRM21SourceCode;
      private string NUnitTestingData;
      private string vrmlTestFileName;
      private string fullname;

      private TimeSpan timeToLoadGardenParkwayTinFromXML;

      //[SetUp]
      [TestFixtureSetUp]
      public void setupDTMtests()
      {
         pathRM21SourceCode = AppDomain.CurrentDomain.BaseDirectory + @"..\..\..\";
         NUnitTestingData = pathRM21SourceCode + @"NUnitTestingLibrary\TestingData\";
         vrmlTestFileName = "B4656_rm21_mesh_test.wrl";
         fullname = NUnitTestingData + vrmlTestFileName;

         // verify that the file exists


         aDTM = new ptsDTM();
         aDTM.LoadTextFile(NUnitTestingData + vrmlTestFileName);

         GardenParkwayDTM = new ptsDTM();
         var stopwatch = new Stopwatch();

         System.Console.WriteLine("Creating Garden Parkway. . .");
         stopwatch.Start();
         try { GardenParkwayDTM.LoadTextFile(NUnitTestingData + "GPEtin.xml"); }
         catch (FileNotFoundException fnf) { GardenParkwayDTM = null; }
         stopwatch.Stop();
         timeToLoadGardenParkwayTinFromXML = stopwatch.Elapsed;
         System.Console.WriteLine("time to create Garden Parkway: " + timeToLoadGardenParkwayTinFromXML.ToString());
      }
      
      [Test]
      public void TIN_GardenParkwayTests_PointOnTriangleHasCorrectValues()
      {
         if (null == GardenParkwayDTM) Assert.True(true);

         var testpoint = new ptsPoint(529790.0, 1406750.0);
         var expectedElevation = 674.9297;
         var actualElevation = GardenParkwayDTM.getElevation(testpoint);
         Assert.AreEqual(
            expected: expectedElevation, 
            actual: actualElevation, 
            delta: 0.0001, message: "Elevation");

         var expectedSlope = 7.072;
         var actualSlope = GardenParkwayDTM.getSlope(testpoint);
         Assert.AreEqual(
            expected: expectedSlope,
            actual: actualSlope,
            delta: 0.01, message: "Slope");

         var expectedAzimuth = 176.905;
         var actualAzimuth = GardenParkwayDTM.getSlopeAzimuth(testpoint);
         Assert.AreEqual(
            expected: expectedAzimuth,
            actual: actualAzimuth.getAsDegreesDouble(),
            delta: 0.0001, message: "Azimuth of Slope");

      }

      [Test]
      public void TIN_GardenParkwayTests_PointOnTriangleLineHasCorrectElevation()
      {
         if (null == GardenParkwayDTM) Assert.True(true);

         var testpoint = new ptsPoint(529666.7993, 1406618.4759);
         var expectedElevation = 673.8398;
         var actualElevation = GardenParkwayDTM.getElevation(testpoint);
         Assert.AreEqual(
            expected: expectedElevation,
            actual: actualElevation,
            delta: 0.0001, message: "Elevation");

      }

      //[Test]
      public void TIN_GardenParkwayTests_PointOnTriangleVertexHasCorrectElevation()
      {
         if (null == GardenParkwayDTM) Assert.True(true);

         var testpoint = new ptsPoint(529649.9585, 1406460.9585);
         var expectedElevation = 683.7885;
         var actualElevation = GardenParkwayDTM.getElevation(testpoint);
         Assert.AreEqual(
            expected: expectedElevation,
            actual: actualElevation,
            delta: 0.0001, message: "Elevation");

      }

      [Test]
      public void TIN_whenPointIsOutsideBoundingBox_shouldReturnNull()
      {
         double? elev;
         ptsPoint testPt = new ptsPoint(2082100.0, 74200.0, 0.0);
         elev = aDTM.getElevation(testPt);

         Assert.IsNull(elev);
      }

      [Test]
      public void TIN_whenPointIsInsideBoundingBoxAndOutsideTinHull_shouldReturnNull()
      {
         double? elev;
         ptsPoint testPt = new ptsPoint(2083000, 740648.0, 0.0);
         elev = aDTM.getElevation(testPt);

         Assert.IsNull(elev);
      }

      //[Test]
      //public void TIN_whenXYpointIsInsdeInternalTriangle_ElevationIsCorrect()
      //{
      //   double expectedDbl, actualDbl;
      //   expectedDbl = 464.223;
      //   ptsPoint testPt = new ptsPoint(2083057.836, 740822.223, 0.0);
      //   actualDbl = (Double)aDTM.getElevation(testPt);

      //   Assert.AreEqual(expected: expectedDbl, actual: actualDbl, delta: 0.0015);
      //}

      [Test]
      public void TIN_whenXYpointIsOnTheTINhull_ElevationIsCorrect()
      {
         double expectedDbl, actualDbl;
         expectedDbl = 454.504;
         ptsPoint testPt = new ptsPoint(2082985.4480, 740657.1722, 0.0);
         actualDbl = (Double)aDTM.getElevation(testPt);

         Assert.AreEqual(expected: expectedDbl, actual: actualDbl, delta: 0.0015);
      }

      [Test]
      public void TIN_whenXYpointIsOnInternalTriangleLine_ElevationIsCorrect()
      {
         double expectedDbl, actualDbl;
         expectedDbl = 464.0131;
         ptsPoint testPt = new ptsPoint(2083058.8956, 740819.1854, 0.0);
         actualDbl = (Double)aDTM.getElevation(testPt);

         Assert.AreEqual(expected: expectedDbl, actual: actualDbl, delta: 0.0015);
      }

      [Test]
      public void TIN_whenXYpointIsOnInternalTriangleVertex_ElevationIsCorrect()
      {
         double expectedDbl, actualDbl;
         expectedDbl = 482.5578;
         ptsPoint testPt = new ptsPoint(2082886.0883, 740821.373, 0.0);
         actualDbl = (Double)aDTM.getElevation(testPt);

         Assert.AreEqual(expected: expectedDbl, actual: actualDbl, delta: 0.0015);
      }

   }
}
