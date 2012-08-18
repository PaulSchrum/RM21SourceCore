using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ptsCogo.coordinates.CurvilinearCoordinates;
using ptsCogo;



namespace rm21Core
{
   public abstract class ribbonBase
   {
      public Profile Widths { get; private set; }
      internal Profile interpretWidths { get; set; }
      public Profile CrossSlopes { get; private set; }
      internal Profile interpretCrossSlopes { get; set; }

      public void accumulateRibbonTraversal(ref StationOffsetElevation aSOE)
      {
         aSOE.station = 1.2;
         aSOE.elevation = (Elevation) 2.2;
         aSOE.offset = (Offset) 55.5;
      }
   }

   public enum enmWidthInterpret
   {
      Width = 1,     // most common case: the elevation in Profile Widths is the horizontal width of the ribbon
      Height = 2,    // the elevation in Profile Widths is the height of the ribbon
      Hybrid = 3,    // when slope < 45 degrees, interpret as width; when slope > 45 degrees, intepret as height
      RaySheet = 4    // the ribbon is really a ray sheet. Resolve down the slope of the ray until intersecting another surface
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
