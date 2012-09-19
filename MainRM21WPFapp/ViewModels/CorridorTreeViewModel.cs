using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using rm21Core;

namespace MainRM21WPFapp.ViewModels
{
   public class CorridorTreeViewModel
   {
      public CorridorTreeViewModel(rm21Core.rm21Corridor aCorridor)
      {
         allpglgVMs = new ObservableCollection<PglGroupingViewModel>(
            (from pglGr in aCorridor.allPGLgroupings
             select new PglGroupingViewModel(pglGr))
             .ToList<PglGroupingViewModel>()
            );
      }

      private ObservableCollection<PglGroupingViewModel> allpglgVMs;
      public ObservableCollection<PglGroupingViewModel> AllpglgVMS
      { get { return allpglgVMs; } }
   }
}
