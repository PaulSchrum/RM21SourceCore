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

         RoadwayModelTabVM = new RoadwayModel_TabVM(this);

         loadData();

         CurrentCorridor = theRM21model.allCorridors[0];
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
         pglGrLT.addOutsideRibbon(new FrontSlopeCutDitch((CogoStation)1000, (CogoStation)10000, 15.0,  -1.0 / 6.0));
         

         pglGrRT.addOutsideRibbon(new RoadwayLane((CogoStation)1000, (CogoStation)10000, 12.0, -0.02));
         pglGrRT.addOutsideRibbon(new Shoulder((CogoStation)1000, (CogoStation)10000, 10.0, -0.08));
         pglGrRT.addOutsideRibbon(new FrontSlopeCutDitch((CogoStation)1000, (CogoStation)10000, 15.0, -1.0 / 6.0));

         aCorridor.addPGLgrouping(pglGrLT);
         aCorridor.addPGLgrouping(pglGrRT);

      }

      private System.Windows.Window myViewReference_;
      public System.Windows.Window myViewReference
      {
         get { return myViewReference_; }
         set { myViewReference_ = value; } 
      }

      private RoadwayModel_TabVM roadwayModelTabVM_;
      public RoadwayModel_TabVM RoadwayModelTabVM
      {
         get { return roadwayModelTabVM_; }
         set
         {
            if (roadwayModelTabVM_ != value)
            {
               roadwayModelTabVM_ = value;
               RaisePropertyChanged("RoadwayModelTabVM");
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
               RoadwayModelTabVM.CurrentCorridor = currentCorridor_;
               //myViewReference.getv
               TestText3_9_26 = "Top level test, " + currentCorridor_.Name;
               RaisePropertyChanged("CurrentCorridor");
            }
         }
      }

      private String testText3_9_26_;
      public String TestText3_9_26
      {
         get { return testText3_9_26_; }
         set
         {
            if (testText3_9_26_ != value)
            {
               testText3_9_26_ = value;
               RaisePropertyChanged("TestText3_9_26");
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
