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

      private ObservableCollection<PglGroupingViewModel> allpglgVMs_;
      public ObservableCollection<PglGroupingViewModel> AllpglgVMS
      { 
         get { return allpglgVMs_; }
         set
         {
            if (value != allpglgVMs_)
            {
               allpglgVMs_ = value;
               this.OnPropertyChanged("AllpglgVMS");
            }
         }
      }

   }
}
