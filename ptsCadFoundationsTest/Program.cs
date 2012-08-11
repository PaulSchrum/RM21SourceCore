using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ptsCogo;
using ptsCogo.Angle;

namespace ptsCadFoundationsTest
{
   class Program
   {
      static void Main(string[] args)
      {
         double dblAngle = 112 * Math.PI / 180.0;
         ptsAngle anAngle = dblAngle;
         anAngle.setFromDegrees(30);

         Console.Write("anAngle = ");
         Console.WriteLine(anAngle.getAsDegrees());
         Console.WriteLine(anAngle.getAsRadians());

         Console.WriteLine();
         Azimuth anAzimuth = new Azimuth(); Deflection aDefl = new Deflection();
         anAzimuth.setFromDegreesMinutesSeconds(183, 29, 29.5);
         aDefl.setFromDegreesMinutesSeconds(-5, 18, 29.5);
         anAzimuth = anAzimuth + aDefl;

         Console.WriteLine();
         anAzimuth.setFromXY(-1, -0.1);
         Console.Write("Azimuth = ");
         Console.WriteLine(anAzimuth.getAsDegrees());
         Console.WriteLine(anAzimuth.getAsRadians());

         Console.WriteLine();
         Console.ReadLine();
      }
   }
}
