using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using rm21Core;

namespace MainRM21WPFapp.ViewModels
{
   public class CorridorTreeViewModel : TreeViewItemViewModel
   {
      
      public CorridorTreeViewModel(rm21Core.rm21Corridor aCorridor) : base(null)
      {
         theCorridor_ = aCorridor;
         if (null != aCorridor.allPGLgroupings)
         {
            allpglgVMs_ = new ObservableCollection<PglGroupingViewModel>(
               (from pglGr in aCorridor.allPGLgroupings
                select new PglGroupingViewModel(pglGr))
                .ToList<PglGroupingViewModel>()
               );
            TestString = "There is data available.";
         }
         else
            TestString = "There is no data.";
      }

      private rm21Core.rm21Corridor theCorridor_;
      public rm21Corridor TheCorridor
      {
         get { return theCorridor_; }
         set
         {
            if (value != theCorridor_)
            {
               theCorridor_ = value;
               this.OnPropertyChanged("TheCorridor");
            }
         }
      }


      private String testString_;
      public String TestString
      {
         get { return testString_; }
         set
         {
            if (value != testString_)
            {
               testString_ = value;
               this.OnPropertyChanged("TestString");
            }
         }
      }

      private ObservableCollection<PglGroupingViewModel> level_1_items_;
      public ObservableCollection<PglGroupingViewModel> Level1Items
      {
         get { return level_1_items_; }
         set
         {
            if (value != level_1_items_)
            {
               level_1_items_ = value;
               this.OnPropertyChanged("Level1Items");
            }
         }
      }

      private ObservableCollection<PglGroupingViewModel> allpglgVMs_;
      private ObservableCollection<PglGroupingViewModel> AllpglgVMS
      { 
         get { return allpglgVMs_; }
         set
         {
            if (value != allpglgVMs_)
            {
               allpglgVMs_ = value;
               Level1Items = allpglgVMs_;
               //this.OnPropertyChanged("AllpglgVMS");
            }
         }
      }

   }
}
