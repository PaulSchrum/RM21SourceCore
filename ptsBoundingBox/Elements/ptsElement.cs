using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ptsCogo
{
   
   public abstract class ptsElement
   {
      public ptsPoint startPoint { get; set; }
      public ptsBoundingBox2d boundingBox {get; protected set;}
      public rm21Feature feature { get; set; }
            
   }
}
