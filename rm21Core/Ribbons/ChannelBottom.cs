using ptsCogo;
using ptsCogo.Angle;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace rm21Core.Ribbons
{
   class ChannelBottom : ribbonBase
   {
      public ChannelBottom(CogoStation beginStation, CogoStation endStation, double initialWidth, Slope initialSlope)
         : base(beginStation, endStation, initialWidth, initialSlope) { }

      public ChannelBottom(Double width, Slope slope) : base(width, slope) { }

      public override string getHashName() { return "Channel Bottom"; }
   
   }
}
