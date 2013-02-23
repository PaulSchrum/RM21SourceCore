using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ptsCogo;

namespace ptsCogo.Horizontal
{
   public interface IRM21fundamentalGeometry
   {
      List<ptsPoint> getPointList();
      Double getBeginningDegreeOfCurve();
      Double getEndingDegreeOfCurve();
      expectedType getExpectedType();

   }

   public enum expectedType 
   { 
      LineSegment, 
      ArcSegmentInsideSolution, 
      ArcSegmentOutsideSoluion,
      ArcHalfCircle,
      EulerSpiral 
   };
}
