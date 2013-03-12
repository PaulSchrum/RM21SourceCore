using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ptsCogo.Horizontal;

namespace rm21Core
{
   public class RM21Project
   {
      public string Name { get; set; }
      public string Decription { get; set; }
      public string SymbologyToFeatureMappingFilename { get; set; }
      public string TypicalSectionLibrary { get; set; }

      public List<rm21Corridor> allCorridors { get; set; }
      public List<rm21HorizontalAlignment> unaffiliatedHAs { get; set; }
   }
}
