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
         
         //if (null != thePglGrouping_.

         if (null != thePglGrouping_.thePGLoffsetRibbon)
         {
            this.Children.Add(new RibbonViewModel(thePglGrouping_.thePGLoffsetRibbon));
         }
         
         //this.Children
         if (null != thePglGrouping_.outsideRibbons)
         {
            foreach (var ribbon in thePglGrouping_.outsideRibbons)
            {
               this.Children.Add(new RibbonViewModel(ribbon as ribbonBase));
            }
         }
      }

      private PGLGrouping thePglGrouping_;

      public new String HashName
      {
         get { return thePglGrouping_.getHashName(); }
      }
   }
}
