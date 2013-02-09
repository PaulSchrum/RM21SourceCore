using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using ptsCogo;
using ptsCogo.coordinates;
using ptsCogo.Angle;


namespace NUnitTestingLibrary
{
   [TestFixture]
   public class TestClassForProfiles
   {
      double doubleDelta=0.0000001;
      private Profile profile1;
      private Profile profile2;
      private Profile resultingProfile;

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

      [Test]
      public void intersectProfileOnVerticalTangentWithRayWithOneHit()
      {
         ptsRay aRay = new ptsRay();

      }

      //[Test] public void intersectProfileOnVerticalTangentWithRayWithOneHit()
      //[Test] public void intersectProfilewithRayNoHits()
      //[Test] public void intersectProfileOnVerticalCurveWithRayWithOneHit()
      //[Test] public void intersectProfileWithRayWithTwoHits()
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

   }

}
