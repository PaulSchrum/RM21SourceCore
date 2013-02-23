using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ptsCogo
{
   [Serializable]
   public class ptsAngle
   {
      private double angle__;
      internal double angle_ { get { return angle__; } set { normalize(value); } }
      //private static double angleScratchPad;

      public double getAsRadians() { return angle_; }

      public virtual double getAsDegrees()
      {
         return 180.0 * angle_ / Math.PI;
      }

      public virtual void setFromDegrees(double degrees)
      {
         angle_ = Math.PI * degrees / 180.0;
      }

      public static double radiansFromDegree(double degrees)
      {
         return Math.PI * degrees / 180.0;
      }

      public virtual void setFromDegreesMinutesSeconds(int degrees, int minutes, double seconds)
      {
         setFromDegrees(
               (double) degrees + (double) minutes / 60.0 + seconds / 3600.0
                        );
      }

      public void setFromDMSstring(string angleInDMS)
      {
         throw new NotImplementedException();
      }

      public virtual void setFromXY(double x, double y)
      {
         double dbl = Math.Atan2(y, x);
         angle_ = dbl;
         //angle_ = Math.Atan2(y, x);
      }

      protected double fp(double val)
      {
         if (val > 0.0)
            return val - Math.Floor(val);
         else if (val < 0.0)
            return -1.0 * (val - Math.Floor(val));
         else
            return 0.0;
      }

      protected void normalize(double anAngle)
      {
         //angleScratchPad = anAngle / Math.PI;
         //angle__ = fp(angleScratchPad) * Math.PI;
         double oldAngle = anAngle * 180 / Math.PI;

         // approach to normalize #1, probably too slow
         angle__ = Math.Atan2(Math.Sin(anAngle), Math.Cos(anAngle));

         // approach to normalizae #2, can't get it to work -- will one day
         //int sign = Math.Sign(anAngle);
         //angleScratchPad = (anAngle * sign) / Math.PI;
         //angle__ = (fp(angleScratchPad) * sign) * Math.PI;

      }

      public override string ToString()
      {
         return (angle__ * 180 / Math.PI).ToString();
      }

      public static ptsAngle operator +(ptsAngle angle1, ptsAngle angle2)
      {
         var retVal = new ptsAngle();
         retVal.normalize(angle1.angle__ + angle2.angle__);
         return retVal;
      }

      public static ptsAngle operator -(ptsAngle angle1, ptsAngle angle2)
      {
         var retVal = new ptsAngle();
         retVal.normalize( angle1.angle__ - angle2.angle__);
         return retVal;
      }
      
      // operator overloads
      public static implicit operator ptsAngle(double angleAs_double)
      {
         ptsAngle anAngle = new ptsAngle();
         anAngle.angle_ = angleAs_double;
         return anAngle;
      }

      public static implicit operator ptsAngle(ptsVector angleAs_vector)
      {
         ptsAngle anAngle = new ptsAngle();
         anAngle.angle__ = Math.Atan2(angleAs_vector.y, angleAs_vector.x);
         return anAngle;
      }

   }
}
