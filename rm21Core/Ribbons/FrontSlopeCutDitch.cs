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
   public class FrontSlopeCutDitch : ribbonBase
   {
      public FrontSlopeCutDitch(CogoStation beginStation, CogoStation endStation, double initialWidth, Slope initialSlope)
         : base(beginStation, endStation, initialWidth, initialSlope) { }

      public override string getHashName() { return "Ditch Front Slope"; }

      public override void DrawCrossSection(IRM21cad2dDrawingContext cadContext,
         ref StationOffsetElevation aSOE, int whichSide)
      {
         base.setupCrossSectionDrawing(cadContext);
         cadContext.setElementColor(Color.FromArgb(255, 40, 123, 54));
         cadContext.setElementWeight(1.1);
         base.DrawCrossSection(cadContext, ref aSOE, whichSide);
      }

   }
}

