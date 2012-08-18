using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;



namespace ptsCogo.coordinates.CurvilinearCoordinates
{
   public class StationOffsetElevation
   {
      public double station{get; set;}
      public Offset offset { get; set; }
      public Elevation elevation { get; set; }
   }
}
