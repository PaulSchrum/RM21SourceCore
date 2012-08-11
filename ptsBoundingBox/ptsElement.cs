using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ptsCogo
{
   [Serializable]
   public abstract class ptsElement
   {
      //internal ptsBoundingBox2d boundingBox_i;
      public ptsFeature feature { get; set; }
   }
}
