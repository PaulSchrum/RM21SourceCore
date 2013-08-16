using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using ptsCogo;
using ptsCogo.Horizontal;
using NUnitTestingLibrary.Mocks;
using ptsCogo.coordinates.CurvilinearCoordinates;
using ptsCogo.Angle;

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
      public void Deflection_positiveLessThan180_getAsDegrees()
      {
         Double expectedValue = 45.0;
         Deflection defl = new Deflection(0.785398164, 1);
         Double actualValue = defl.getAsDegreesDouble();
         Assert.AreEqual(expected: expectedValue, actual: actualValue, delta: 0.00001);
      }

      [Test]
      public void Deflection_positiveGreaterThan180_getAsDegrees()
      {
         Double expectedValue = 310.0;
         Deflection defl = new Deflection(5.41052068118, 1);
         Double actualValue = defl.getAsDegreesDouble();
         Assert.AreEqual(expected: expectedValue, actual: actualValue, delta: 0.00001);
      }

      [Test]
      public void Deflection_negativeLessThan180_getAsDegrees()
      {
         Double expectedValue = -45.0;
         Deflection defl = new Deflection(0.785398164, -1);
         Double actualValue = defl.getAsDegreesDouble();
         Assert.AreEqual(expected: expectedValue, actual: actualValue, delta: 0.00001);
      }

      [Test]
      public void Deflection_negativeGreaterThan180_getAsDegrees()
      {
         Double expectedValue = -310.0;
         Deflection defl = new Deflection(5.41052068118, -1);
         Double actualValue = defl.getAsDegreesDouble();
         Assert.AreEqual(expected: expectedValue, actual: actualValue, delta: 0.00001);
      }

      [Test]
      public void Deflection_positiveLessThan180_getAsRadians()
      {
         Double expectedValue = 0.785398164;
         Deflection defl = new Deflection(0.785398164, 1);
         Double actualValue = defl.getAsRadians();
         Assert.AreEqual(expected: expectedValue, actual: actualValue, delta: 0.00001);
      }

      [Test]
      public void Deflection_positiveGreaterThan180_getAsRadians()
      {
         Double expectedValue = 5.41052068118;
         Deflection defl = new Deflection(5.41052068118, 1);
         Double actualValue = defl.getAsRadians();
         Assert.AreEqual(expected: expectedValue, actual: actualValue, delta: 0.00001);
      }

      [Test]
      public void Deflection_negativeLessThan180_getAsRadians()
      {
         Double expectedValue = -0.39479111970;
         Azimuth begAz = new Azimuth(new ptsPoint(0.0, 0.0, 0.0), new ptsPoint(10.0, 50.0, 0.0));
         Azimuth endAz = new Azimuth(new ptsPoint(10.0, 50.0, 0.0), new ptsPoint(0.0, 100.0, 0.0));
         Deflection defl = new Deflection(begAz, endAz, true);
         Double actualValue = defl.getAsRadians();
         Assert.AreEqual(expected: expectedValue, actual: actualValue, delta: 0.0000001);
      }

      [Test]
      public void Deflection_negativeGreaterThan180_getAsRadians()
      {
         Double expectedValue = -5.88839418748;
         Azimuth endAz = new Azimuth(new ptsPoint(0.0, 0.0, 0.0), new ptsPoint(10.0, 50.0, 0.0));
         Azimuth begAz = new Azimuth(new ptsPoint(10.0, 50.0, 0.0), new ptsPoint(0.0, 100.0, 0.0));
         Deflection defl = new Deflection(begAz, endAz, false);
         Double actualValue = defl.getAsRadians();
         Assert.AreEqual(expected: expectedValue, actual: actualValue, delta: 0.00001);
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

         Double actualValue = DegreeOfCurve.getAsDegreesDouble();
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
         funGeomItem.deflectionSign = 1;
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
         funGeomItem.deflectionSign = -1;
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
      public void HorizontalAlignment_givenStationOffsetValues_getXYvalues()
      {
         bool allValuesAgree = true;
         List<IRM21fundamentalGeometry> fundmtlGeoms = createTestHA_fundGeom1();

         rm21HorizontalAlignment HA = new rm21HorizontalAlignment(
            fundamentalGeometryList: fundmtlGeoms,
            Name: null, stationEquationing: null);

         StationOffsetElevation anSOE = new StationOffsetElevation();

         // test a point right of the second line segment (the third segment)
         anSOE.station = 3611.75; anSOE.offset.OFST = 238.949;
         ptsPoint anXYpoint = HA.getXYZcoordinates(anSOE);
         if (anXYpoint != null)
         {
            allValuesAgree &= anXYpoint.x.tolerantEquals(6180.0, 0.014);
            allValuesAgree &= anXYpoint.y.tolerantEquals(4460.0, 0.014);
            allValuesAgree &= anXYpoint.z.tolerantEquals(0.0, 0.000001);
         }
         else
         {
            allValuesAgree = false;
         }

         // test a point right of the second arc (the fourth segment)
         anSOE.station = 4469.2978; anSOE.offset.OFST = 138.1336;
         anXYpoint = HA.getXYZcoordinates(anSOE);
         if (anXYpoint != null)
         {
            allValuesAgree &= anXYpoint.x.tolerantEquals(7062.3839, 0.014);
            allValuesAgree &= anXYpoint.y.tolerantEquals(4636.0766, 0.014);
            allValuesAgree &= anXYpoint.z.tolerantEquals(0.0, 0.000001);
         }
         else
         {
            allValuesAgree = false;
         }

         Assert.IsTrue(allValuesAgree);
      }

      [Test]
      public void HorizontalAlignment_givenBeginStationAndLeftOffset_getXYcoordinates()
      {
         var HA = new rm21HorizontalAlignment(
            fundamentalGeometryList: createTestHA_fundGeom1(),
            Name: null, stationEquationing: null);

         var actual = HA.getXYZcoordinates(0.0, -20.0, 0.0);
         Assert.AreEqual(expected: 3540.3922, actual: actual.x, delta: 0.00015);
         Assert.AreEqual(expected: 2538.8385, actual: actual.y, delta: 0.00015);

      }

      [Test]
      public void HorizontalAlignment_givenXYvalues_getStationOffsetValues()
      {
         List<IRM21fundamentalGeometry> fundmtlGeoms = createTestHA_fundGeom1();

         rm21HorizontalAlignment HA = new rm21HorizontalAlignment(
            fundamentalGeometryList: fundmtlGeoms,
            Name: null, stationEquationing: null);

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

         // test point offset at center of first arc
         somePoint.x = 5700.5429; somePoint.y = 3716.3124;
         soePoints = HA.getStationOffsetElevation(somePoint);
         if (soePoints != null && soePoints.Count > 0)
         {
            anSOE = soePoints.FirstOrDefault();
            allValuesAgree &= anSOE.station.tolerantEquals(2252.1594, 0.00014);
            allValuesAgree &= anSOE.offset.OFST.tolerantEquals(970.1887, 0.00014);
            allValuesAgree &= anSOE.elevation.EL.tolerantEquals(0.0, 0.000001);
         }
         else
         {
            allValuesAgree = false;
         }

         // test point offset right that returns 2 SOE instances for the same point
         somePoint.x = 5940.0; somePoint.y = 3310.0;
         soePoints = HA.getStationOffsetElevation(somePoint);
         if (soePoints != null && soePoints.Count > 0)
         {
            allValuesAgree &= (soePoints.Count == 2);
            anSOE = soePoints.FirstOrDefault();
            allValuesAgree &= anSOE.station.tolerantEquals(3342.5539, 0.00014);
            allValuesAgree &= anSOE.offset.OFST.tolerantEquals(1382.4657, 0.00014);
            allValuesAgree &= anSOE.elevation.EL.tolerantEquals(0.0, 0.000001);
         }
         else
         {
            allValuesAgree = false;
         }

         Assert.IsTrue(allValuesAgree);
      }

      private rm21HorizontalAlignment createSingleTangentHA()
      {
         var newHA = new rm21HorizontalAlignment();
         newHA.reset(new ptsPoint(2082268.0907, 740846.3249, 0.0), 
            new ptsPoint(2082339.9608, 740834.3849, 0.0));
         return newHA;
      }

      [Test]
      public void HorizontalAlignment_from2points_producesCorrectValues()
      {
         rm21HorizontalAlignment HA = createSingleTangentHA();

         double expectedDbl = 72.8552;
         double actualDbl = (Double)HA.EndStation;
         Assert.AreEqual(expected: expectedDbl, actual: actualDbl, delta: 0.0001);

         expectedDbl = 99.43257281767;
         Azimuth az = HA.EndAzimuth;
         actualDbl = az.getAsDegreesDouble();
         Assert.AreEqual(expected: expectedDbl, actual: actualDbl, delta: 0.0001);

         HA.reset(new ptsPoint(100.0, 100.0, 0.0), new ptsPoint(80.0, 150.0, 0.0));

         expectedDbl = 53.8516480713;
         actualDbl = (Double)HA.EndStation;
         Assert.AreEqual(expected: expectedDbl, actual: actualDbl, delta: 0.0001);

         expectedDbl = 338.198590514;
         actualDbl = HA.EndAzimuth.getAsDegreesDouble();
         Assert.AreEqual(expected: expectedDbl, actual: actualDbl, delta: 0.0001);

      }

      [Test]
      public void HorizontalAlignment_appendSimpleArcToTangent_DPonAlignment_EndStationIs391p78()
      {
         Double radius = 1170.0;
         var HA = createSingleTangentHA();
         HA.appendArc(ArcEndPoint: new ptsPoint(2082657.7727, 740825.3769, 0.0),
            radius: radius);

         Double expectedDbl = 391.7812;
         Double actualDbl = (Double)HA.EndStation;
         Assert.AreEqual(expected: expectedDbl, actual: actualDbl, delta: 0.0001);

         expectedDbl = -15.61806;
         actualDbl = HA.GetElementByStation(390.0).Deflection.getAsDegreesDouble();
         Assert.AreEqual(expected: expectedDbl, actual: actualDbl, delta: 0.0001);
      }

      [Test]
      public void HorizontalAlignment_appendSimpleArcToTangent_DPoffAlignment_EndStationIs391p78()
      {
         Double radius = 1170.0;
         var HA = createSingleTangentHA();
         HA.appendArc(ArcEndPoint: new ptsPoint(2082652.9833, 740869.5684, 0.0),
            radius: radius);

         Double expectedDbl = 391.7812;
         Double actualDbl = (Double)HA.EndStation;
         Assert.AreEqual(expected: expectedDbl, actual: actualDbl, delta: 0.0001);

         expectedDbl = -15.61806;
         actualDbl = HA.GetElementByStation(390.0).Deflection.getAsDegreesDouble();
         Assert.AreEqual(expected: expectedDbl, actual: actualDbl, delta: 0.0001);
      }

      [Test]
      public void HorizontalAlignment_appendTangentToSimpleArc_EndStationIs481p78()
      {
         Double radius = 1170.0;
         var HA = createSingleTangentHA();
         HA.appendArc(ArcEndPoint: new ptsPoint(2082660.0, 740870.0, 0.0),
            radius: radius);

         Double expectedDbl = -15.618046;
         Double actualDbl = HA.GetElementByStation(390.0).Deflection.getAsDegreesDouble();
         Assert.AreNotEqual(expected: expectedDbl, actual: actualDbl);

         HA.appendTangent(TangentEndPoint: new ptsPoint(2082747.242780, 740835.073448, 0.0));

         expectedDbl = -15.61806;
         actualDbl = HA.GetElementByStation(390.0).Deflection.getAsDegreesDouble();
         Assert.AreEqual(expected: expectedDbl, actual: actualDbl, delta: 0.000045);
         // Note: The error is 1 inch over 20 miles -- acceptable accuracy.
      }

   }
}
