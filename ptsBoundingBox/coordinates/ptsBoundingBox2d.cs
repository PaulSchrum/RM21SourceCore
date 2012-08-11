using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ptsCogo
{
   [Serializable]
   public class ptsBoundingBox2d
   {
      public ptsPoint lowerLeftPt { get; set; }
      public ptsPoint upperRightPt { get; set; }

      private ptsBoundingBox2d() { }

      public ptsBoundingBox2d(ptsPoint aPoint)
      {
         lowerLeftPt = new ptsPoint(aPoint);
         upperRightPt = new ptsPoint(aPoint);
      }

      public void expandByPoint(ptsPoint aPoint)
      {
         if (lowerLeftPt.isEmpty == true)
         {
            lowerLeftPt = new ptsPoint(aPoint);
            upperRightPt = new ptsPoint(aPoint);
         }
         else
         {
            if (aPoint.x < lowerLeftPt.x)
               lowerLeftPt.x = aPoint.x;
            if (aPoint.y < lowerLeftPt.y)
               lowerLeftPt.y = aPoint.y;
            if (aPoint.z < lowerLeftPt.z)
               lowerLeftPt.z = aPoint.z;

            if (aPoint.x > upperRightPt.x)
               upperRightPt.x = aPoint.x;
            if (aPoint.y > upperRightPt.y)
               upperRightPt.y = aPoint.y;
            if (aPoint.z > upperRightPt.z)
               upperRightPt.z = aPoint.z;
         }
      }

      public bool isPointInsideBB2d(ptsPoint testPoint)
      {
         if (testPoint.x < lowerLeftPt.x)
            return false;
         if (testPoint.y < lowerLeftPt.y)
            return false;

         if (testPoint.x > upperRightPt.x)
            return false;
         if (testPoint.y > upperRightPt.y)
            return false;

         return true;
      }

      public bool isPointInsideBB3d(ptsPoint testPoint)
      {
         if (isPointInsideBB2d(testPoint) == false)
            return false;

         if (testPoint.z < lowerLeftPt.z)
            return false;

         if (testPoint.z > upperRightPt.z)
            return false;

         return true;
      }
   }
}
