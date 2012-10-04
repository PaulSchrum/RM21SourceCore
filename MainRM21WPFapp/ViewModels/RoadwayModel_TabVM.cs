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
         CurrentCorridor = parentVM_.CurrentCorridor;
         this.testText9_26_ = "Not set.";
         theCorridorAsTreeViewModel = new CorridorTreeViewModel();
      }

      private MainWindowVM parentVM_;

      private CorridorTreeViewModel theCorridorAsTreeViewModel_;
      public CorridorTreeViewModel theCorridorAsTreeViewModel
      {
         get { return theCorridorAsTreeViewModel_; }
         set
         {
            if (theCorridorAsTreeViewModel_ != value)
            {
               theCorridorAsTreeViewModel_ = value;
               theCorridorAsTreeViewModel_.TheCorridor = CurrentCorridor;
               theCorridorAsTreeViewModel_.ownerRoadwayVM = this;
               RaisePropertyChanged("theCorridorAsTreeViewModel");
            }
         }
      }

      private String testText9_26_;
      public String TestText9_26
      {
         get { return testText9_26_; }
         set
         {
            if (testText9_26_ != value)
            {
               testText9_26_ = value;
               //theCorridorAsTreeViewModel = new CorridorTreeViewModel(currentCorridor_);
               RaisePropertyChanged("TestText9_26");
            }
         }
      }

      private rm21Corridor currentCorridor_;
      public rm21Corridor CurrentCorridor
      {
         get { return currentCorridor_; }
         set
         {
            if (currentCorridor_ != value)
            {
               currentCorridor_ = value;
               
               if (null == theCorridorAsTreeViewModel)
                  theCorridorAsTreeViewModel = new CorridorTreeViewModel();
               theCorridorAsTreeViewModel.TheCorridor = currentCorridor_;

               RaisePropertyChanged("CurrentCorridor");
               if (parentVM_ != null && parentVM_.CurrentCorridor != null && parentVM_.CurrentCorridor.Name != null)
               {
                  TestText9_26 = "Corridor is " + parentVM_.CurrentCorridor.Name;
               }
            }
         }
      }

      private RibbonViewModel selectedRibbon_;
      public RibbonViewModel SelectedRibbon
      {
         get { return selectedRibbon_; }
         set
         {
            if (selectedRibbon_ != value)
            {
               selectedRibbon_ = value;
               RaisePropertyChanged("SelectedRibbon");

               selectedRibbon_.Widths = selectedRibbon_.TheRibbon.Widths;
               selectedRibbon_.CrossSlopes = selectedRibbon_.TheRibbon.CrossSlopes;
            }
         }
      }

   }
}
