using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

namespace ptsCogo.CorridorTypes
{
   public sealed class TypicalSection : rm21CorridorPrecursor
   {
      public TypicalSection()
      {
         this.allTypicalSectionPGLgroupings = new ObservableCollection<PGLGroupingTypicalSection>();
      }

      public TypicalSection(String tsName, params PGLGroupingTypicalSection[] pglGts)
      {
         this.TSname = tsName;
         this.allTypicalSectionPGLgroupings = new ObservableCollection<PGLGroupingTypicalSection>(pglGts);
      }
      
      public String TSname { get; set; }

   }
}
