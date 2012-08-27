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
      // Note: allRibbons always contains allRibbons[0], which is the PGL Offset
      // ribbon, even if it has width 0 for the entire length of the chain
      private ribbonCollection allRibbons;

      public Profile pglProfile { get; set; }


      // public HorizontalAlignment theHorizontalAlignment { get; set; }
      /// <summary>
      /// Collection of all ribbons in the PGL Grouping
      /// </summary>
      /// <remarks>
      /// All Ribbon Collections are required to have a ribbon[0], which
      /// is the "phantom ribbon" that represents the offset from the 
      /// Space Curve to the profile grade line.  If one is not provided,
      /// then one is created by default which has its width == 0.0 
      /// throughout it length.
      /// </remarks>
      private class ribbonCollection : Dictionary<int, IRibbonLike>
      {
         public int getMaxIndex()
         {
            int returnValue = 0;
            while (base.ContainsKey(returnValue++) == true) { }

            return returnValue - 1;
         }

         public int getMinIndex()
         {
            int returnValue = 0;
            while (base.ContainsKey(returnValue--) == true) { }

            return returnValue + 1;
         }

         public int getNextIndexBySide(int side)
         {
            int direction = (Math.Sign(side) > 0) ? 1 : -1;
            int returnValue = 0;
            while (base.ContainsKey(returnValue) == true) { returnValue += direction; }

            return returnValue;
         }

         public void Add(int side, IRibbonLike newRibbonLike)
         {
            base.Add(this.getNextIndexBySide(side), newRibbonLike);
         }

         public void Insert(int indexInsertBefore, IRibbonLike newRibbonLike)
         {
            throw new NotImplementedException();  // to do: implement this next
         }
      }
   }

}
