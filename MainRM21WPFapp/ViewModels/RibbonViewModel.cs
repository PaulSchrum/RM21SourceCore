using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using rm21Core;
using ptsCogo;

namespace MainRM21WPFapp.ViewModels
{
   public class RibbonViewModel : TreeViewItemViewModel
   {
      public RibbonViewModel(TreeViewItemViewModel parent) : base(parent)
      { }

      public RibbonViewModel(ribbonBase newRibbon)
         : base(null)
      {
         theRibbon_ = newRibbon;
      }

      private ribbonBase theRibbon_;
      internal ribbonBase TheRibbon
      { get { return theRibbon_; } }

      public new String HashName
      {
         get { return theRibbon_.getHashName(); }
      }

      private Profile widths_;
      public Profile Widths
      {
         get { return widths_; }
         set
         {
            if (widths_ != value)
            {
               widths_ = value;

               RaisePropertyChanged("Widths");
            }
         }
      }

      private Profile crossSlopes_;
      public Profile CrossSlopes
      {
         get { return crossSlopes_; }
         set
         {
            if (crossSlopes_ != value)
            {
               crossSlopes_ = value;

               RaisePropertyChanged("CrossSlopes");
            }
         }
      }


   }
}
