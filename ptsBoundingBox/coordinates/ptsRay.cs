using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ptsCogo.Angle;

namespace ptsCogo.coordinates
{
   public class ptsRay
   {
      public ptsPoint StartPoint { get; set; }
      public Slope Slope { get; set; }
      private bool advanceForward_  = true;
      public bool advanceForward { get { return advanceForward_; } set { advanceForward_ = value; } }
      public Azimuth HorizontalDirection { get; set; }

      public double? getElevationAlong(double X)
      {
         if (true == Slope.isVertical())
            return null;

         double horizDistance = X - StartPoint.x;

         if (Math.Sign(horizDistance) != Math.Sign(Slope.getAsSlope()))
            return null;

         return (double?)
            ((horizDistance * Slope.getAsSlope()) + this.StartPoint.z);

      }

      public double get_m() { return this.Slope; }
      public double get_b()
      {
         if (true == Slope.isVertical())
            return Double.NaN;

         return this.StartPoint.z - (StartPoint.x * Slope.getAsSlope());
      }

      public bool isWithinDomain(double testX)
      {
         if (true == Slope.isVertical())
            return (testX == this.StartPoint.x);

         if (Math.Sign(testX - this.StartPoint.x) == Math.Sign(this.Slope.getAsSlope()))
            return true;

         return false;
      }
   }
}
