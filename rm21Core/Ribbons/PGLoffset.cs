using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ptsCogo.coordinates.CurvilinearCoordinates;
using ptsCogo;
using ptsCogo.Angle;


namespace rm21Core.Ribbons
{
   class PGLoffset : ribbonBase
   {
      public PGLoffset(CogoStation beginStation, CogoStation endStation, double initialWidth, Slope initialSlope)
         : base(beginStation, endStation, initialWidth, initialSlope) { }

      public override string getHashName() { return "PGL Offset"; }

   }
}
