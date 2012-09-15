using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ptsCogo.coordinates.CurvilinearCoordinates;

namespace rm21Core
{
   public interface IRibbonLike
   {
      int getChildRibbonCount();
      IRibbonLike getChildRibbonByIndex(int index);
      void accumulateRibbonTraversal(ref StationOffsetElevation aSOE);

      int getMyIndex();
      void setMyIndex(int index);
      void incrementMyIndex();
   }
}
