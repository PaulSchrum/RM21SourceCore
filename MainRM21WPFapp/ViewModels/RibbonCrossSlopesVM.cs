using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ptsCogo;

namespace MainRM21WPFapp.ViewModels
{
   class RibbonCrossSlopesVM : RibbonAspectVMbase
   {

      public RibbonCrossSlopesVM(RibbonViewModel aRibbonVM)
         : base(aRibbonVM)
      {
         Aspect = Ribbon.CrossSlopes;
      }

   }
}
