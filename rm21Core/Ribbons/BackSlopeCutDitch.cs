using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ptsCogo.coordinates.CurvilinearCoordinates;
using ptsCogo;
using ptsCogo.Angle;
using System.Windows.Media;


namespace rm21Core.Ribbons
{
   public class BackSlopeCutDitch : RaySheet
   {
      public BackSlopeCutDitch(CogoStation beginStation, CogoStation endStation, Slope initialSlope)
         : base(beginStation, endStation, initialSlope) { }

      public override string getHashName() { return "Ditch Back Slope"; }

      public override void DrawCrossSection(IRM21cad2dDrawingContext cadContext,
         ref StationOffsetElevation aSOE, int whichSide)
      {
         base.setupCrossSectionDrawing(cadContext);
         cadContext.setElementColor(Color.FromArgb(255, 40, 123, 54));
         cadContext.setElementWeight(1.1);
         base.DrawCrossSection(cadContext, ref aSOE, whichSide);
      }

      public override void DrawPlanViewSchematic(IRM21cad2dDrawingContext cadContext, int whichSide)
      {
         cadContext.setElementColor(Color.FromArgb(255, 40, 123, 54));
         cadContext.setElementWeight(1.1);
         base.DrawPlanViewSchematic(cadContext, whichSide);
      }
   }
}
