﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using rm21Core;

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

      internal void OnIsSelected()
      {
         getRoadwayModelVM().SelectedRibbon = this;
      }
   }
}