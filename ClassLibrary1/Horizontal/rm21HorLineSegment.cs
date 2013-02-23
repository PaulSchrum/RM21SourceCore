using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ptsCogo.Horizontal
{
   public class rm21HorLineSegment : HorizontalAlignmentBase
   {
      public ptsPoint BeginPoint { get; set; }
      public ptsPoint EndPoint { get; set; }

      public rm21HorLineSegment(ptsPoint begPt, ptsPoint endPt) : base()
      {
         BeginPoint = begPt;
         EndPoint = endPt;
      }

      public Double Length
      {
         get
         {
            return BeginPoint.GetHorizontalDistanceTo(EndPoint);  //next: add list and list management to rm21HorizontalAlignment class
         }
         private set { }
      }
   }
}
