using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using ptsCogo;


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

         profile1 = new Profile(aVpiList);

         aVpiList = new vpiList();
         aVpiList.add(1062.50, 12.0);
         aVpiList.add(1120.00, 12.0);
         aVpiList.add(1220.00, 15.0);

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
         double computedElevation = (double) resultingProfile.getElevation(sta);

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

   }

}
