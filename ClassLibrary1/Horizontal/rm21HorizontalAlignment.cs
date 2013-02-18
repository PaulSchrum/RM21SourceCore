using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ptsCogo.Horizontal
{
   public class rm21HorizontalAlignment : GenericAlignment
   {

      public void getCrossSectionEndPoints(
         CogoStation station, 
         out ptsPoint leftPt, 
         double leftOffset, 
         out ptsPoint rightPt, 
         double rightOffset)
      {
         leftPt = null;
         rightPt = null;
      }
   }
}
