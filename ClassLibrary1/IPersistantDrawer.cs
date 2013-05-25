using ptsCogo.coordinates.CurvilinearCoordinates;
using ptsCogo.Horizontal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ptsCogo
{
   public interface IPersistantDrawer
   {
      void PlaceLine(rm21HorLineSegment lineSegment, 
         ptsPoint startPoint, StationOffsetElevation startSOE,
         ptsPoint endPoint, StationOffsetElevation endSOE);

      void PlaceArc(rm21HorArc arc,
         ptsPoint startPoint, StationOffsetElevation startSOE,
         ptsPoint endPoint, StationOffsetElevation endSOE);
      
   }
}
