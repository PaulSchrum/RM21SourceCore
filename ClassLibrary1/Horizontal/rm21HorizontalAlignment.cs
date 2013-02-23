using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ptsCogo.Horizontal
{
   public class rm21HorizontalAlignment : HorizontalAlignmentBase
   {
      

      public rm21HorizontalAlignment(List<IRM21fundamentalGeometry> fundamentalGeometryList,
         String Name, List<Double> stationEquationing)
         : base(stationEquationing)
      {
         if (null == fundamentalGeometryList)
         { throw new NullReferenceException("Fundamental Geometry List may not be null."); }

      }

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
