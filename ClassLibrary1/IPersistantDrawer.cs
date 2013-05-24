using ptsCogo.Horizontal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ptsCogo
{
   public interface IPersistantDrawer
   {
      void PlaceLine(rm21HorLineSegment lineSegment, ptsPoint startPoint, ptsPoint endPoint);
      void PlaceArc(rm21HorArc arc, ptsPoint startPoint, ptsPoint endPoint);
   }
}
