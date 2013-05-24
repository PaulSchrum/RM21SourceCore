using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ptsCogo;
using ptsCogo;

namespace MainRM21WPFapp.ViewModels
{
   public class RibbonAspectVMbase : NotifyPropertyChangedVMbase
   {
      public RibbonAspectVMbase(RibbonViewModel aRibbonVM)
      {
         myRibbonVM_ = aRibbonVM;
         myRibbon_ = myRibbonVM_.TheRibbon;
      }

      private RibbonViewModel myRibbonVM_;
      private ribbonBase myRibbon_;
      public ribbonBase Ribbon
      {
         get { return myRibbon_; }
         set
         {
            if (value != myRibbon_)
            {
               myRibbon_ = value;
               this.OnPropertyChanged("Ribbon");
            }
         }
      }

      private Profile myAspect_;
      public Profile Aspect
      {
         get { return myAspect_; }
         set
         {
            if (value != myAspect_)
            {
               myAspect_ = value;
               this.OnPropertyChanged("Aspect");
            }
         }
      }

   }
}
