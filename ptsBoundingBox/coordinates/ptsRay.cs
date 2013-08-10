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
      private int advanceDirection_  = 1;
      public int advanceDirection 
      { get { return advanceDirection_; } 
         set 
         {
            advanceDirection_ = Math.Sign(value);
            if (0 == value) advanceDirection_ = 1;
         } 
      }
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

      public double get_m() { return this.Slope * this.advanceDirection; }
      public double get_b()
      {
         if (true == Slope.isVertical())
            return Double.NaN;

         return this.StartPoint.z - (StartPoint.x * Slope.getAsSlope() * this.advanceDirection);
      }

      public bool isWithinDomain(double testX)
      {
         if (true == Slope.isVertical())
            return (testX == this.StartPoint.x);

         int sign = Math.Sign(testX - this.StartPoint.x);

         if (Math.Sign(testX - this.StartPoint.x) == this.advanceDirection )
            return true;

         return false;
      }

      public double getOffset(ptsPoint endPt)
      {
         ptsVector directVectr = endPt - this.StartPoint;
         ptsAngle alpha =  this.HorizontalDirection - directVectr;
         Double offset = -1.0 * directVectr.Length * Math.Sin(alpha.getAsRadians());
         return offset;
      }
   }
}
