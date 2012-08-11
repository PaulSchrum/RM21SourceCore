using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ptsDigitalTerrainModel
{
   class ptsDTMtriangleLine
   {
      public ptsDTMpoint firstPoint { get; set; }
      public ptsDTMpoint secondPoint { get; set; }
      public ptsDTMtriangle oneTriangle { get; set; }
      public ptsDTMtriangle theOtherTriangle { get; set; }

      public ptsDTMtriangleLine(ptsDTMpoint pt1, ptsDTMpoint pt2, ptsDTMtriangle tngle)
      {
         firstPoint = pt1;
         secondPoint = pt2;
         oneTriangle = tngle;
         theOtherTriangle = null;
      }

   }
}
