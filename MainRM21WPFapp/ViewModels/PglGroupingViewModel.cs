using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using rm21Core;

namespace MainRM21WPFapp.ViewModels
{
   public class PglGroupingViewModel : TreeViewItemViewModel
   {
      public PglGroupingViewModel(TreeViewItemViewModel parent) : base(parent)
      { }

      public PglGroupingViewModel(PGLGrouping theNewPglGrouping)
         : base(null)
      {
         thePglGrouping_ = theNewPglGrouping;
      }

      private PGLGrouping thePglGrouping_;
   }
}
