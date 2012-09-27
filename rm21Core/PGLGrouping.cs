using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ptsCogo.coordinates.CurvilinearCoordinates;
using ptsCogo;
using ptsCogo.Angle;
using System.Collections.ObjectModel;



namespace rm21Core
{
   public class PGLGrouping : Irm21TreeViewItemable
   {
      // offset from 3d space curve to the Profile Grade Line
      private ribbonBase PGLoffsetRibbon_;
      public ribbonBase PGLoffsetRibbon { get; set; }
      
      private LinkedList<IRibbonLike> outsideRibbons;  
      // All elements from the PGL toward the outside.  This is to the right
      // for the right PGLGrouping (myIndex > 0)
      // and to the left for the left PGLGrouping (myIndex < 0
      
      private LinkedList<IRibbonLike> insideRibbons;
      //private int whichSide;

      public PGLGrouping(int whichSide)
      {
         myIndex = whichSide;
      }
      // All element from the PGL toward the inside.  This is to the left
      // for the right PGLGrouping and to the right for the left PGLGrouping.

      public int myIndex { get; set; }  // Should never be left as 0
      public Profile pglProfile { get; set; }
      // public HorizontalAlignment horizAlignment { get; set; }

      public void addOutsideRibbon(IRibbonLike newOutsideRibbon)
      {
         if (outsideRibbons == null)
            outsideRibbons = new LinkedList<IRibbonLike>();

         newOutsideRibbon.setMyIndex(outsideRibbons.Count);
         outsideRibbons.AddLast(newOutsideRibbon);
      }

      public void addInsideRibbon(IRibbonLike newInsideRibbon)
      {
         if (insideRibbons == null)
            insideRibbons = new LinkedList<IRibbonLike>();

         newInsideRibbon.setMyIndex(-1*insideRibbons.Count);
         insideRibbons.AddLast(newInsideRibbon);
      }

      public void setPGLoffsetRibbon(ribbonBase newPGLoffsetRibbon)
      {
         PGLoffsetRibbon_ = newPGLoffsetRibbon;
      }

      //public void accumulateRibbonTraversal(ref StationOffsetElevation aSOE);

      /// <summary>
      /// Gets the elevation of a point on the PGLgrouping
      /// if the point is on the grouping.  If the point is off the grouping to 
      /// the left, return -1.  If it is off the grouping to the right, 
      /// retrun +1.
      /// </summary>
      /// <param name="soePoint">Reads station and offset.
      /// Sets elevation.</param>
      /// <returns>0 if the point is on the PGLgrouping
      /// -1 if the point is left of the grouping
      /// +1 if the point is right of the grouping</returns>
      public int getElevationFromSOE(ref StationOffsetElevation soePoint)
      {
         StationOffsetElevation workingSOE = new StationOffsetElevation(soePoint);
         workingSOE.offset *= myIndex;

         // seek the correct ribbon
         if (PGLoffsetRibbon_ != null)
         {
            double? pglOffset = PGLoffsetRibbon_.getActualWidth((CogoStation)workingSOE.station);
            if (pglOffset != null)
               workingSOE.offset -= pglOffset;
         }
         if (workingSOE.offset > 0.0)
         {
            if (outsideRibbons == null) return 1;

            foreach (var aRibbon in outsideRibbons)
            {
               aRibbon.accumulateRibbonTraversal(ref workingSOE);
               if (workingSOE.offset <= 0.0) break;
            }
            if (workingSOE.offset > 0.0)
               return 1;
         }
         else if (workingSOE.offset < 0.0)
         {
            if (insideRibbons == null) return -1;

            workingSOE.offset *= -1.0;
            foreach (var aRibbon in insideRibbons)
            {
               aRibbon.accumulateRibbonTraversal(ref workingSOE);
               if (workingSOE.offset <= 0.0) break;
            }
            if (workingSOE.offset > 0.0)
               return -1;
         }
         else
            workingSOE.elevation = 0.0;

         soePoint.elevation = workingSOE.elevation;

         return 0;
      }

      public override string ToString()
      {
         if (myIndex > 0)
            return "PGL Grouping: +" + myIndex;

         return "PGL Grouping: " + myIndex;
      }

      public string getHashName()
      {
         return this.ToString();
      }

      public ObservableCollection<Irm21TreeViewItemable> getChildren()
      {
         ObservableCollection<Irm21TreeViewItemable> children = new ObservableCollection<Irm21TreeViewItemable>();
         children.Add(PGLoffsetRibbon);
         foreach (var child in insideRibbons) {children.Add(child);}
         foreach (var child in outsideRibbons) { children.Add(child); }
         return children;
      }
   }

}
