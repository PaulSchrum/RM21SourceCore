using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using ptsCogo;
using ptsCogo.coordinates;
using ptsCogo.Angle;
using rm21Core.Mocks;


namespace NUnitTestingLibrary
{
   [TestFixture]
   public class TestClassForProfiles
   {
      double doubleDelta=0.0000001;
      private Profile profile1;
      private Profile profile2;
      private Profile resultingProfile;
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
         aSurface.theProfile.addStationAndElevation((CogoStation)(-20.0), 12.0);
         aSurface.theProfile.addStationAndElevation((CogoStation)20.0, 8.0);
         aSurface.theProfile.addStationAndElevation((CogoStation)200, 16.0);

         Profile profileFromSurface = aSurface.getSectionProfile(null, 0.0, null);

         double actual = (double)profileFromSurface.getElevation((CogoStation)(-10.0));
         double expected = 11.0;

         Assert.AreEqual(expected: expected, actual: actual, delta: 0.00001);
      }

   }

}
