using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using rm21Core.ExternalClasses;
using rm21Core;
using rm21Core.CorridorTypes;
using rm21Core.Ribbons;
using System.Windows.Input;
using ptsCogo;


namespace MainRM21WPFapp.ViewModels
{
   public class RoadwayModel_TabVM : ViewModelBase
   {
      public RoadwayModel_TabVM() { }
      public RoadwayModel_TabVM(MainWindowVM parent)
      { 
         parentVM_ = parent; 
         currentCorridor_ = parentVM_.CurrentCorridor; 
      }

      private MainWindowVM parentVM_;

      private rm21Corridor currentCorridor_;
      public rm21Corridor CurrentCorridor
      {
         get { return currentCorridor_; }
         set
         {
            if (currentCorridor_ != value)
            {
               currentCorridor_ = value;
               RaisePropertyChanged("CurrentCorridor");
            }
         }
      }

      
   }
}
