using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using ptsCogo;

namespace ptsDigitalTerrainModel
{
   [Serializable]
   public struct ptsDTMpoint //: ptsCogo.ptsPoint
   {
      public ulong myIndex { get; internal set; }
      public Double x { get; set; }
      public Double y { get; set; }
      public Double z { get; set; }

      //private int zAsInt;
      //public Double z 
      //{
      //   get { return ((Double)zAsInt) / 1000; }
      //   set { zAsInt = (int)(value * 1000); }
      //}

      [NonSerialized]
      private static String[] parsedStrings;

      public ptsDTMpoint(double newX, double newY, double newZ) : this()
      { x = newX; y = newY; z = newZ; } //myIndex = 0L; }

      public ptsDTMpoint(ulong ndex, double newX, double newY, double newZ) : 
         this(newX, newY, newZ)
      {
         myIndex = ndex;
      }

      public ptsDTMpoint(String ptAsString, UInt64 myIndx) : this()
      {
         parsedStrings = ptAsString.Split(' ');
         this.x = Double.Parse(parsedStrings[0]);
         this.y = Double.Parse(parsedStrings[1]);
         this.z = Double.Parse(parsedStrings[2]);
         myIndex = myIndx;
      }

      public ptsDTMpoint(String x, String y, String z) : this()
      {
         this.x = Double.Parse(x);
         this.y = Double.Parse(y);
         this.z = Double.Parse(z);
      }

      internal void SaveToSQLiteDB(SQLiteConnection conn)
      {
         StringBuilder SQLstring = new StringBuilder();
         SQLstring.Append("INSERT INTO points (pointID, x, y, z) ");
         SQLstring.AppendFormat("VALUES ('{0}', '{1}', '{2}', '{3}')", this.myIndex, x, y, z);
         var cmd = new SQLiteCommand(conn);
         cmd.CommandText = SQLstring.ToString();
         cmd.ExecuteNonQuery();
      }

      static public ptsDTMpoint getAveragePoint(ptsDTMpoint pt1, ptsDTMpoint pt2, ptsDTMpoint pt3)
      {
         return new ptsDTMpoint(
            (pt1.x + pt2.x + pt3.x) / 3.0,
            (pt1.y + pt2.y + pt3.y) / 3.0,
            (pt1.z + pt2.z + pt3.z) / 3.0
            );
      }

      public static ptsVector operator -(ptsDTMpoint p1, ptsDTMpoint p2)
      {
         return new ptsVector(p2.x - p1.x, p2.y - p1.y, p2.z - p1.z);
      }

      public static explicit operator ptsDTMpoint(ptsPoint aPt)
      {
         return new ptsDTMpoint(aPt.x, aPt.y, aPt.z);
      }
   }
}
