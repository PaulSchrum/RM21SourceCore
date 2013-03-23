using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using ptsCogo;
using ptsCogo.Horizontal;
using NUnitTestingLibrary.Mocks;
using ptsCogo.coordinates.CurvilinearCoordinates;

namespace NUnitTestingLibrary
{
   [TestFixture]
   public class TestClassForHorizontalAlignments
   {
      public List<Double> testList {get;set;}

      [SetUp]
      public void HAtestSetup()
      {

      }

      [Test]
      public void GenericAlignment_instantiateWithBeginStationOnly()
      {
         testList = new List<Double>();
         testList.Add(1000.0);
         GenericAlignment align = new GenericAlignment(testList);

         double actualDbl = align.BeginStation;
         double expectedDbl = 1000.0;

         Assert.AreEqual(expected: expectedDbl, actual: actualDbl, delta: 0.0000001);
      }

      [Test]
      public void GenericAlignment_instantiateWithBeginAndEndStation()
      {
         testList = new List<Double>();
         testList.Add(1000.0);
         testList.Add(2000.0);
         GenericAlignment align = new GenericAlignment(testList);

         bool actualBool = true;
         actualBool = actualBool && (0 == utilFunctions.tolerantCompare(align.BeginStation, 1000.0, 0.00001));
         actualBool = actualBool && (0 == utilFunctions.tolerantCompare(align.EndStation, 2000.0, 0.00001));

         Assert.AreEqual(expected: true, actual: actualBool);
      }

      [Test]
      public void HorizontalAlignment_instantiateSingleLine_fromNullFundamentalGeometry()
      {
         Assert.That(() => new rm21HorizontalAlignment(fundamentalGeometryList: (List<IRM21fundamentalGeometry>)null,
            Name: null, stationEquationing: null), Throws.Exception.TypeOf<NullReferenceException>());
      }

      private List<IRM21fundamentalGeometry> createFundmGeoms1()
      {
         List<IRM21fundamentalGeometry> fundmtlGeoms = new List<IRM21fundamentalGeometry>();

         rm21MockFundamentalGeometry mockFG = new rm21MockFundamentalGeometry();

         List<ptsPoint> ptLst = new List<ptsPoint>();
         ptLst.Add(new ptsPoint(10.0, 10.0, 0.0));
         ptLst.Add(new ptsPoint(80.7106781188, 80.7106781188, 0.0));

         mockFG.pointList = ptLst;
         mockFG.expectedType = expectedType.LineSegment;

         fundmtlGeoms.Add(mockFG);
         return fundmtlGeoms;
      }

      private List<IRM21fundamentalGeometry> createFundmGeoms_arc1()
      {
         List<IRM21fundamentalGeometry> fundmtlGeoms = new List<IRM21fundamentalGeometry>();

         rm21MockFundamentalGeometry mockFG = new rm21MockFundamentalGeometry();

         List<ptsPoint> ptLst = new List<ptsPoint>();
         ptLst.Add(new ptsPoint(443.176112, 569.321807, 0.0));
         ptLst.Add(new ptsPoint(1211.097635, -23.605404, 0.0));
         ptLst.Add(new ptsPoint(1186.397526, 946.268836, 0.0));

         mockFG.pointList = ptLst;
         mockFG.expectedType = expectedType.ArcSegmentInsideSolution;

         fundmtlGeoms.Add(mockFG);
         return fundmtlGeoms;
      }

      private List<IRM21fundamentalGeometry> createFundmGeoms_arc1_butExternalSolution()
      {
         List<IRM21fundamentalGeometry> fundmtlGeoms = new List<IRM21fundamentalGeometry>();

         rm21MockFundamentalGeometry mockFG2 = new rm21MockFundamentalGeometry();

         List<ptsPoint> ptLst = new List<ptsPoint>();
         ptLst.Add(new ptsPoint(443.176112, 569.321807, 0.0));
         ptLst.Add(new ptsPoint(1211.097635, -23.605404, 0.0));
         ptLst.Add(new ptsPoint(1186.397526, 946.268836, 0.0));

         mockFG2.pointList = ptLst;
         
         mockFG2.expectedType = expectedType.ArcSegmentOutsideSoluion;
         mockFG2.deflectionSign = -1;

         fundmtlGeoms.Add(mockFG2);
         return fundmtlGeoms;
      }

      [Test]
      public void HorizontalAlignment_ComputeDegreeOfCurveFromRadius()
      {
         Double Radius = 5729.58; Double LengthForDegreeOfCurve = 100.0;
         ptsAngle DegreeOfCurve = new ptsAngle(Radius, LengthForDegreeOfCurve);

         Double actualValue = DegreeOfCurve.getAsDegrees();
         Double expectedValue = 1.00;

         Assert.AreEqual(expected: expectedValue, actual: actualValue, delta: 0.000001);
      }

      [Test]
      public void HorizontalAlignment_instantiateSingleLine_fromFundamentalGeometry()
      {
         List<IRM21fundamentalGeometry> fundmtlGeoms = createFundmGeoms1();

         rm21HorizontalAlignment HA = new rm21HorizontalAlignment(
            fundamentalGeometryList: fundmtlGeoms,
            Name: null, stationEquationing: null);

         Assert.IsNotNull(HA);
      }

      [Test]
      public void HorizontalAlignment_instantiateSingleLine_fromFundamentalGeometry_HAlengthIs100()
      {
         List<IRM21fundamentalGeometry> fundmtlGeoms = createFundmGeoms1();

         rm21HorizontalAlignment HA = new rm21HorizontalAlignment(
            fundamentalGeometryList: fundmtlGeoms,
            Name: null, stationEquationing: null);

         double actualLength = HA.EndStation - HA.BeginStation;
         double expectedLength = 100.0;

         Assert.AreEqual(expected: expectedLength, actual: actualLength, delta: 0.00001);

      }

      [Test]
      public void HorizontalAlignment_instantiateSingleArcInterior_fromFundamentalGeometry_HAlengthIs861()
      {
         List<IRM21fundamentalGeometry> fundmtlGeoms = createFundmGeoms_arc1();

         rm21HorizontalAlignment HA = new rm21HorizontalAlignment(
            fundamentalGeometryList: fundmtlGeoms,
            Name: null, stationEquationing: null);

         double actualLength = HA.EndStation - HA.BeginStation;
         double expectedLength = 861.359280;

         Assert.AreEqual(expected: expectedLength, actual: actualLength, delta: 0.00001);

      }

      [Test]
      public void HorizontalAlignment_instantiateSingleArcExterior_fromFundamentalGeometry_HAlengthIs5235()
      {
         List<IRM21fundamentalGeometry> fundmtlGeoms = createFundmGeoms_arc1_butExternalSolution();

         rm21HorizontalAlignment HA = new rm21HorizontalAlignment(
            fundamentalGeometryList: fundmtlGeoms,
            Name: null, stationEquationing: null);

         double actualLength = HA.EndStation - HA.BeginStation;
         double expectedLength = 5234.5162;

         Assert.AreEqual(expected: expectedLength, actual: actualLength, delta: 0.00001);

      }

      [Test]
      public void HorizontalAlignment_instantiate5ItemHA_fromFundamentalGeometry_HAlengthIs7155()
      {
         List<IRM21fundamentalGeometry> fundmtlGeoms = createTestHA_fundGeom1();

         rm21HorizontalAlignment HA = new rm21HorizontalAlignment(
            fundamentalGeometryList: fundmtlGeoms,
            Name: null, stationEquationing: null);

         double actualLength = HA.EndStation - HA.BeginStation;
         double expectedLength = 7154.9385;

         Assert.AreEqual(expected: expectedLength, actual: actualLength, delta: 0.00015);

      }

      private List<IRM21fundamentalGeometry> createTestHA_fundGeom1()
      {
         // Code editors Note:  Items 4 and 5 are deliberately swapped.
         // Please do not change this.  The point of swapping them is to
         //    test the process of putting the items in the correct order.
         var returnList = new List<IRM21fundamentalGeometry>();
         var funGeomItem = new rm21MockFundamentalGeometry();

         // Line 1, Item 1
         funGeomItem.pointList.Add(new ptsPoint(3556.2226, 2526.6156, 0.0));
         funGeomItem.pointList.Add(new ptsPoint(4932.6214, 4309.2396, 0.0));
         funGeomItem.expectedType = expectedType.LineSegment;
         returnList.Add(funGeomItem);

         // Arc 1, Item 2
         funGeomItem = new rm21MockFundamentalGeometry();
         funGeomItem.pointList.Add(new ptsPoint(4932.6214, 4309.2396, 0.0));
         funGeomItem.pointList.Add(new ptsPoint(5700.5429, 3716.3124, 0.0));
         funGeomItem.pointList.Add(new ptsPoint(5675.8428, 4686.1866, 0.0));
         funGeomItem.expectedType = expectedType.ArcSegmentInsideSolution;
         returnList.Add(funGeomItem);

         // Line 2, Item 3
         funGeomItem = new rm21MockFundamentalGeometry();
         funGeomItem.pointList.Add(new ptsPoint(5675.8428, 4686.1866, 0.0));
         funGeomItem.pointList.Add(new ptsPoint(6624.6701, 4710.3507, 0.0));
         funGeomItem.expectedType = expectedType.LineSegment;
         returnList.Add(funGeomItem);

         // Line 3, Item 5
         funGeomItem = new rm21MockFundamentalGeometry();
         funGeomItem.pointList.Add(new ptsPoint(7738.3259, 5168.4199, 0.0));
         funGeomItem.pointList.Add(new ptsPoint(9093.6332, 6443.7163, 0.0));
         funGeomItem.expectedType = expectedType.LineSegment;
         returnList.Add(funGeomItem);

         // Arc 2, Item 4
         funGeomItem = new rm21MockFundamentalGeometry();
         funGeomItem.pointList.Add(new ptsPoint(6624.6701, 4710.3507, 0.0));
         funGeomItem.pointList.Add(new ptsPoint(6581.7001, 6397.6113, 0.0));
         funGeomItem.pointList.Add(new ptsPoint(7738.3259, 5168.4199, 0.0));
         funGeomItem.expectedType = expectedType.ArcSegmentInsideSolution;
         returnList.Add(funGeomItem);

         return returnList;
      }

      [Test]
      public void HorizontalAlignment_singleArcHAinsideSolutionRight_fromFundamentalGeometry_HAlengthIs666()
      {
         var funGeomItem = new rm21MockFundamentalGeometry();
         funGeomItem.pointList.Add(new ptsPoint(6903.1384, 3830.6151, 0.0));
         funGeomItem.pointList.Add(new ptsPoint(7458.9796, 3830.6151, 0.0));
         funGeomItem.pointList.Add(new ptsPoint(7257.2446, 4348.5557, 0.0));
         funGeomItem.expectedType = expectedType.ArcSegmentInsideSolution;
         funGeomItem.deflectionSign = 1;
         var fGeomList = new List<IRM21fundamentalGeometry>();
         fGeomList.Add(funGeomItem);


         rm21HorizontalAlignment HA = new rm21HorizontalAlignment(
            fundamentalGeometryList: fGeomList,
            Name: null, stationEquationing: null);

         double actualLength = HA.EndStation - HA.BeginStation;
         double expectedLength = 666.6644;

         Assert.AreEqual(expected: expectedLength, actual: actualLength, delta: 0.00015);

      }

      [Test]
      public void HorizontalAlignment_singleArcHAOutsideSolutionRight_fromFundamentalGeometry_HAlengthIs2439()
      {
         var funGeomItem = new rm21MockFundamentalGeometry();
         funGeomItem.pointList.Add(new ptsPoint(7415.7202, 4384.7704, 0.0));
         funGeomItem.pointList.Add(new ptsPoint(7458.9796, 3830.6151, 0.0));
         funGeomItem.pointList.Add(new ptsPoint(6947.3437, 3613.3867, 0.0));
         funGeomItem.expectedType = expectedType.ArcSegmentOutsideSoluion;
         funGeomItem.deflectionSign = 1;
         var fGeomList = new List<IRM21fundamentalGeometry>();
         fGeomList.Add(funGeomItem);


         rm21HorizontalAlignment HA = new rm21HorizontalAlignment(
            fundamentalGeometryList: fGeomList,
            Name: null, stationEquationing: null);

         double actualLength = HA.EndStation - HA.BeginStation;
         double expectedLength = 2439.4665;

         //Assert.AreEqual(expected: expectedLength, actual: actualLength, delta: 0.00015);

      }

      [Test]
      public void HorizontalAlignment_singleArcHAInsideSolutionLeft_fromFundamentalGeometry_HAlengthIs1051()
      {
         var funGeomItem = new rm21MockFundamentalGeometry();
         funGeomItem.pointList.Add(new ptsPoint(4003.3849, 4491.7185, 0.0));
         funGeomItem.pointList.Add(new ptsPoint(3995.0953, 5346.2102, 0.0));
         funGeomItem.pointList.Add(new ptsPoint(4803.1466, 5068.2214, 0.0));
         funGeomItem.expectedType = expectedType.ArcSegmentInsideSolution;
         funGeomItem.deflectionSign = -1;
         var fGeomList = new List<IRM21fundamentalGeometry>();
         fGeomList.Add(funGeomItem);


         rm21HorizontalAlignment HA = new rm21HorizontalAlignment(
            fundamentalGeometryList: fGeomList,
            Name: null, stationEquationing: null);

         double actualLength = HA.EndStation - HA.BeginStation;
         double expectedLength = 1050.8644;

         Assert.AreEqual(expected: expectedLength, actual: actualLength, delta: 0.00015);

      }

      [Test]
      public void HorizontalAlignment_singleArcHAOutsideSolutionLeft_fromFundamentalGeometry_HAlengthIs4038()
      {
         var funGeomItem = new rm21MockFundamentalGeometry();
         funGeomItem.pointList.Add(new ptsPoint(4837.3905, 5202.1148, 0.0));
         funGeomItem.pointList.Add(new ptsPoint(3995.0953, 5346.2102, 0.0));
         funGeomItem.pointList.Add(new ptsPoint(3861.9225, 4502.1191, 0.0));
         funGeomItem.expectedType = expectedType.ArcSegmentOutsideSoluion;
         funGeomItem.deflectionSign = -1;
         var fGeomList = new List<IRM21fundamentalGeometry>();
         fGeomList.Add(funGeomItem);


         rm21HorizontalAlignment HA = new rm21HorizontalAlignment(
            fundamentalGeometryList: fGeomList,
            Name: null, stationEquationing: null);

         double actualLength = HA.EndStation - HA.BeginStation;
         double expectedLength = 4037.9558;

         //Assert.AreEqual(expected: expectedLength, actual: actualLength, delta: 0.00015);

      }

      [Test]
      public void HorizontalAlignment_singleArcHAOutsideSolutionLeft_fromFundamentalGeometry_HAlengthIs3396()
      {
         var funGeomItem = new rm21MockFundamentalGeometry();
         funGeomItem.pointList.Add(new ptsPoint(6925.6663, 6218.7689, 0.0));
         funGeomItem.pointList.Add(new ptsPoint(6540.7903, 5615.8802, 0.0));
         funGeomItem.pointList.Add(new ptsPoint(5952.2191, 6022.3138, 0.0));
         funGeomItem.expectedType = expectedType.ArcSegmentOutsideSoluion;
         funGeomItem.deflectionSign = 1;
         var fGeomList = new List<IRM21fundamentalGeometry>();
         fGeomList.Add(funGeomItem);


         rm21HorizontalAlignment HA = new rm21HorizontalAlignment(
            fundamentalGeometryList: fGeomList,
            Name: null, stationEquationing: null);

         double actualLength = HA.EndStation - HA.BeginStation;
         double expectedLength = 3396.4881;

         //Assert.AreEqual(expected: expectedLength, actual: actualLength, delta: 0.0045);

      }

      [Test]
      public void angleNormalization_withinPlusOrMinus2Pi_OverPositive2PI()
      {
         Double angleNeedingToBeNormalized = 2 * Math.PI * 4.56;
         Double expectedAfterNormalized = 2 * Math.PI * 0.56;
         ptsAngle anAngle = new ptsAngle();
         Double actualAfterNormalization =
            ptsAngle.ComputeRemainderScaledByDenominator(angleNeedingToBeNormalized, 2 * Math.PI);

         Assert.AreEqual(expected: expectedAfterNormalized,
            actual: actualAfterNormalization, delta: 0.0000001);
      }

      [Test]
      public void angleNormalization_withinPlusOrMinus2Pi_UnderNegative2PI()
      {
         Double angleNeedingToBeNormalized = -710.0;
         Double expectedAfterNormalized = -350.0;
         ptsAngle anAngle = new ptsAngle();
         Double actualAfterNormalization =
            ptsAngle.ComputeRemainderScaledByDenominator(angleNeedingToBeNormalized, 360.0);

         Assert.AreEqual(expected: expectedAfterNormalized,
            actual: actualAfterNormalization, delta: 0.0000001);
      }

      [Test]
      public void HorizontalAlignment_givenXYvalues_getStationOffsetValues()
      {
         List<IRM21fundamentalGeometry> fundmtlGeoms = createTestHA_fundGeom1();

         rm21HorizontalAlignment HA = new rm21HorizontalAlignment(
            fundamentalGeometryList: fundmtlGeoms,
            Name: null, stationEquationing: null);

         StationOffsetElevation theSOE= new StationOffsetElevation(801.8849, 0.0, 0.0);
         StationOffsetElevation anSOE = null;
         bool allValuesAgree = true;

         // test point on first tangent
         ptsPoint somePoint = new ptsPoint(4046.2915, 3161.3216, 0.0);
         var soePoints = HA.getStationOffsetElevation(somePoint);
         if (soePoints != null && soePoints.Count > 0)
         {
            anSOE = soePoints.FirstOrDefault();
            allValuesAgree &= anSOE.station.tolerantEquals(801.8849, 0.00014);
            allValuesAgree &= anSOE.offset.OFST.tolerantEquals(0.0, 0.00014);
            allValuesAgree &= anSOE.elevation.EL.tolerantEquals(0.0, 0.00000001);
         }
         else
         {
            allValuesAgree = false;
         }

         // test point which is before the beginning of the HA
         somePoint = new ptsPoint(2500.0, 1000.0, 0.0);
         soePoints = HA.getStationOffsetElevation(somePoint);
         allValuesAgree &= soePoints.Count == 0;

         // test point which is beyond the end of the HA
         somePoint = new ptsPoint(9554.0, 9000.0, 0.0);
         soePoints = HA.getStationOffsetElevation(somePoint);
         allValuesAgree &= soePoints.Count == 0;

         // test point offset from first tangent
         somePoint.x = 4516.0; somePoint.y = 3404.0;
         soePoints = HA.getStationOffsetElevation(somePoint);
         if (soePoints != null && soePoints.Count > 0)
         {
            anSOE = soePoints.FirstOrDefault();
            allValuesAgree &= anSOE.station.tolerantEquals(1281.0297, 0.00014);
            allValuesAgree &= anSOE.offset.OFST.tolerantEquals(223.4706, 0.00014);
            allValuesAgree &= anSOE.elevation.EL.tolerantEquals(0.0, 0.000001);
         }
         else
         {
            allValuesAgree = false;
         }

         // test point on first arc
         somePoint.x = 5494.3772; somePoint.y = 4664.3429;
         soePoints = HA.getStationOffsetElevation(somePoint);
         if (soePoints != null && soePoints.Count > 0)
         {
            anSOE = soePoints.FirstOrDefault();
            allValuesAgree &= anSOE.station.tolerantEquals(2930.4718, 0.00014);
            allValuesAgree &= anSOE.offset.OFST.tolerantEquals(0.0, 0.00014);
            allValuesAgree &= anSOE.elevation.EL.tolerantEquals(0.0, 0.000001);
         }
         else
         {
            allValuesAgree = false;
         }

         // test point offset right of second arc
         somePoint.x = 6918.0; somePoint.y = 4557.0;
         soePoints = HA.getStationOffsetElevation(somePoint);
         if (soePoints != null && soePoints.Count > 0)
         {
            anSOE = soePoints.FirstOrDefault();
            allValuesAgree &= anSOE.station.tolerantEquals(4324.6956, 0.00014);
            allValuesAgree &= anSOE.offset.OFST.tolerantEquals(183.2743, 0.00014);
            allValuesAgree &= anSOE.elevation.EL.tolerantEquals(0.0, 0.000001);
         }
         else
         {
            allValuesAgree = false;
         }

         // test point offset left of second arc
         somePoint.x = 7103.0; somePoint.y = 4979.0;
         soePoints = HA.getStationOffsetElevation(somePoint);
         if (soePoints != null && soePoints.Count > 0)
         {
            anSOE = soePoints.FirstOrDefault();
            allValuesAgree &= anSOE.station.tolerantEquals(4614.0481, 0.00014);
            allValuesAgree &= anSOE.offset.OFST.tolerantEquals(-176.4468, 0.00014);
            allValuesAgree &= anSOE.elevation.EL.tolerantEquals(0.0, 0.000001);
         }
         else
         {
            allValuesAgree = false;
         }

         // test point offset left of third line (fifth segment
         somePoint.x = 8071.7032; somePoint.y = 5913.8278;
         soePoints = HA.getStationOffsetElevation(somePoint);
         if (soePoints != null && soePoints.Count > 0)
         {
            anSOE = soePoints.FirstOrDefault();
            allValuesAgree &= anSOE.station.tolerantEquals(6047.5668, 0.00014);
            allValuesAgree &= anSOE.offset.OFST.tolerantEquals(-314.4057, 0.00014);
            allValuesAgree &= anSOE.elevation.EL.tolerantEquals(0.0, 0.000001);
         }
         else
         {
            allValuesAgree = false;
         }
         
         Assert.IsTrue(allValuesAgree);
      }

   }
}
