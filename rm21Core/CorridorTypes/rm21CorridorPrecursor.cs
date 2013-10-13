using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace ptsCogo.CorridorTypes
{
   public abstract class rm21CorridorPrecursor
   {
      public ObservableCollection<PGLGroupingTypicalSection> allTypicalSectionPGLgroupings { get; protected set; }
   }
}
