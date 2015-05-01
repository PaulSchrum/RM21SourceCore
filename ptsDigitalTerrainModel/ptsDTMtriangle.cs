using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using ptsCogo;
using ptsCogo.Angle;

namespace ptsDigitalTerrainModel
{
   [Serializable]
   internal class ptsDTMtriangle : IComparable
   {
      // temporary scratch pad members -- do not serialize
      [NonSerialized]
      private Dictionary<UInt64, ptsDTMpoint> allPoints;
      [NonSerialized]
      private UInt64[] indices = new UInt64[3];

      // substantitve fields - Do Serialize these
      public ulong index1 { get; set; }
      public ulong index2 { get; set; }
      public ulong index3 { get; set; }

      public ptsDTMpoint point1 { get { return allPoints[indices[0]]; } }
      public ptsDTMpoint point2 { get { return allPoints[indices[1]]; } }
      public ptsDTMpoint point3 { get { return allPoints[indices[2]]; } }

      public ptsVector normalVec 
      {
         get { return (point2 - point1).crossProduct(point3 - point1); } 
      }

      // non-substantive fields
      [NonSerialized]
      private ptsCogo.ptsBoundingBox2d myBoundingBox_;

      public ptsDTMtriangle(Dictionary<UInt64, ptsDTMpoint> pointList, string pointRefs)
      {
         this.allPoints = pointList;

         String[] indexStrings;
         indexStrings = pointRefs.Split(' ');
         UInt64.TryParse(indexStrings[0], out indices[0]);
         UInt64.TryParse(indexStrings[1], out indices[1]);
         UInt64.TryParse(indexStrings[2], out indices[2]);

         computeBoundingBox();
      }

      public ptsDTMtriangle(Dictionary<UInt64, ptsDTMpoint> pointList, UInt64 ptIndex1,
         UInt64 ptIndex2, UInt64 ptIndex3)
      {
         this.allPoints = pointList;
         this.index1 = ptIndex1;
         this.index2 = ptIndex2;
         this.index3 = ptIndex3;

         computeBoundingBox();
      }

      public void computeBoundingBox()
      {
         myBoundingBox_ = new ptsBoundingBox2d(point1.x, point1.y, point1.x, point1.y);
         myBoundingBox_.expandByPoint(point2.x, point2.y, point2.z);
         myBoundingBox_.expandByPoint(point3.x, point3.y, point3.z);
      }

      public bool isPointInBoundingBox(ptsDTMpoint aPoint)
      {
         return myBoundingBox_.isPointInsideBB2d(aPoint.x, aPoint.y);
      }

      #region IComparable Members

      /// <summary>
      /// Makes ptsDTMtriangles automatically sort itself based on x-axis order.
      /// </summary>
      /// <param name="obj"></param>
      /// <returns>int</returns>
      int IComparable.CompareTo(object obj)
      {
         ptsDTMtriangle other = (ptsDTMtriangle)obj;
         return this.myBoundingBox_.lowerLeftPt.compareByXthenY(other.myBoundingBox_.lowerLeftPt);
      }

      #endregion
      
      // adapted from
      // http://stackoverflow.com/questions/2049582/how-to-determine-a-point-in-a-triangle
      internal bool contains(ptsDTMpoint aPoint)
      {
         bool b1, b2, b3;

         b1 = sign(aPoint, point1, point2) < 0.0f;
         b2 = sign(aPoint, point2, point3) < 0.0f;
         b3 = sign(aPoint, point3, point1) < 0.0f;

         return ((b1 == b2) && (b2 == b3));
      }

      double sign(ptsDTMpoint p1, ptsDTMpoint p2, ptsDTMpoint p3)
      {
         return (p1.x - p3.x) * (p2.y - p3.y) - (p2.x - p3.x) * (p1.y - p3.y);
      }
      // End: adapted from

      public double givenXYgetZ(ptsDTMpoint aPoint)
      {
         // Use equation         ax + bx
         //                 z = ----------     taken from Wolfram Alpha
         //                        -c
         //
         //  where a is normalVec_.i, b is .j, and c is .k
         //    and X is aPoint.x - point1.x
         //    and Y is aPoint.y - point1.y
         //
         //  Ultimately add z to point1.z to get the elevation

         double X = aPoint.x - point1.x;
         double Y = aPoint.y - point1.y;

         double Z = ((normalVec.x * X) + (normalVec.y * Y)) / 
                     (-1.0 * normalVec.z);

         return Z + point1.z;
      }

      public double? givenXYgetSlopePercent(ptsPoint aPoint)
      {
         return givenXYgetSlopePercent((ptsDTMpoint)aPoint);
      }

      public double? givenXYgetSlopePercent(ptsDTMpoint aPoint)
      {
         var normalVector = this.normalVec;
         if (0.0 == normalVector.z) return null;

         return Math.Abs (100.0 *
            Math.Sqrt(normalVector.x * normalVector.x + normalVector.y * normalVector.y) /
                        normalVector.z);
      }

      public Azimuth givenXYgetSlopeAzimuth(ptsPoint aPoint)
      {
         return givenXYgetSlopeAzimuth((ptsDTMpoint)aPoint);
      }

      public Azimuth givenXYgetSlopeAzimuth(ptsDTMpoint aPoint)
      {
         Azimuth slopeAz = new Azimuth();
         slopeAz.setFromXY(this.normalVec.y, this.normalVec.x);

         return slopeAz; 
         
      }

      internal void SaveToSQLiteDB(System.Data.SQLite.SQLiteConnection conn)
      {
         StringBuilder SQLstring = new StringBuilder();
         SQLstring.Append("INSERT INTO triangles (indexPt1, indexPt2, indexPt3) ");
         SQLstring.AppendFormat("VALUES ('{0}', '{1}', '{2}')", this.indices[0], indices[1], indices[2]);
         var cmd = new SQLiteCommand(conn);
         cmd.CommandText = SQLstring.ToString();
         cmd.ExecuteNonQuery();
      }

      internal void WriteToFile(System.IO.StreamWriter outStream)
      {
         outStream.WriteLine(String.Format("{0} {1} {2}",
            this.indices[0], indices[1], indices[2]
            ));
      }
   }
}
