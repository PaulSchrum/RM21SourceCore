using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ptsCogo.CorridorTypes
{
   public class rm21RoadwayCorridor : rm21Corridor
   {
      public rm21RoadwayCorridor(string name) : base(name) { }

      public rm21RoadwayCorridor(string name_, TypicalSection TypicalSection)
         : base(name_, TypicalSection) { }

   }
}
