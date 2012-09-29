using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ptsCogo;
using ptsCogo.Angle;

namespace rm21Core.Ribbons
{
   public class Shoulder: ribbonBase
   {
      public Shoulder(CogoStation beginStation, CogoStation endStation, double initialWidth, Slope initialSlope)
         : base(beginStation, endStation, initialWidth, initialSlope) { }

      public override string getHashName() { return "Shoulder"; }

   }
}
