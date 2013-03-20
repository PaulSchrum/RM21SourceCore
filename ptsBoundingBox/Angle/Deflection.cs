using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ptsCogo.Angle
{
   sealed public class Deflection : ptsAngle
   {
      public Deflection() { }

      public Deflection(Azimuth BeginAzimuth, Azimuth EndAzimuth, bool assumeDeflectionIsLessThan180Degrees)
      {
         if (true == assumeDeflectionIsLessThan180Degrees)
         {
            this.angle__ = EndAzimuth - BeginAzimuth;
            if (this.angle__ > Math.PI)
               this.angle__ -= 2 * Math.PI;
            else if (this.angle__ < -Math.PI)
               this.angle__ += 2 * Math.PI;
         }
         else
         {
            throw new NotImplementedException();
         }
      }

      public static Deflection ctorDeflectionFromAngle(double angleDegrees)
      {
         return new Deflection(ptsAngle.radiansFromDegree(angleDegrees));
      }

      public Deflection(double anAngleDbl)
      {
         base.angle__ = ptsAngle.normalizeToPlusOrMinus2PiStatic(anAngleDbl); // that last test -- problem is here.
         //angle_ = anAngleDbl;
      }

      public Deflection(ptsAngle anAngle)
      {
         angle_ = anAngle.angle_;
      }

   }
}
