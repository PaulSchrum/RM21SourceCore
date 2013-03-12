using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ptsCogo.Angle
{
   sealed public class Deflection : ptsAngle
   {
      public Deflection() { }

      public Deflection(double anAngleDbl)
      {
         //base.angle__ = anAngleDbl;
         angle_ = anAngleDbl;
      }

      public Deflection(ptsAngle anAngle)
      {
         angle_ = anAngle.angle_;
      }

      public Deflection(ptsVector v1, ptsVector v2)
      {
         this.normalize(v2.Azimuth.angle_ - v1.Azimuth.angle_);
      }

      public double getAsRadians_NormalizedTo360() 
      {
         double returnAngleRadians = angle_;
         if (returnAngleRadians < 0.0)
            returnAngleRadians += 2.0 * Math.PI;

         return returnAngleRadians;
      }

   }
}
