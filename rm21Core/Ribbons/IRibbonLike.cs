using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ptsCogo.coordinates.CurvilinearCoordinates;
using ptsCogo;
using ptsCogo.Angle;

namespace ptsCogo
{
   public interface IRibbonLike : Irm21TreeViewItemable
   {
      int getChildRibbonCount();
      IRibbonLike getChildRibbonByIndex(int index);
      void accumulateRibbonTraversal(ref StationOffsetElevation aSOE);
      void DrawCrossSection(IRM21cad2dDrawingContext cadContext,
         ref StationOffsetElevation aSOE, int whichSide);
      void DrawPlanViewSchematic(IRM21cad2dDrawingContext cadContext,
         int whichSide);

      void moveToOuterEdge(ref StationOffsetElevation aSOE, int whichSide);

      event EventHandler onOffsetsChanged;
      void setInsideRibbon(IRibbonLike insideRibbon);

      int getMyScaleFactor();
      int getMyIndex();
      void setMyIndex(int index);
      void setMyProgressionDirection(rm21Side side);
      void incrementMyIndex();

      Slope getCrossSlope(CogoStation aStation);
      Profile getOffsetProfile();

      void setPGLgroupingParent(PGLGrouping pglGrouping);
   }

}


