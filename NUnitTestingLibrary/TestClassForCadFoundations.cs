using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using ptsCogo;
using ptsCogo.Angle;


namespace NUnitTestingLibrary
{
   [TestFixture]
   public class TestClassForCadFoundations
   {
      private Double delta = 0.0000001;

      [Test]
      public void ptsAngle_settingTo1_shouldResultIn_equals57_2957795Degrees()
      {
         ptsAngle angle = 1.0;
         Double expected = 57.2957795;
         Double actual = angle.getAsDegrees();
         Assert.AreEqual(expected: expected, actual: actual, delta: delta);
      }

      [Test]
      public void Azimuth_setToDMS183__29__29_5_shouldResultIn_Angle()
      {
         Azimuth anAzimuth = new Azimuth();
         anAzimuth.setFromDegreesMinutesSeconds(183, 29, 29.5);
         Double expected = 183.4915277778;
         Double actual = anAzimuth.getAsDegrees();
         Assert.AreEqual(expected: expected, actual: actual, delta: delta);
      }

      [Test]
      public void Deflection_setTo_Pos1Rad_shouldBe_Pos1Rad()
      {
         Deflection aDefl = new Deflection();
         aDefl = (Deflection)1.0;
         Double expected = 1.0;
         Double actual = aDefl.getAsRadians();
         Assert.AreEqual(expected: expected, actual: actual, delta: delta);
      }

      [Test]
      public void Deflection_setTo_Neg1Rad_shouldBe_Neg1Rad()
      {
         Deflection aDefl = new Deflection();
         aDefl = (Deflection) (-1.0);
         Double expected = -1.0;
         Double actual = aDefl.getAsRadians();
         Assert.AreEqual(expected: expected, actual: actual, delta: delta);
      }

      [Test]
      public void Deflection_setTo_Pos6Rad_shouldBe_Neg0_28318Rad()
      {
         Deflection aDefl = new Deflection();
         aDefl = (Deflection) 6.0;
         Double expected = -0.283185307181;
         Double actual = aDefl.getAsRadians();
         Assert.AreEqual(expected: expected, actual: actual, delta: delta);
      }

      [Test]
      public void Deflection_setTo_Pos2_shouldBe_Pos2Degrees()
      {
         Deflection defl = new Deflection();
         defl.setFromDegrees(2.0);

         Double expected = 2.0;
         Double actual = defl.getAsDegrees();
         Assert.AreEqual(expected: expected, actual: actual, delta: delta);
      }

      [Test]
      public void Deflection_setTo_neg5__18__29_5()
      {
         Deflection aDeflection = new Deflection();
         aDeflection.setFromDegreesMinutesSeconds(-5, 18, 29.5);
         Double expected = -5.30811111111;
         Double actual = aDeflection.getAsDegrees();
         Assert.AreEqual(expected: expected, actual: actual, delta: delta);
      }

      [Test]
      public void Azimuth1_30_addDeflection_Pos2_15_shouldYieldNewAzimuth_3_45()
      {
         Azimuth anAzimuth = new Azimuth();
         anAzimuth.setFromDegreesMinutesSeconds(1, 30, 0);
         Deflection aDefl = new Deflection();
         aDefl.setFromDegreesMinutesSeconds(2, 15, 0);

         Double expected = 3.75;
         Azimuth newAz = anAzimuth + aDefl;
         Double actual = newAz.getAsDegrees();
         Assert.AreEqual(expected: expected, actual: actual, delta: delta);
      }

      [TestCase(10, 2, 78.690067526)]
      [TestCase(10, -2, 101.309932474)]
      [TestCase(-10, 2, 281.309932474)]
      [TestCase(-10, -2, 258.690067526)]
      public void Azimuth_setFromXY(Double x, Double y, Double expectedDegrees)
      {
         Azimuth anAzimuth = new Azimuth();
         anAzimuth.setFromXY(x, y);
         Double actualDegrees = anAzimuth.getAsDegrees();

         Assert.AreEqual(expected: expectedDegrees, actual: actualDegrees, delta: delta);
      }


   }
}
