﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ptsCogo.Elements.Paths
{
   public abstract class ptsPath : ptsElement
   {
      public ptsPoint startPoint { get; set; }
      public ptsPoint endPoint { get; set; }
   }
}