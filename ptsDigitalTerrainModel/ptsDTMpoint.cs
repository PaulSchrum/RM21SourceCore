using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ptsCogo;

namespace ptsDigitalTerrainModel
{
   [Serializable]
   public struct efficientPoint
   {
      public efficientPoint(String Index, String X, String Y, String Z)
      {
         index = UInt64.Parse(Index);
         x = Double.Parse(X);
         y = Double.Parse(Y);
         z = Double.Parse(Z);
      }

      public UInt64 index;
      public Double x;
      public Double y;
      public Double z;
   }

   [Serializable]
   public class ptsDTMpoint : ptsCogo.ptsPoint
   {
      //public UInt64 myIndex { get; private set; }

      [NonSerialized]
      private static String[] parsedStrings;

      public ptsDTMpoint(double newX, double newY, double newZ)
      { x = newX; y = newY; z = newZ; } //myIndex = 0L; }

      public ptsDTMpoint(String ptAsString, UInt64 myIndx)
      {
         parsedStrings = ptAsString.Split(' ');
         Double.TryParse(parsedStrings[0], out base.x_);
         Double.TryParse(parsedStrings[1], out base.y_);
         Double.TryParse(parsedStrings[2], out base.z_);
         //myIndex = myIndx;
      }

      private ptsDTMpoint() { }

      public ptsDTMpoint(String x, String y, String z)
         : base(x, y, z)
      {  }  //myIndex = 0L;   }

      static public ptsDTMpoint getAveragePoint(ptsDTMpoint pt1, ptsDTMpoint pt2, ptsDTMpoint pt3)
      {
         ptsDTMpoint returnPoint = new ptsDTMpoint();
         returnPoint.x_ = (pt1.x_ + pt2.x_ + pt3.x_) / 3.0;
         returnPoint.y_ = (pt1.y_ + pt2.y_ + pt3.y_) / 3.0;
         returnPoint.z_ = (pt1.z_ + pt2.z_ + pt3.z_) / 3.0;
         return returnPoint;
      }
   }
}
