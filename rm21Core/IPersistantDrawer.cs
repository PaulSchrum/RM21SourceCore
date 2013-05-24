using ptsCogo.Elements.Paths;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace rm21Core
{
   public interface IPersistantDrawer
   {
      void PlaceLine(ptsLineSegment lineSegment);
      void PlaceArc(ptsArc arc);
   }
}
