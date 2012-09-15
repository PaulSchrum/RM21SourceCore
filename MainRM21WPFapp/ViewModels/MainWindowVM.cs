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
   public class MainWindowVM : ViewModelBase
   {
      public MainWindowVM()
      {
         theRM21model = new rm21Model();
         theRM21model.allCorridors.Add(new rm21Corridor("C1"));
         theRM21model.allCorridors.Add(new rm21RoadwayCorridor("L"));
         theRM21model.allCorridors.Add(new rm21RoadwayCorridor("Y1"));

         LoadDataCmd = new RelayCommand(loadData, () => canLoadData);
         canLoadData = true;

         roadwayModelTabVM_ = new RoadwayModel_TabVM(this);

         loadData();
      }
      


      private void setupCorridorL()
      {
         rm21Corridor aCorridor = 
            theRM21model.allCorridors.FirstOrDefault
                  (aCorr => aCorr.Name.Equals("L"));

         PGLGrouping pglGrLT = new PGLGrouping(-1);
         PGLGrouping pglGrRT = new PGLGrouping(1);

         pglGrLT.addOutsideRibbon(new RoadwayLane((CogoStation)1000, (CogoStation)10000, 12.0, -0.02));
         pglGrLT.addOutsideRibbon(new Shoulder((CogoStation)1000, (CogoStation)10000, 10.0, -0.08));
         pglGrLT.addOutsideRibbon(new FrontSlopeCutDitch((CogoStation)1000, (CogoStation)10000, 15.0, -1 / 6));

         pglGrRT.addOutsideRibbon(new RoadwayLane((CogoStation)1000, (CogoStation)10000, 12.0, -0.02));
         pglGrRT.addOutsideRibbon(new Shoulder((CogoStation)1000, (CogoStation)10000, 10.0, -0.08));
         pglGrRT.addOutsideRibbon(new FrontSlopeCutDitch((CogoStation)1000, (CogoStation)10000, 15.0, -1 / 6));

         aCorridor.addPGLgrouping(pglGrLT);
         aCorridor.addPGLgrouping(pglGrRT);

      }

      private RoadwayModel_TabVM roadwayModelTabVM_;
      public RoadwayModel_TabVM RoadwayModelTabMV
      {
         get { return roadwayModelTabVM_; }
         set
         {
            if (roadwayModelTabVM_ != value)
            {
               roadwayModelTabVM_ = value;
               RaisePropertyChanged("RoadwayModelTabMV");
            }
         }
      }

      private rm21Model theRM21model_;
      public rm21Model theRM21model
      {
         get { return theRM21model_; }
         set
         {
            if (theRM21model_ != value)
            {
               theRM21model_ = value;
               RaisePropertyChanged("theRM21model");
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
               RaisePropertyChanged("CurrentCorridor");
            }
         }
      }

      private bool canLoadData;
      public ICommand LoadDataCmd { get; private set; }
      private void loadData()
      {
         setupCorridorL();
      }

   }

}
