using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ptsCogo.coordinates.CurvilinearCoordinates;
using ptsCogo;
using ptsCogo.Angle;



namespace rm21Core
{
   public abstract class ribbonBase
   {
      public Profile Widths { get; private set; }
      internal Profile interpretWidths { get; set; }
      public Profile CrossSlopes { get; private set; }
      internal Profile interpretCrossSlopes { get; set; }

      public ribbonBase() { }

      public ribbonBase(CogoStation beginStation, CogoStation endStation, double initialWidth, Slope initialSlope)
      {
         interpretWidths = new Profile(beginStation, endStation, (double) enmWidthInterpret.HorizontalOnly);
         Widths = new Profile(beginStation, endStation, initialWidth);
         interpretCrossSlopes = new Profile(beginStation, endStation, (double)enmCrossSlopeInterpret.xPercentage);
         CrossSlopes = new Profile(beginStation, endStation, initialSlope);
      }

      public void accumulateRibbonTraversal(ref StationOffsetElevation aSOE)
      {
         aSOE.station = 1.2;
         aSOE.elevation = (Elevation) 2.2;
         aSOE.offset = (Offset) 55.5;
      }

      public double? getActualWidth(CogoStation aStation, out tupleNullableDoubles result)
      {
         Widths.getElevation(aStation, out result);
         if (result.back != null && result.isSingleValue == false)
         {
            return result.ahead;
         }
         return result.back;
      }

   }

   public enum enmWidthInterpret
   {
      HorizontalOnly = 1, // most common case: the elevation in Profile Widths is the horizontal width of the ribbon
      VerticalOnly = 2,   // the elevation in Profile Widths is the height of the ribbon
      Hybrid = 3,         // when slope < 45 degrees, interpret as width; when slope > 45 degrees, intepret as height
      LengthAlong = 4,    // length along the element given its slope: the tangent instead of the cosine
      RaySheet = 5,       // the ribbon is really a ray sheet. Resolve down the slope of the ray until intersecting another surface
   }

   public enum enmCrossSlopeInterpret
   {
      xPercentage = 1,
      xTo1 = 2,
      degrees = 3,
      yTo1 = 4,
      yPercentage = 5,
      straightLineInRelativeSpace = 6,
      straightLineInWorldSpace = 7
   }
}
