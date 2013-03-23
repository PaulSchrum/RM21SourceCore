using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ptsCogo.Angle
{
   sealed public class Azimuth : ptsAngle
   {
      public Azimuth() { }

      public Azimuth(double anAngleDbl)
      {
         angle_ = anAngleDbl;
      }

      public Azimuth(ptsPoint beginPt, ptsPoint endPt)
      {
         this.angle__ = Math.Atan2(endPt.y - beginPt.y, endPt.x - beginPt.x);
      }

      public new double angle_ { get { return getAsAzimuth(); } set { base.normalize(value); } }
      
      //public override void setFromXY(double x, double y)
      //{
      //   double dbl = Math.Atan2(x, y);
      //   angle_ = dbl;
      //}

      private double getAsAzimuth()
      {
         double retVal;

         retVal = (-1.0 * base.angle_) + (Math.PI / 2.0);

         return retVal;
      }

      public override string ToString()
      {
         return this.getAsDegrees().ToString();
      }

      public override double getAsDegrees()
      {
         double retValueDbl = getAsAzimuth() * 180 / Math.PI;
         return retValueDbl >= 0.0 ? retValueDbl : retValueDbl + 360.0;
      }

      public override void setFromDegrees(double degrees)
      {
         //double adjustedDegrees = ((degrees / -180.0)+ 1) *180.0;
         double radians = degrees * Math.PI / 180.0;
         angle_ = Math.Atan2(Math.Cos(radians), Math.Sin(radians));  // This is flipped intentionally

      }

      public override void setFromDegreesMinutesSeconds(int degrees, int minutes, double seconds)
      {
         setFromDegrees(
               (double)degrees + (double)minutes / 60.0 + seconds / 3600.0
                        );
      }

      public static int getQuadrant(double angleDegrees)
      {
         return (int)Math.Round((angleDegrees / 90.0) + 0.5);
      }

      //to do:
      //setAsAzimuth
      //getAsDegreeMinuteSecond
      //setAsDegree
      //setAsDegreeMinuteSecond
      //yada

      //public void fromCastOf(

      // operator overloads
      public static implicit operator Azimuth(double angleAs_double)
      {
         Azimuth anAzimuth = new Azimuth();
         anAzimuth.angle_ = angleAs_double;
         return anAzimuth;
      }

      //public static explicit operator Azimuth(ptsAngle asAngle)
      //{
      //   Azimuth anAzimuth = new Azimuth();
      //   anAzimuth.angle_ = asAngle.angle_;
      //   return anAzimuth;
      //}
      public static Azimuth operator +(Azimuth anAz, ptsAngle anAngle)
      {
         return new Azimuth(anAz.getAsRadians() - anAngle.getAsRadians());  // Note: Subtraction is intentional since azimuths are clockwise
      }

      public static double operator -(Azimuth Az1, Azimuth Az2)
      {
         //Double returnDeflection = ptsAngle.normalizeToPlusOrMinus2PiStatic(Az1.angle_ - Az2.angle_);
         Double returnDeflection = (Az1.angle_ - Az2.angle_);
         if (returnDeflection < 0.0)
            returnDeflection += 2*Math.PI;

         return ptsAngle.normalizeToPlusOrMinus2PiStatic(returnDeflection);
      }
   }
}
