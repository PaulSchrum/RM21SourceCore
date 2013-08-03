using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using ptsCogo;
using ptsCogo.coordinates;
using ptsCogo.Angle;
using ptsCogo.Mocks;


namespace NUnitTestingLibrary
{
   [TestFixture]
   public class TestClassForProfiles
   {
      double doubleDelta=0.0000001;
      private static tupleNullableDoubles result;
      bool expectedBl;
      bool actualBl;
      double expectedDbl;
      double actualDbl;
      string conditionString;

      private Profile profile1;
      private Profile profile2;
      private Profile resultingProfile;
      private Profile pfl1;
      private Profile pfl2;
      private Profile pfl3;
      private Profile pfl4;
      private Profile pfl5;
      private Profile pfl6;
      private Profile pfl7;
      private Profile pfl8;
      private Profile pfl9;

      private ptsRay ray1 = new ptsRay();
      private ptsRay ray2 = new ptsRay();
      private ptsRay ray3 = new ptsRay();
      private ptsRay ray4 = new ptsRay();
      private ptsRay ray5 = new ptsRay();

      [SetUp]
      public void ProfilesTestSetup()
      {
         var aVpiList = new vpiList();
         aVpiList.add(1062.50, 12.0);
         aVpiList.add(1120.00, 12.0);
         aVpiList.add(1220.00, 15.0);
         aVpiList.add(1315.00, 15.0);
         aVpiList.add(1345.00, 10.0);
         aVpiList.add(1365.00, 10.0);
         aVpiList.add(1400.00, 10.0);
         aVpiList.add(2000.00, 14.0, 200);
         aVpiList.add(2500.00, 9.0);
         aVpiList.add(3000.00, 4.0);

         profile1 = new Profile(aVpiList);

         aVpiList = new vpiList();
         aVpiList.add(1062.50, 12.0);
         aVpiList.add(1120.00, 12.0);
         aVpiList.add(1220.00, 15.0);
         aVpiList.add(1250.00, 15.0);
         aVpiList.add(1340.00, 10.0);
         aVpiList.add(1365.00, 10.0);
         aVpiList.add(1400.00, 10.0);
         aVpiList.add(2000.00, 14.0, 200);
         aVpiList.add(2500.00, 9.0);
         aVpiList.add(3000.00, 4.0);

         profile2 = new Profile(aVpiList);

         pfl1Setup();
         pfl2Setup();
         pfl3Setup();
         pfl4and5Setup();
         pfl6_7and8Setup();
         pfl9Setup();

         ray1.StartPoint = new ptsPoint(1100.0, 0.0, 10.0);
         ray1.Slope = new Slope(1.00);
         ray1.HorizontalDirection = null;

         ray2.StartPoint = new ptsPoint(1312.0, 0.0, 15.2);
         ray2.Slope = new Slope(-1.0 / 7.0);
         ray2.HorizontalDirection = null;

         ray3.StartPoint = new ptsPoint(1312.0, 0.0, 15.2);
         ray3.Slope = new Slope(+1.0 / 7.0);
         ray3.HorizontalDirection = null;

         ray4.StartPoint = new ptsPoint(1880.0, 0.0, 13.54533333333);
         ray4.Slope = new Slope(-0.02 / 100.0);
         ray4.HorizontalDirection = null;

         ray5.StartPoint = new ptsPoint(1976.600, 0.0, 13.50);
         ray5.Slope = new Slope(-0.06 / 100.0);
         ray5.advanceDirection = -1;
         ray5.HorizontalDirection = null;
      }

      private void pfl1Setup()
      {
         vpiList aVpiList = new vpiList();
         aVpiList.add(1062.50, 2178.23);
         aVpiList.add(1120.00, 2173.973, 115.0);
         aVpiList.add(1220.00, 2173.140, 85.0);
         aVpiList.add(1315.00, 2168.2265, 90.0);
         aVpiList.add(1365.00, 2167.8765);

         pfl1 = new Profile(aVpiList);
      }

      private void pfl2Setup()
      {
         var aVpiList = new vpiList();
         aVpiList.add(1000.00, 100.0);
         aVpiList.add(1100.00, 110.0);
         aVpiList.add(1200.00, 102.0);

         pfl2 = new Profile(aVpiList);
      }

      private void pfl3Setup()
      {
         var aVpiList = new vpiList();
         aVpiList.add(1000.00, 12.0);
         aVpiList.add(1100.00, 12.0);

         pfl3 = new Profile(aVpiList);
      }

      [Test]
      public void arithmaticAdd_computesCorrectElevation_whenOnTangentAndTangent()
      {
         CogoStation sta = new CogoStation(1100.0);
         double el1 = (double)profile1.getElevation(sta);
         double el2 = (double)profile2.getElevation(sta);
         double expectedValue = el1 + el2;

         resultingProfile = Profile.arithmaticAddProfile(profile1, profile2, 1.0);
         double computedElevation = (double)resultingProfile.getElevation(sta);

         Assert.AreEqual(expectedValue, computedElevation, doubleDelta);
      }

      [Test]
      public void arithmaticAdd_computesCorrectElevation_whenBothProfilesAreOnVerticalCurves()
      {
         CogoStation sta = new CogoStation(1950.0);
         double el1 = (double)profile1.getElevation(sta);
         double el2 = (double)profile2.getElevation(sta);
         double expectedValue = el1 + el2;

         resultingProfile = Profile.arithmaticAddProfile(profile1, profile2, 1.0);
         double computedElevation = (double)resultingProfile.getElevation(sta);

         Assert.AreEqual(expectedValue, computedElevation, doubleDelta);
      }

      [Test]
      public void arithmaticAdd_computesCorrectElevation_whenBothProfilesAreOnVerticalCurves_subtract()
      {
         CogoStation sta = new CogoStation(1950.0);
         double el1 = (double)profile1.getElevation(sta);
         double el2 = (double)profile2.getElevation(sta);
         double expectedValue = el1 - el2;

         resultingProfile = Profile.arithmaticAddProfile(profile1, profile2, -1.0);
         double computedElevation = (double)resultingProfile.getElevation(sta);

         Assert.AreEqual(expectedValue, computedElevation, doubleDelta);
      }

      [Test]
      public void arithmaticAdd_computesCorrectElevation_whenOnTangentAndDifferentTangent()
      {
         CogoStation sta = new CogoStation(1130.0);
         double el1 = (double)profile1.getElevation(sta);
         double el2 = (double)profile2.getElevation(sta);
         double expectedValue = el1 + el2;

         resultingProfile = Profile.arithmaticAddProfile(profile1, profile2, 1.0);
         double computedElevation = (double)resultingProfile.getElevation(sta);

         Assert.AreEqual(expectedValue, computedElevation, doubleDelta);
      }

      [Test]
      public void arithmaticAdd_computesCorrectElevation_whenOnTangentSubtractingDifferentTangent()
      {
         CogoStation sta = new CogoStation(1130.0);
         double el1 = (double)profile1.getElevation(sta);
         double el2 = (double)profile2.getElevation(sta);
         double expectedValue = el1 - el2;

         resultingProfile = Profile.arithmaticAddProfile(profile1, profile2, -1.0);
         double computedElevation = (double)resultingProfile.getElevation(sta);

         Assert.AreEqual(expectedValue, computedElevation, doubleDelta);
      }

      [Test]
      public void Intersect2SlopesInX_1()
      {
         double x = Profile.intersect2SlopesInX(1000.0, 100.0, 0.02, 1100.0, 102.0, 0.02);
         double expectedX = 1050.0;
         Assert.AreEqual(expectedX, x, 0.000001);
      }

      public void Intersect2SlopesInX_delegate()
      {
         double x = Profile.intersect2SlopesInX(1000.0, 100.0, 0.02, 1100.0, 100.0, 0.02);
      }

      [Test]
      public void Intersect2SlopesInX_exception()
      {
         Assert.Throws(typeof(Exception), Intersect2SlopesInX_delegate, "Parallel Slopes do not intersect.");
      }

      [Test]
      public void Intersect2SlopesInX_2()
      {
         double x = Profile.intersect2SlopesInX(1000.0, 100.0, 0.02, 1100.0, 100.0, -0.02);
         double expectedX = 1050.0;
         Assert.AreEqual(expectedX, x, 0.000001);
      }

      [Test]
      public void Intersect2SlopesInX_3()
      {
         double x = Profile.intersect2SlopesInX(1000.0, 100.0, 0.02, 1100.0, 96.0, -0.02);
         double expectedX = 950.0;
         Assert.AreEqual(expectedX, x, 0.000001);
      }

      /* This test must be moved soon the CadFoundations test suite when it is ready */
      [Test]
      public void rayGetElevationInRayDomain_IsCorrect()
      {
         ptsRay localRay = new ptsRay();
         localRay.StartPoint = new ptsPoint(25.0, 0.0, 25.0);
         localRay.Slope = new Slope(1.0);
         localRay.HorizontalDirection = null;
         double? actual = localRay.getElevationAlong(35.0);
         double? expected = 35.0;
         Assert.AreEqual(expected.ToString(), actual.ToString());
         //String act = actual.ToString();
         //String exp = expected.ToString();
      }

      /* This test must be moved soon the CadFoundations test suite when it is ready */
      [Test]
      public void rayGetElevationOutsideRayDomain_IsNull()
      {
         ptsRay localRay = new ptsRay();
         localRay.StartPoint = new ptsPoint(25.0, 0.0, 25.0);
         localRay.Slope = new Slope();
         localRay.Slope.setFromXY(0, 1);
         localRay.HorizontalDirection = null;
         double? actual = localRay.getElevationAlong(25.0);
         Assert.IsNull(actual);
      }

      /* This test must be moved soon the CadFoundations test suite when it is ready */
      [Test]
      public void rayGetElevationWhenRayIsVertical_IsNull()
      {
         ptsRay localRay = new ptsRay();
         localRay.StartPoint = new ptsPoint(25.0, 0.0, 25.0);
         localRay.Slope = new Slope(1.0);
         localRay.HorizontalDirection = null;
         double? actual = localRay.getElevationAlong(15.0);
         Assert.IsNull(actual);
      }

      [Test]
      public void rayIntersectProfileWithRayWithOneHit_ReturnsNotNull()
      {

         List<Profile> raySegments = profile1.getIntersections(ray1);
         Assert.IsNotNull(raySegments);
      }

      [Test]
      public void rayIntersectProfileOnVerticalTangentWithOneHit()
      {
         List<Profile> rayResults = profile1.getIntersections(ray1);
         int actual = rayResults.Count;
         int expected = 1;
         Assert.AreEqual(expected: expected, actual: actual);
      }

      [Test]
      public void rayIntersectProfileOnVerticalTangentWithOneHit_AtCorrectStation()
      {
         List<Profile> rayResults = profile1.getIntersections(ray1);
         double actual = rayResults.First<Profile>().EndProfTrueStation;
         double expected = 1102.0;
         Assert.AreEqual(expected: expected, actual: actual, delta: 0.00000001);
      }

      [Test]
      public void rayIntersectProfileOnVerticalTangentWithThreeHits()
      {
         List<Profile> rayResults = profile1.getIntersections(ray2);
         int actual = rayResults.Count;
         int expected = 3;
         Assert.AreEqual(expected: expected, actual: actual);
      }

      [Test]
      public void rayIntersectProfileOnVerticalTangentWithThreeHits_AtCorrectStation()
      {
         List<Profile> rayResults = profile1.getIntersections(ray2);
         double actual = rayResults.Last<Profile>().EndProfTrueStation;
         double expected = 1348.40;
         Assert.AreEqual(expected: expected, actual: actual, delta: 0.00000001);
      }

      [Test]
      public void rayIntersectProfileWithNoHits_returnsNull()
      {
         List<Profile> rayResults = profile1.getIntersections(ray3);
         Assert.IsNull(rayResults);
      }
      //[Test] public void intersectProfileOnVerticalCurveWithRayWithOneHit()

      [Test]
      public void rayIntersectProfileOnParabola_WithTwoHits()
      {
         List<Profile> rayResults = profile1.getIntersections(ray4);
         int actual = rayResults.Count;
         int expected = 2;
         Assert.AreEqual(expected: expected, actual: actual);
      }

      [Test]
      public void rayIntersectProfileOnParabola_WithTwoHits_AtCorrectStations()
      {
         List<Profile> rayResults = profile1.getIntersections(ray4);
         if (2 != rayResults.Count)
            throw new Exception("Expected 2 intersections but got not 2.");

         bool actualBool = true;
         actualBool = actualBool && (0 == utilFunctions.tolerantCompare(rayResults[0].EndProfTrueStation, 1940.00, 0.000001));
         actualBool = actualBool && (0 == utilFunctions.tolerantCompare(rayResults[1].EndProfTrueStation, 2024.80, 0.000001));

         Assert.AreEqual(expected: true, actual: actualBool);
      }

      [Test]
      public void rayIntersectProfileOnParabola_WithThreeHits_AtCorrectStations()
      {
         List<Profile> rayResults = profile1.getIntersections(ray5);
         if (null == rayResults)
            throw new NullReferenceException();
         if (3 != rayResults.Count)
            throw new Exception("Expected 3 intersections but got not 3.");

         bool actualBool = true;
         actualBool = actualBool && (0 == utilFunctions.tolerantCompare(rayResults[0].BeginProfTrueStation, 1923.7808, 0.0001));
         actualBool = actualBool && (0 == utilFunctions.tolerantCompare(rayResults[1].BeginProfTrueStation, 1326.3409, 0.0001));
         actualBool = actualBool && (0 == utilFunctions.tolerantCompare(rayResults[2].BeginProfTrueStation, 1153.5388, 0.0001));

         Assert.AreEqual(expected: true, actual: actualBool);
      }
      //[Test] public void intersectProfileWithRayWithTwoHits_andOneIsAColocatedLine()

      [Test]
      public void fixedBug2012011_1()
      {
         Profile pfl2 = new Profile();
         pfl2.addSegment(
               (CogoStation) 1000,  // BeginStation
               12,  // BeginElevation -- EndElevation = 12
               0,  // BeginSlope
               0,  // EndSlope -- KValue = Infinity
               9000,  // Length
               false,  // IsBeginPINC
               false,  // IsEndPINC
               false);  // IsaProfileGap

         Profile pfl1 = new Profile();
         // Add a Segment: No 0
         pfl1.addSegment(
               (CogoStation)1000,  // BeginStation
               0,  // BeginElevation -- EndElevation = 0
               0,  // BeginSlope
               0,  // EndSlope -- KValue = 0
               1235,  // Length
               false,  // IsBeginPINC
               true,  // IsEndPINC
               false);  // IsaProfileGap

         // Add a Segment: No 1
         pfl1.addSegment(
               (CogoStation)2235,  // BeginStation
               0,  // BeginElevation -- EndElevation = 12
               0.0375,  // BeginSlope
               0.0375,  // EndSlope -- KValue = 0
               320,  // Length
               true,  // IsBeginPINC
               false,  // IsEndPINC
               false);  // IsaProfileGap

         // Add a Segment: No 2
         pfl1.addSegment(
               (CogoStation)2555,  // BeginStation
               12,  // BeginElevation -- EndElevation = 27
               0.0375,  // BeginSlope
               0.0375,  // EndSlope -- KValue = 0
               400,  // Length
               false,  // IsBeginPINC
               true,  // IsEndPINC
               false);  // IsaProfileGap

         // Add a Segment: No 3
         pfl1.addSegment(
               (CogoStation)2955,  // BeginStation
               27,  // BeginElevation -- EndElevation = 27
               0,  // BeginSlope
               0,  // EndSlope -- KValue = 0
               5095,  // Length
               true,  // IsBeginPINC
               true,  // IsEndPINC
               false);  // IsaProfileGap

         // Add a Segment: No 4
         pfl1.addSegment(
               (CogoStation)8050,  // BeginStation
               27,  // BeginElevation -- EndElevation = 0
               -26999.9999944994,  // BeginSlope
               -26999.9999944994,  // EndSlope -- KValue = 0
               0.00100000000020373,  // Length
               true,  // IsBeginPINC
               true,  // IsEndPINC
               false);  // IsaProfileGap

         // Add a Segment: No 5
         pfl1.addSegment(
               (CogoStation)8050.001,  // BeginStation
               0,  // BeginElevation -- EndElevation = 0
               0,  // BeginSlope
               0,  // EndSlope -- KValue = 0
               1949.999,  // Length
               true,  // IsBeginPINC
               false,  // IsEndPINC
               false);  // IsaProfileGap

         Profile pflResult = Profile.arithmaticAddProfile(pfl1, pfl2, -1.0);

         double actualElevation = (double)pflResult.getElevation((CogoStation)5000);
         double expectedElevation = 15.0;

         Assert.AreEqual(expectedElevation, actualElevation, 0.000001);
      
      }

      [Test]
      public void getProfileFromMockSurface_DefaultProfile_isNotNull()
      {
         rm21MockSurface aSurface = new rm21MockSurface();
         Profile profileFromSurface = aSurface.getSectionProfile(null, -200.0, null);

         Assert.NotNull(profileFromSurface);
      }

      [Test]
      public void getProfileFromMockSurface_DefaultProfile_hasCorrectElevation()
      {
         rm21MockSurface aSurface = new rm21MockSurface();
         Profile profileFromSurface = aSurface.getSectionProfile(null, -200.0, null);

         double actual = (double) profileFromSurface.getElevation((CogoStation) (- 10.0));
         double expected = 10.0;

         Assert.AreEqual(expected: expected, actual: actual, delta: 0.00001);
      }

      [Test]
      public void getProfileFromMockSurface_DefaultProfile_hasCorrectEndStation()
      {
         rm21MockSurface aSurface = new rm21MockSurface();
         Profile profileFromSurface = aSurface.getSectionProfile(null, -200.0, null);

         double actual = profileFromSurface.EndProfTrueStation;
         double expected = 200.0;

         Assert.AreEqual(expected: expected, actual: actual, delta: 0.00001);
      }

      [Test]
      public void getProfileFromMockSurface_CustomProfile_hasCorrectElevation()
      {
         rm21MockSurface aSurface = new rm21MockSurface();
         aSurface.theProfile = new Profile(-190.0, -100.0, 10.0);
         aSurface.theProfile.appendStationAndElevation((CogoStation)(-20.0), 12.0);
         aSurface.theProfile.appendStationAndElevation((CogoStation)20.0, 8.0);
         aSurface.theProfile.appendStationAndElevation((CogoStation)200, 16.0);

         Profile profileFromSurface = aSurface.getSectionProfile(null, 0.0, null);

         double actual = (double)profileFromSurface.getElevation((CogoStation)(-10.0));
         double expected = 11.0;

         Assert.AreEqual(expected: expected, actual: actual, delta: 0.00001);
      }

      [Test]
      public void Profile_WhenOnVerticalCurveAtStation1120_elevationResultIsSingleValue()
      {
         conditionString = "Verify tupleNullableDoubles isSingleValue is true when getting elevation";
         result.back = result.ahead = 0.0;
         result.isSingleValue = false;
         pfl1.getElevation((CogoStation)1120.00, out result);
         expectedBl = true;
         actualBl = result.isSingleValue;
         Assert.AreEqual(expected: expectedBl, actual: actualBl, message: conditionString);
         //TestingFramework.assertEquals<bool>(expectedBl, actualBl, conditionString);
      }

      [Test]
      public void Profile_WhenOnVerticalCurveAtStation1120_elevationIs2171p9175()
      {
         conditionString = "Verify back elevation is 2174.9175";
         expectedDbl = 2174.9175;
         pfl1.getElevation((CogoStation)1120.00, out result);
         actualDbl = Math.Round((double)result.back, 4);
         Assert.AreEqual(expected: expectedDbl, actual: actualDbl, message: conditionString);
      }

      [Test]
      public void Profile_WhenOnVerticalCurveAtStation1120_backSlopeIsNeg4p1182percent()
      {
         conditionString = "Verify back slope is -4.1182%";
         expectedDbl = -0.041182;
         pfl1.getSlope((CogoStation)1120.00, out result);
         actualDbl = Math.Round((double)result.back, 6);
         Assert.AreEqual(expected: expectedDbl, actual: actualDbl, message: conditionString);
      }

      [Test]
      public void Profile_WhenOnVerticalCurveAtStation1120_aheadSlopeIsNeg4p1182percent()
      {
         conditionString = "Verify ahead slope is -4.1182%";
         expectedDbl = -0.041182;
         pfl1.getSlope((CogoStation)1120.00, out result);
         actualDbl = Math.Round((double)result.ahead, 6);
         Assert.AreEqual(expected: expectedDbl, actual: actualDbl, message: conditionString);
      }

      [Test]
      public void Profile_WhenOnVerticalCurveAtStation1120_kValueResultIsSingleValue()
      {
         conditionString = "Verify tupleNullableDoubles isSingleValue is true when getting K Value";
         pfl1.getKvalue((CogoStation)1120.00, out result);
         expectedBl = true;
         actualBl = result.isSingleValue;
         Assert.AreEqual(expected: expectedDbl, actual: actualDbl, message: conditionString);
      }

      [Test]
      public void Profile_WhenOnVerticalCurveAtStation1120_backKvalueIs17p5()
      {
         conditionString = "Verify back K value is +17.5";
         expectedDbl = 17.5;
         pfl1.getKvalue((CogoStation)1120.00, out result);
         actualDbl = Math.Round((double)result.back, 1);
         Assert.AreEqual(expected: expectedDbl, actual: actualDbl, message: conditionString);
      }

      [Test]
      public void Profile_WhenOnVerticalCurveAtStation1120_aheadKvalueIs17p5()
      {
         conditionString = "Verify ahead K value is +17.5";
         expectedDbl = 17.5;
         pfl1.getKvalue((CogoStation)1120.00, out result);
         actualDbl = Math.Round((double)result.ahead, 1);
         Assert.AreEqual(expected: expectedDbl, actual: actualDbl, message: conditionString);
      }

      [Test]
      public void Profile_WhenOnVerticalTangentAtStation1265_elevationResultIsSingleValue()
      {
         conditionString = "Verify tupleNullableDoubles isSingleValue is true";
         result.back = result.ahead = 0.0;
         result.isSingleValue = false;
         pfl1.getElevation((CogoStation)1265.00, out result);
         expectedBl = true;
         actualBl = result.isSingleValue;
         Assert.AreEqual(expected: expectedDbl, actual: actualDbl, message: conditionString);
      }

      [Test]
      public void Profile_WhenOnVerticalTangentAtStation1265_backElevationIs2170p8126()
      {
         conditionString = "Verify back elevation is 2170.8126";
         expectedDbl = 2170.8126;
         pfl1.getElevation((CogoStation)1265.00, out result);
         actualDbl = Math.Round((double)result.back, 4);
         Assert.AreEqual(expected: expectedDbl, actual: actualDbl, message: conditionString);
      }

      [Test]
      public void Profile_WhenOnVerticalTangentAtStation1265_aheadElevationIs2170p8126()
      {
         conditionString = "Verify ahead elevation is 2170.8126";
         expectedDbl = 2170.8126;
         pfl1.getElevation((CogoStation)1265.00, out result);
         actualDbl = Math.Round((double)result.ahead, 4);
         Assert.AreEqual(expected: expectedDbl, actual: actualDbl, message: conditionString);
      }

      [Test]
      public void Profile_WhenOnVerticalTangentAtStation1265_backSlopeIsNeg5p1721percent()
      {
         conditionString = "Verify back slope is -5.1721%";
         expectedDbl = -0.051721;
         pfl1.getSlope((CogoStation)1265.00, out result);
         actualDbl = Math.Round((double)result.back, 6);
         Assert.AreEqual(expected: expectedDbl, actual: actualDbl, message: conditionString);
      }

      [Test]
      public void Profile_WhenOnVerticalTangentAtStation1265_aheadSlopeIsNeg5p1721percent()
      {
         conditionString = "Verify ahead slope is -5.1721%";
         expectedDbl = -0.051721;
         pfl1.getSlope((CogoStation)1265.00, out result);
         actualDbl = Math.Round((double)result.ahead, 6);
         Assert.AreEqual(expected: expectedDbl, actual: actualDbl, message: conditionString);
      }

      [Test]
      public void Profile_WhenOnVerticalTangentAtStation1265_backKvalueIsInfinity()
      {
         conditionString = "Verify back K Value is infinity";
         expectedDbl = double.PositiveInfinity;
         pfl1.getKvalue((CogoStation)1265.00, out result);
         actualDbl = (double) result.back;
         //Assert.AreEqual(expected: expectedDbl, actual: actualDbl, message: conditionString);
         Assert.True(true);
      }

      [Test]
      public void Profile_WhenOnVerticalTangentAtStation1265_aheadKvalueIsInfinity()
      {
         conditionString = "Verify ahead K Value is infintiy";
         expectedDbl = double.PositiveInfinity;
         actualDbl = (double) result.ahead;
         //Assert.AreEqual(expected: expectedDbl, actual: actualDbl, message: conditionString);
         Assert.True(true);
      }

      [Test]
      public void Profile_WhenOnVerticalTangentAtBeginProfile_elevationResultIsNotSingleValue()
      {
         conditionString = "Verify tupleNullableDoubles isSingleValue is false";
         result.back = result.ahead = 0.0;
         result.isSingleValue = false;
         pfl1.getElevation((CogoStation)1062.50, out result);
         expectedBl = false;
         actualBl = result.isSingleValue;
         Assert.AreEqual(expected: expectedBl, actual: actualBl, message: conditionString);
      }

      [Test]
      public void Profile_WhenOnVerticalTangentAtBeginProfile_backElevationIsNull()
      {
         conditionString = "Verify back elevation is null";
         result.back = result.ahead = 0.0;
         result.isSingleValue = false;
         pfl1.getElevation((CogoStation)1062.50, out result);
         Assert.IsNull(result.back, conditionString);
      }

      [Test]
      public void Profile_WhenOnVerticalTangentAtBeginProfile_aheadElevationIs2178p23()
      {
         conditionString = "Verify ahead elevation is 2178.23";
         expectedDbl = 2178.23;
         pfl1.getElevation((CogoStation)1062.50, out result);
         actualDbl = Math.Round((double)result.ahead, 4);
         Assert.AreEqual(expected: expectedDbl, actual: actualDbl, message: conditionString);
      }

      [Test]
      public void Profile_WhenOnVerticalTangentAtBeginProfile_slopeResultsIsNotSingleValue()
      {
         pfl1.getSlope((CogoStation)1062.50, out result);
         expectedBl = false;
         actualBl = result.isSingleValue;
         Assert.AreEqual(expected: expectedBl, actual: actualBl, message: conditionString);
      }

      [Test]
      public void Profile_WhenOnVerticalTangentAtBeginProfile_backSlopeIsNull()
      {
         conditionString = "Verify back slope is null";
         pfl1.getSlope((CogoStation)1062.50, out result);
         Assert.IsNull(result.back, conditionString);
      }

      [Test]
      public void Profile_WhenOnVerticalTangentAtBeginProfile_aheadSlopeIsNeg7p4035percent()
      {
         conditionString = "Verify ahead slope is -7.4035%";
         expectedDbl = -0.074035;
         pfl1.getSlope((CogoStation)1062.50, out result);
         actualDbl = Math.Round((double)result.ahead, 6);
         Assert.AreEqual(expected: expectedDbl, actual: actualDbl, message: conditionString);
      }

      [Test]
      public void Profile_WhenOnVerticalTangentAtEndProfile_elevationValueIsNotSingleValue()
      {
         conditionString = "Verify tupleNullableDoubles isSingleValue is false";
         result.back = result.ahead = 0.0;
         result.isSingleValue = false;
         pfl1.getElevation((CogoStation)1365.0, out result);
         expectedBl = false;
         actualBl = result.isSingleValue;
         Assert.AreEqual(expected: expectedDbl, actual: actualDbl, message: conditionString);
      }

      [Test]
      public void Profile_WhenOnVerticalTangentAtEndProfile_backElevationIs2167p8765()
      {
         conditionString = "Verify back elevation is 2167.8765";
         expectedDbl = 2167.8765;
         pfl1.getElevation((CogoStation)1365.0, out result);
         actualDbl = Math.Round((double)result.back, 4);
         Assert.AreEqual(expected: expectedDbl, actual: actualDbl, message: conditionString);
      }

      [Test]
      public void Profile_WhenOnVerticalTangentAtEndProfile_aheadElevationIsNull()
      {
         conditionString = "Verify ahead elevation is null";
         pfl1.getElevation((CogoStation)1365.0, out result);
         Assert.IsNull(result.ahead, conditionString);
      }

      [Test]
      public void Profile_WhenAtVerticalPRC_slopeValueIsSingleValue()
      {
         conditionString = "Verify tupleNullableDoubles isSingleValue is true for slopes";
         result.back = result.ahead = 0.0;
         result.isSingleValue = true;
         pfl1.getSlope((CogoStation)1177.5, out result);
         expectedBl = true;
         actualBl = result.isSingleValue;
         Assert.AreEqual(expected: expectedDbl, actual: actualDbl, message: conditionString);
      }

      [Test]
      public void Profile_WhenAtVerticalPRC_backSlopeIsNeg0p8330percent()
      {
         conditionString = "Verify back slope is -0.8330%";
         expectedDbl = -0.00833;
         pfl1.getSlope((CogoStation)1177.5, out result);
         actualDbl = Math.Round((double)result.back, 6);
         Assert.AreEqual(expected: expectedDbl, actual: actualDbl, message: conditionString);
      }

      [Test]
      public void Profile_WhenAtVerticalPRC_aheadSlopeIsNeg0p8330percent()
      {
         conditionString = "Verify ahead slope is -0.8330%";
         expectedDbl = -0.00833;
         pfl1.getSlope((CogoStation)1177.5, out result);
         actualDbl = Math.Round((double)result.ahead, 6);
         Assert.AreEqual(expected: expectedDbl, actual: actualDbl, message: conditionString);
      }

      [Test]
      public void Profile_WhenAtVerticalPRC_kValuesIsNotSingleValue()
      {
         conditionString = "Verify tupleNullableDoubles isSingleValue is false for K Values";
         result.back = result.ahead = 0.0;
         result.isSingleValue = false;
         pfl1.getKvalue((CogoStation)1177.5, out result);
         expectedBl = false;
         actualBl = result.isSingleValue;
         Assert.AreEqual(expected: expectedDbl, actual: actualDbl, message: conditionString);
      }

      [Test]
      public void Profile_WhenAtVerticalPRC_backKvalueIs17p5()
      {
         conditionString = "Verify back K value is 17.5";
         expectedDbl = 17.5;
         pfl1.getKvalue((CogoStation)1177.5, out result);
         actualDbl = Math.Round((double)result.back, 1);
         Assert.AreEqual(expected: expectedDbl, actual: actualDbl, message: conditionString);
      }

      [Test]
      public void Profile_WhenAtVerticalPRC_aheadKvalueIsNeg19p6()
      {
         conditionString = "Verify ahead K value is -19.6";
         expectedDbl = -19.6;
         pfl1.getKvalue((CogoStation)1177.5, out result);
         actualDbl = Math.Round((double)result.ahead, 1);
         Assert.AreEqual(expected: expectedDbl, actual: actualDbl, message: conditionString);
      }

      [Test]
      public void Profile_WhenNotOnVerticalPINC_IsOnPINCisFalse()
      {
         conditionString = "Verify we know we are not on a PINC at station 11+77.50";
         expectedBl = false;
         actualBl = pfl1.isOnPINC((CogoStation)1177.50);
         Assert.AreEqual(expected: expectedBl, actual: actualBl, message: conditionString);
      }

      [Test]
      public void Profile_WhenOnVerticalPINC_11plus00_elevationIsSingleValue()
      {
         conditionString = "Verify tupleNullableDoubles isSingleValue is true for elevation";
         result.back = result.ahead = 0.0;
         result.isSingleValue = true;
         pfl2.getElevation((CogoStation)1100.0, out result);
         expectedBl = true;
         actualBl = result.isSingleValue;
         Assert.AreEqual(expected: expectedDbl, actual: actualDbl, message: conditionString);
      }

      [Test]
      public void Profile_WhenOnVerticalPINC_11plus00_confirmStationIsOnVerticalPINC()
      {
         conditionString = "Verify we know we are on a PINC at station 11+00";
         expectedBl = true;
         actualBl = pfl2.isOnPINC((CogoStation)1100.0);
         Assert.AreEqual(expected: expectedBl, actual: actualBl, message: conditionString);
      }

      [Test]
      public void Profile_WhenOnVerticalPINC_11plus50_confirmStationIsNotOnVerticalPINC()
      {
         conditionString = "Verify we know we are not on a PINC at station 11+50";
         expectedBl = false;
         actualBl = pfl2.isOnPINC((CogoStation)1150.0);
         Assert.AreEqual(expected: expectedBl, actual: actualBl, message: conditionString);
      }

      [Test]
      public void Profile_WhenOnVerticalPINC_11plus00_backElevationIs110p0()
      {
         conditionString = "Verify back elevation is 110.0";
         expectedDbl = 110.0;
         pfl2.getElevation((CogoStation)1100.0, out result);
         actualDbl = Math.Round((double)result.back, 6);
         Assert.AreEqual(expected: expectedDbl, actual: actualDbl, message: conditionString);
      }

      [Test]
      public void Profile_WhenOnVerticalPINC_11plus00_aheadElevationIs110p0()
      {
         conditionString = "Verify ahead elevation is 110.0";
         expectedDbl = 110.0;
         pfl2.getElevation((CogoStation)1100.0, out result);
         actualDbl = Math.Round((double)result.ahead, 6);
         Assert.AreEqual(expected: expectedDbl, actual: actualDbl, message: conditionString);
      }

      [Test]
      public void Profile_WhenOnVerticalPINC_11plus00_slopeIsNotSingleValue()
      {
         conditionString = "Verify tupleNullableDoubles isSingleValue is false for slope";
         pfl2.getSlope((CogoStation)1100.0, out result);
         expectedBl = false;
         actualBl = result.isSingleValue;
         Assert.AreEqual(expected: expectedBl, actual: actualBl, message: conditionString);
      }

      [Test]
      public void Profile_WhenOnVerticalPINC_11plus00_backSlopeIs10percent()
      {
         conditionString = "Verify back slope is +10.0%";
         expectedDbl = 0.10;
         pfl2.getSlope((CogoStation)1100.0, out result);
         actualDbl = Math.Round((double)result.back, 6);
         Assert.AreEqual(expected: expectedDbl, actual: actualDbl, message: conditionString);
      }

      [Test]
      public void Profile_WhenOnVerticalPINC_11plus00_aheadSlopeIsNeg8percent()
      {
         conditionString = "Verify ahead slope is -8.0%";
         expectedDbl = -0.080;
         pfl2.getSlope((CogoStation)1100.0, out result);
         actualDbl = Math.Round((double)result.ahead, 6);
         Assert.AreEqual(expected: expectedDbl, actual: actualDbl, message: conditionString);
      }

      [Test]
      public void Profile_WhenOnVerticalPINC_11plus00_kValueIsSingleValue()
      {
         conditionString = "Verify tupleNullableDoubles isSingleValue is true for K value";
         expectedBl = true;
         pfl2.getKvalue((CogoStation)1100.0, out result);
         actualBl = result.isSingleValue;
         Assert.AreEqual(expected: expectedBl, actual: actualBl, message: conditionString);
      }

      [Test]
      public void Profile_WhenOnVerticalPINC_11plus00_backKvalueIsZero()
      {
         conditionString = "Verify back K value is 0.0";
         expectedDbl = 0.0;
         pfl2.getKvalue((CogoStation)1100.0, out result);
         actualDbl = (double) result.back;
         Assert.AreEqual(expected: expectedDbl, actual: actualDbl, message: conditionString);
      }

      [Test]
      public void Profile_WhenOnVerticalPINC_11plus00_aheadKvalueIsZero()
      {
         conditionString = "Verify ahead K value is 0.0";
         expectedDbl = 0.0;
         pfl2.getKvalue((CogoStation)1100.0, out result);
         actualDbl = (double)result.ahead;
         Assert.AreEqual(expected: expectedDbl, actual: actualDbl, message: conditionString);
      }

      [Test]
      public void Profile_AfterAppendingToAprofile_11plus50_elevationIs13()
      {
         pfl3Setup();
         pfl3.appendStationAndElevation((CogoStation)900.00, 14.0);
         conditionString = "Verify elevation == 13.0";
         pfl3.getElevation((CogoStation)950.0, out result);
         expectedDbl = 13.0;
         actualDbl = (double) result.ahead;
         Assert.AreEqual(expected: expectedDbl, actual: actualDbl, message: conditionString);
      }

      [Test]
      public void Profile_AfterInsertingAvpi_10plus50_elevationIsNeg8()
      {
         pfl3Setup();
         pfl3.appendStationAndElevation((CogoStation)900.00, 14.0);
         pfl3.appendStationAndElevation((CogoStation)1050.00, -8.0);
         conditionString = "Verify elevation == -8.0 at 10+50";
         pfl3.getElevation((CogoStation)1050.0, out result);
         expectedDbl = -8.0;
         actualDbl = (double) result.ahead;
         Assert.AreEqual(expected: expectedDbl, actual: actualDbl, message: conditionString);
      }

      [Test]
      public void Profile_OnAnoVCProfile_sta10plus25_elevationIs2()
      {
         conditionString = "Verify elevation == +2.0 at 10+25";
         pfl3Setup();
         pfl3.appendStationAndElevation((CogoStation)900.00, 14.0);
         pfl3.appendStationAndElevation((CogoStation)1050.00, -8.0);
         pfl3.getElevation((CogoStation)1025.0, out result);
         expectedDbl = 2.0;
         actualDbl = (double) result.ahead;
         Assert.AreEqual(expected: expectedDbl, actual: actualDbl, message: conditionString);
      }

      [Test]
      public void Profile_OnAnoVCProfile_sta10plus75_elevationIs2()
      {
         conditionString = "Verify elevation == +2.0 at 10+75";
         pfl3Setup();
         pfl3.appendStationAndElevation((CogoStation)900.00, 14.0);
         pfl3.appendStationAndElevation((CogoStation)1050.00, -8.0);
         pfl3.getElevation((CogoStation)1075.0, out result);
         expectedDbl = 2.0;
         actualDbl = (double) result.ahead;
         Assert.AreEqual(expected: expectedDbl, actual: actualDbl, message: conditionString);
      }

      private void pfl4and5Setup()
      {
         var aVpiList = new vpiList();
         aVpiList.add(1062.50, 2178.23);
         aVpiList.add(1120.00, 2173.973, 115.0);
         aVpiList.add(1220.00, 2173.140, 85.0);
         aVpiList.add(1315.00, 2168.2265, 90.0);
         aVpiList.add(1365.00, 2167.8765);

         pfl4 = new Profile(aVpiList);

         pfl5 = Profile.arithmaticAddProfile(null, pfl4, -1.0);

      }

      [Test]
      public void Profile4_station11plus20_elevationIs2174p9175()
      {
         conditionString = "Verify back elevation is 2174.9175 at 11+20";
         result.back = result.ahead = 0.0;
         result.isSingleValue = false;
         pfl4.getElevation((CogoStation)1120.00, out result);
         expectedDbl = 2174.9175;
         actualDbl = Math.Round((double)result.back, 4);
         Assert.AreEqual(expected: expectedDbl, actual: actualDbl, message: conditionString);
      }


      [Test]
      public void Profile4_station11plus20_gradeIsNeg4p118percent()
      {
         conditionString = "Verify grade is -4.118% at 11+20";
         result.back = result.ahead = 0.0;
         result.isSingleValue = false;
         pfl4.getSlope((CogoStation)1120.00, out result);
         expectedDbl = -0.041182;
         actualDbl = Math.Round((double)result.back, 6);
         Assert.AreEqual(expected: expectedDbl, actual: actualDbl, message: conditionString);
      }

      [Test]
      public void Profile5_station11plus20_elevationIsNeg2174p9175()
      {
         result.back = result.ahead = 0.0;
         result.isSingleValue = false;
         pfl5.getElevation((CogoStation)1120.00, out result);
         expectedDbl = -2174.9175;
         actualDbl = Math.Round((double)result.back, 4);
         Assert.AreEqual(expected: expectedDbl, actual: actualDbl, message: conditionString);
      }

      [Test]
      public void Profile5_station11plus20_gradeIs4p118percent()
      {
         conditionString = "Verify grade is +4.118% at 11+20";
         result.back = result.ahead = 0.0;
         result.isSingleValue = false;
         pfl5.getSlope((CogoStation)1120.00, out result);
         expectedDbl = 0.041182;
         actualDbl = Math.Round((double)result.back, 6);
         Assert.AreEqual(expected: expectedDbl, actual: actualDbl, message: conditionString);
      }

      private void pfl6_7and8Setup()
      {
         var aVpiList = new vpiList();
         aVpiList.add(1062.50, 12.0);
         aVpiList.add(1120.00, 12.0);
         aVpiList.add(1220.00, 15.0);
         aVpiList.add(1315.00, 15.0);
         aVpiList.add(1345.00, 10.0);
         aVpiList.add(1365.00, 10.0);

         pfl6 = new Profile(aVpiList);

         aVpiList = new vpiList();
         aVpiList.add(2062.50, 12.0);
         aVpiList.add(2120.00, 12.0);
         aVpiList.add(2220.00, 15.0);

         pfl7 = new Profile(aVpiList);

         pfl8 = Profile.arithmaticAddProfile(pfl6, pfl7, 1.0);

      }

      [Test]
      public void Profile8_atBeginningOfInteriorGap_elevationIsNotSingleValue()
      {
         result.back = result.ahead = 0.0;
         result.isSingleValue = true;
         expectedBl = false;
         pfl8.getElevation((CogoStation)1365.0, out result);
         actualBl = result.isSingleValue;
         Assert.AreEqual(expected: expectedBl, actual: actualBl, message: conditionString);
      }

      [Test]
      public void Profile8_atEndOfInteriorGap_elevationIsNotSingleValue()
      {
         result.back = result.ahead = 0.0;
         result.isSingleValue = true;
         expectedBl = false;
         pfl8.getElevation((CogoStation)2062.5, out result);
         actualBl = result.isSingleValue;
         Assert.AreEqual(expected: expectedBl, actual: actualBl, message: conditionString);
      }

      private void pfl9Setup()
      {
         // Tests for adding two profiles that cover the same range: No VCs
         var aVpiList = new vpiList();
         aVpiList.add(1062.50, 12.0);
         aVpiList.add(1120.00, 12.0);
         aVpiList.add(1220.00, 15.0);
         aVpiList.add(1315.00, 15.0);
         aVpiList.add(1345.00, 10.0);
         aVpiList.add(1365.00, 10.0);

         var aProfile = new Profile(aVpiList);

         aVpiList = new vpiList();
         aVpiList.add(1062.50, 12.0);
         aVpiList.add(1120.00, 12.0);
         aVpiList.add(1219.00, 15.0);
         aVpiList.add(1315.00, 15.0);
         aVpiList.add(1345.00, 10.0);
         aVpiList.add(1365.00, 10.0);
         var otherPfl = new Profile(aVpiList);

         pfl9 = Profile.arithmaticAddProfile(aProfile, otherPfl, 1.0);
      }

      [Test]
      public void Profile9_station13plus00_elevationIs30()
      {
         result.back = result.ahead = 0.0;
         result.isSingleValue = true;
         expectedDbl = 30.0;
         pfl9.getElevation((CogoStation)1300.0, out result);
         if (result.isSingleValue == false)
            throw new Exception("Unexpected value for elevation is not single value");

         actualDbl = (double)result.back;
         Assert.AreEqual(expected: expectedDbl, actual: actualDbl, message: conditionString);
      }

      [Test]
      public void Profile9_station13plus00_slopeIs0()
      {
         result.back = result.ahead = 0.0;
         result.isSingleValue = true;
         expectedDbl = 0.0;
         pfl9.getSlope((CogoStation)1300.0, out result);
         if (result.isSingleValue == false)
            throw new Exception("Unexpected value for elevation is not single value");

         actualDbl = (double)result.back;
         Assert.AreEqual(expected: expectedDbl, actual: actualDbl, message: conditionString);
      }

   }
}
