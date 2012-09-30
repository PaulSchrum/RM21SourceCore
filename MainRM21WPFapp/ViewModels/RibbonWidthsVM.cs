using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ptsCogo;

namespace MainRM21WPFapp.ViewModels
{
   public class RibbonWidthsVM : RibbonAspectVMbase
   {

      public RibbonWidthsVM(RibbonViewModel aRibbonVM)
         : base(aRibbonVM)
      {
         Aspect = Ribbon.Widths;
      }

      
   }
}
