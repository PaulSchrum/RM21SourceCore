using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ptsCogo.coordinates.CurvilinearCoordinates;
using ptsCogo;
using ptsCogo.Angle;


namespace rm21Core
{
   public class PGLGrouping
   {
      private IRibbonLike PGLoffsetRibbon;  
      // offset from 3d space curve to the Profile Grade Line
      
      private LinkedList<IRibbonLike> outsideRibbons;  
      // All elements from the PGL toward the outside.  This is to the right
      // for the right PGLGrouping (myIndex > 0)
      // and to the left for the left PGLGrouping (myIndex < 0
      
      private LinkedList<IRibbonLike> insideRibbons;
      // All element from the PGL toward the inside.  This is to the left
      // for the right PGLGrouping and to the right for the left PGLGrouping.

      public int myIndex { get; set; }  // Should never be left as 0
      public Profile pglProfile { get; set; }
      // public HorizontalAlignment horizAlignment { get; set; }


   }

}
