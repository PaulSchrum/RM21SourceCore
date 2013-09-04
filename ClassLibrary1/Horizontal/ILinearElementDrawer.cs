using ptsCogo.Angle;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ptsCogo.Horizontal
{
   public interface ILinearElementDrawer
   {
      void setDrawingStateTemporary();
      void setDrawingStatePermanent();
      void drawLineSegment(ptsPoint startPt, ptsPoint endPt);
      void drawArcSegment(ptsPoint startPt, ptsPoint centerPt, ptsPoint endPt, Double deflection);
      //void drawEulerSpiralSegment(List<ptsPoint> allPoints, Double offset);
      void setAlignmentValues(
         int itemIndex, 
         String BegSta, 
         String Length, 
         String Azimuth, 
         String Radius, 
         String Deflection);

   }
}
