using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ptsCogo
{
   [Serializable]
   public class ptsFeature
   {
      public string name { get; set; }
      public string color { get; set; }
      public int lineWeight { get; set; }
      public string lineStyle { get; set; }
      public float minThickness { get; set; }
   }
}
