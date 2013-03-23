using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ptsCogo;


namespace ptsCogo.coordinates.CurvilinearCoordinates
{
   public class StationOffsetElevation
   {
      //private StationOffsetElevation soePoint;

      public StationOffsetElevation() { offset = new Offset(0.0); elevation = new Elevation(0.0); }
      public StationOffsetElevation(double aStation, Offset anOffset, Elevation anElevation)
      {
         station = aStation; offset = anOffset; elevation = anElevation;
      }

      public StationOffsetElevation(StationOffsetElevation soeOther)
      {
         station = soeOther.station;
         offset = soeOther.offset;
         elevation = soeOther.elevation;
      }

      public double station{get; set;}
      public Offset offset { get; set; }
      public Elevation elevation { get; set; }

      public override string ToString()
      {
         return station.ToString() + " " + offset.ToString() + "  (EL: " + elevation.ToString() + ")";
      }
   }
}
