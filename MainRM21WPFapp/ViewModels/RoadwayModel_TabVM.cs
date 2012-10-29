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
         theCorridorAsTreeViewModel = new CorridorTreeViewModel();
         CrossSectionViewModel = new CrossSectionViewModel(this);
         CurrentStation = (CogoStation) 1975.0;
         PlanSchematicViewModel = new PlanSchematicViewModel(this);
      }

      internal MainWindowVM parentVM_;

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

      private CrossSectionViewModel xsVM_;
      public CrossSectionViewModel CrossSectionViewModel
      {
         get { return xsVM_; }
         set
         {
            if (xsVM_ != value)
            {
               xsVM_ = value;
               RaisePropertyChanged("CrossSectionViewModel");
            }
         }
      }

      private PlanSchematicViewModel psVM_;
      public PlanSchematicViewModel PlanSchematicViewModel
      {
         get { return psVM_; }
         set
         {
            if (psVM_ != value)
            {
               psVM_ = value;
               RaisePropertyChanged("PlanSchematicViewModel");
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

               if (selectedRibbon_ != null)
               {
                  selectedRibbon_.WidthsVM = new ProfileVPI_VM(selectedRibbon_.TheRibbon.Widths);
                  selectedRibbon_.CrossSlopesVM = new ProfileVPI_VM(selectedRibbon_.TheRibbon.CrossSlopes);
               }
            }
         }
      }

      private CogoStation currentStation_;
      public CogoStation CurrentStation
      {
         get { return currentStation_; }
         set
         {
            if (currentStation_ != value)
            {
               currentStation_ = value;
               RaisePropertyChanged("CurrentStation");
               if (null != CrossSectionViewModel)
                  CrossSectionViewModel.CurrentStation = currentStation_;
               /*
               if (null != PlanSchematicViewModel)
                  PlanSchematicViewModel.CurrentStation = currentStation_; */
            }
         }
      }

   }
}
