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

      public override Double Length
      {
         get
         {
            return BeginPoint.GetHorizontalDistanceTo(EndPoint);
         }
         protected set { }
      }
   }
}
