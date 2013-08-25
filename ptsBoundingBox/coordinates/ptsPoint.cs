using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
//using System.IO;
//using System.Runtime.Serialization.Formatters.Binary;

namespace ptsCogo
{
   [Serializable]
   public class ptsPoint
   {
      protected double x_; protected double y_; protected double z_;
      protected bool isEmpty_;

      public double x { get { return x_; } set { x_ = value; } }
      public double y { get { return y_; } set { y_ = value; } }
      public double z { get { return z_; } set { z_ = value; } }
      public bool isEmpty { get { return isEmpty_; } }

      public ptsPoint() { isEmpty_ = true; }

      public ptsPoint(ptsPoint otherPt)
      {
         isEmpty_ = false;
         x = otherPt.x; y = otherPt.y; z = otherPt.z;
      }

      public ptsPoint(double X, double Y, double Z)
      {
         isEmpty_ = false;
         x = X; y = Y; z = Z;
      }

      public ptsPoint(double X, double Y)
         : this(X, Y, 0.0)
      {

      }

      public static ptsVector operator -(ptsPoint p1, ptsPoint p2)
      {
         return new ptsVector(p2.x - p1.x, p2.y - p1.y, p2.z - p1.z);
      }

      public static ptsPoint operator +(ptsPoint point, ptsVector vector)
      {
         // dont know why I did this:  return new ptsPoint(point.x + vector.y, point.y + vector.x, point.z + vector.z);
         return new ptsPoint(point.x + vector.x, point.y + vector.y, point.z + vector.z);
      }

      public Double GetHorizontalDistanceTo(ptsPoint other)
      {
         Double dx = other.x - this.x;
         Double dy = other.y - this.y;
         return Math.Sqrt(dx * dx + dy * dy);
      }

      public int compareByXthenY(ptsPoint other)
      {
         int xComp; int yComp;
         xComp = this.x_.CompareTo(other.x_);
         if (xComp == 0)
         {
            yComp = this.y_.CompareTo(other.y_);
            return yComp;
         }
         return xComp;
      }

      public int compareByYthenX(ptsPoint other)
      {
         int xComp; int yComp;
         yComp = this.y_.CompareTo(other.y_);
         if (yComp == 0)
         {
            xComp = this.x_.CompareTo(other.x_);
            return xComp;
         }
         return yComp;
      }

      public override string ToString()
      {
         return x.ToString(NumberFormatInfo.InvariantInfo) + ", " + y.ToString(NumberFormatInfo.InvariantInfo) + ", " +
            z.ToString(NumberFormatInfo.InvariantInfo);
      }

   }
}
