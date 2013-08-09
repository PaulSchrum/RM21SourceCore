using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using ptsCogo;
using ptsDigitalTerrainModel;
using ptsCogo.Angle;
using ptsCogo.coordinates;


namespace NUnitTestingLibrary
{
   [TestFixture]
   class TestClassForDTMs
   {
      private ptsDTM aDTM;
      private string pathRM21SourceCode;
      private string NUnitTestingData;
      private string vrmlTestFileName;
      private string fullname;

      [SetUp]
      public void setupDTMtests()
      {
         pathRM21SourceCode = AppDomain.CurrentDomain.BaseDirectory + @"..\..\..\";
         NUnitTestingData = pathRM21SourceCode + @"NUnitTestingLibrary\TestingData\";
         vrmlTestFileName = "B4656_rm21_mesh_test.wrl";
         fullname = NUnitTestingData + vrmlTestFileName;

         // verify that the file exists


         aDTM = new ptsDTM();
         aDTM.LoadTextFile(NUnitTestingData + vrmlTestFileName);
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
