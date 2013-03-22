using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ptsCogo.Horizontal
{
   public class rm21HorLineSegment : HorizontalAlignmentBase
   {
      public rm21HorLineSegment(ptsPoint begPt, ptsPoint endPt)
         : base(begPt, endPt)
      {
         //this.BeginBearing = do this later
         //this.EndBearing = do this later
         this.BeginDegreeOfCurve = 0.0;
         this.EndDegreeOfCurve = 0.0;
      }

      public override Double Length
      {
         get
         {
            return BeginPoint.GetHorizontalDistanceTo(EndPoint);
         }
         protected set { }
      }

      public override StringBuilder createTestSetupOfFundamentalGeometry()
      {
         StringBuilder returnSB = new StringBuilder();

         return returnSB;
      }
   }
}
