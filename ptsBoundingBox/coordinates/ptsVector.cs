using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace ptsCogo
{
   [Serializable]
   public class ptsVector
   {
      public double x { get; set; }
      public double y { get; set; }
      public double z { get; set; }

      public ptsVector() { }
      public ptsVector(double x_, double y_, double z_)
      {
         x = x_; y = y_; z = z_;
      }

      public ptsPoint plus(ptsPoint aPoint)
      {
         if (aPoint.isEmpty)
         {
            return new ptsPoint();
         }

         return new ptsPoint(aPoint.x + this.x, aPoint.y + this.y, aPoint.z + this.z);
      }

      public double dotProduct(ptsVector otherVec)
      {
         return (this.x * otherVec.x) + (this.y * otherVec.y) + (this.z * otherVec.z);
      }

      public ptsVector crossProduct(ptsVector otherVec)
      {
         ptsVector newVec = new ptsVector();
         newVec.x = this.y * otherVec.z - this.z * otherVec.y;
         newVec.y = this.z * otherVec.x - this.x * otherVec.z;
         newVec.z = this.x * otherVec.y - this.y * otherVec.x;
         return newVec;
      }

   }
}
