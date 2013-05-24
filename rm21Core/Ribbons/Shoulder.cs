using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ptsCogo.coordinates.CurvilinearCoordinates;
using ptsCogo;
using ptsCogo.Angle;
using System.Windows.Media;

namespace ptsCogo.Ribbons
{
   public class Shoulder: ribbonBase
   {
      public Shoulder(CogoStation beginStation, CogoStation endStation, double initialWidth, Slope initialSlope)
         : base(beginStation, endStation, initialWidth, initialSlope) { }

      public override string getHashName() { return "Shoulder"; }

      public override void DrawCrossSection(IRM21cad2dDrawingContext cadContext,
         ref StationOffsetElevation aSOE, int whichSide)
      {
         base.setupCrossSectionDrawing(cadContext);
         cadContext.setElementColor(Color.FromArgb(255, 54, 215, 54));
         cadContext.setElementWeight(1.1);
         base.DrawCrossSection(cadContext, ref aSOE, whichSide);
      }

      public override void DrawPlanViewSchematic(IRM21cad2dDrawingContext cadContext, int whichSide)
      {
         cadContext.setElementColor(Color.FromArgb(255, 54, 215, 54));
         cadContext.setElementWeight(1.1);
         base.DrawPlanViewSchematic(cadContext, whichSide);
      }

   }
}
