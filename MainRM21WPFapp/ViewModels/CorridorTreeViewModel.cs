using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using ptsCogo;

namespace MainRM21WPFapp.ViewModels
{
   public class CorridorTreeViewModel : TreeViewItemViewModel
   {
      
      public CorridorTreeViewModel() : base(null)
      {
         Level1Items = new ObservableCollection<PglGroupingViewModel>();
      }

      private ptsCogo.rm21Corridor theCorridor_;
      public rm21Corridor TheCorridor
      {
         get { return theCorridor_; }
         set
         {
            if (value != theCorridor_)
            {
               theCorridor_ = value;

               Level1Items.Clear();
               if (null == theCorridor_.allPGLgroupings)
                  TestString = "There is no data.";
               else
               {
                  TestString = "Data is available.";
                  foreach (var pglGrouping in theCorridor_.allPGLgroupings)
                  {
                     PglGroupingViewModel pgVM = new PglGroupingViewModel(pglGrouping);
                     pgVM.Parent = this;
                     Level1Items.Add(pgVM);
                  }
               }

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

      public RoadwayModel_TabVM ownerRoadwayVM { get; set; }

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
