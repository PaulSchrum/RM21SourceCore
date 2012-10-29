using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ptsCogo.coordinates.CurvilinearCoordinates;
using ptsCogo;
using ptsCogo.Angle;
using System.Windows.Media;


namespace rm21Core
{
   public class RoadwayLane : ribbonBase
   {
      public RoadwayLane(CogoStation beginStation, CogoStation endStation, double initialWidth, Slope initialSlope)
         : base(beginStation, endStation, initialWidth, initialSlope) { }

      public override string getHashName() { return "Roadway Lane"; }

      public override void DrawCrossSection(IRM21cad2dDrawingContext cadContext, 
         ref StationOffsetElevation aSOE, int whichSide)
      {
         base.setupCrossSectionDrawing(cadContext);
         cadContext.setElementColor(Color.FromArgb(255, 128, 128, 128));
         cadContext.setElementWeight(1.5);
         base.DrawCrossSection(cadContext, ref aSOE, whichSide);
      }

   }
}
