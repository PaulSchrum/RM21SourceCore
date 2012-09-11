using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ptsCogo;

namespace Test_GUI_Station.theStationModel
{
   public class ModelOfCogoStation
   {
      public CogoStation aStation { get; set; }
      public ModelOfCogoStation()
      {
         
      }

      public void Load()
      {
         aStation = (CogoStation)1200.0;
      }


   }
}
