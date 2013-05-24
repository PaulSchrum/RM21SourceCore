using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ptsCogo.ExternalClasses;
using ptsCogo;
using ptsCogo.CorridorTypes;
using ptsCogo.Ribbons;
using System.Windows.Input;
using ptsCogo;
using System.Windows;

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

         CurrentCorridor = theRM21model.allCorridors[1];
      }

      private void setupCorridors()
      {
         rm21Corridor aCorridor = 
            theRM21model.allCorridors.FirstOrDefault
                  (aCorr => aCorr.Name.Equals("L"));

         aCorridor.Alignment.BeginStation = 1000.0;
         aCorridor.Alignment.EndStation = 10000.0;

         PGLGrouping pglGrLT = new PGLGrouping(-1);
         PGLGrouping pglGrRT = new PGLGrouping(1);

         PGLoffset pgloRT = new PGLoffset((CogoStation)1000.0, (CogoStation)10000, 0.0, 0.0);  /* PGL RT */
         pgloRT.addWidenedSegment((CogoStation)2555.0, (CogoStation)2955.0, 15.0,
            (CogoStation)8050.0, (CogoStation)8050.001);
         pglGrRT.thePGLoffsetRibbon = pgloRT;

         PGLoffset pgloLT = new PGLoffset((CogoStation)1000.0, (CogoStation)10000, 0.0, 0.0);  /* PGL LT */
         pgloLT.addWidenedSegment((CogoStation)2555.0, (CogoStation)2955.0, 15.0,
            (CogoStation)8050.0, (CogoStation)8050.001);
         pglGrLT.thePGLoffsetRibbon = pgloLT;

         /* Back Thru Lane, Inner */
         RoadwayLane rdyLane = new RoadwayLane((CogoStation)1000, (CogoStation)10000, 0.0, -0.02);
         rdyLane.addWidenedSegment((CogoStation)2235, (CogoStation)2555, 12.0,
            (CogoStation)8050, (CogoStation)8050.001);
         rdyLane.addCrossSlopeChangedSegment((CogoStation)2200, (CogoStation)2300, 0.08,
            (CogoStation)2500, (CogoStation)2600);
         pglGrLT.addOutsideRibbon(rdyLane);
         /* Back Thru Lane, Outer */
         rdyLane = new RoadwayLane((CogoStation)1000, (CogoStation)10000, 12.0, -0.02);
         rdyLane.addCrossSlopeChangedSegment((CogoStation)2200, (CogoStation)2300, 0.08,
            (CogoStation)2500, (CogoStation)2600);
         pglGrLT.addOutsideRibbon(rdyLane);

         Shoulder aShldr = new Shoulder((CogoStation)1000, (CogoStation)10000, 10.0, -0.08);
         aShldr.addWidenedSegment((CogoStation)2000.0, (CogoStation)2040.0, 17.0,
            (CogoStation)2250.0, (CogoStation)2290.00);
         aShldr.addCrossSlopeChangedSegment((CogoStation)2200, (CogoStation)2300, 0.02,
            (CogoStation)2500, (CogoStation)2600);

         pglGrLT.addOutsideRibbon(aShldr);

         /* Outside Cut Ditch, LT */
         pglGrLT.addOutsideRibbon(new FrontSlopeCutDitch((CogoStation)1000, (CogoStation)10000, 15.0,  -1.0 / 4.0));

         /* Outside Cut Ditch Backslope, LT */
         pglGrLT.addOutsideRibbon(new BackSlopeCutDitch((CogoStation)1000, (CogoStation)10000, 1.0 / 2.0));

         /* Median Shoulder LT */
         aShldr = new Shoulder((CogoStation)1000, (CogoStation)10000, 0.0, -0.04);
         aShldr.addWidenedSegment((CogoStation)2555.0, (CogoStation)2715.0, 6.0,
            (CogoStation)8050.0, (CogoStation)8050.001);
         pglGrLT.addInsideRibbon(aShldr);

         /* Median Ditch Slope LT */
         FrontSlopeCutDitch ditchFS = new FrontSlopeCutDitch((CogoStation) 1000, (CogoStation) 10000, 0.0,  -1.0 / 4.0);
         ditchFS.addWidenedSegment((CogoStation) 2715.0, (CogoStation) 2955.0, 9.0,
            (CogoStation)8050.0, (CogoStation)8050.001);
         pglGrLT.addInsideRibbon(ditchFS);


         /* Ahead Thru Lane, Inner */
         rdyLane = new RoadwayLane((CogoStation)1000, (CogoStation)10000, 0.0, -0.02);
         rdyLane.addWidenedSegment((CogoStation)2235, (CogoStation)2555, 12.0,
            (CogoStation)8050, (CogoStation)8050.001);
         rdyLane.addCrossSlopeChangedSegment((CogoStation)2240, (CogoStation)2300, -0.08,
            (CogoStation)2500, (CogoStation)2560);
         pglGrRT.addOutsideRibbon(rdyLane);

         /* Ahead Thru Lane, Outer */
         rdyLane = new RoadwayLane((CogoStation)1000, (CogoStation)10000, 12.0, -0.02);
         rdyLane.addCrossSlopeChangedSegment((CogoStation)2240, (CogoStation)2300, -0.08,
            (CogoStation)2500, (CogoStation)2560);
         pglGrRT.addOutsideRibbon(rdyLane);
         pglGrRT.addOutsideRibbon(new Shoulder((CogoStation)1000, (CogoStation)10000, 10.0, -0.08));
         pglGrRT.addOutsideRibbon(new FrontSlopeCutDitch((CogoStation)1000, (CogoStation)10000, 15.0, -1.0 / 4.0));

         /* Outside Cut Ditch Backslope, LT */
         var ditchBackSlope = new BackSlopeCutDitch((CogoStation)1000, (CogoStation)10000, 1.0 / 2.0);
         ditchBackSlope.addCrossSlopeChangedSegment((CogoStation)3100, (CogoStation)3200,
            1.0 / 4.0, (CogoStation)3300, (CogoStation)3400);
         pglGrRT.addOutsideRibbon(ditchBackSlope);

         /* Median Shoulder RT */
         aShldr = new Shoulder((CogoStation)1000, (CogoStation)10000, 0.0, -0.04);  
         aShldr.addWidenedSegment((CogoStation)2555.0, (CogoStation)2715.0, 6.0,
            (CogoStation)8050.0, (CogoStation)8050.001);
         pglGrRT.addInsideRibbon(aShldr);

         /* Median Ditch Slope RT */
         ditchFS = new FrontSlopeCutDitch((CogoStation)1000, (CogoStation)10000, 0.0, -1.0 / 4.0);
         ditchFS.addWidenedSegment((CogoStation)2715.0, (CogoStation)2955.0, 9.0,
            (CogoStation)8050.0, (CogoStation)8050.001);
         pglGrRT.addInsideRibbon(ditchFS);


         aCorridor.addPGLgrouping(pglGrLT);
         aCorridor.addPGLgrouping(pglGrRT);

         aCorridor.existingGroundSurface = new ptsCogo.Mocks.rm21MockSurface();
         vpiList vpiList = new vpiList();
         vpiList.add(-200.0, 3.0); vpiList.add(-150.0, -3.0); vpiList.add(-130.0, 6.0);
         vpiList.add(-40.0, 3.5); vpiList.add(38.0, 6.2); vpiList.add(65.0, 2.8);
         vpiList.add(68.0, 7.2); vpiList.add(100.0, 5.5); vpiList.add(200.0, 2.8);
         aCorridor.existingGroundSurface.setSectionProfileForMocking(new Profile(vpiList));
      }

      private MainWindow myViewReference_;
      public MainWindow myViewReference
      {
         get { return myViewReference_; }
         set 
         { 
            myViewReference_ = value;
            //RoadwayModelTabVM.CrossSectionViewModel.initializeDrawing();
         } 
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

      public void updateCanvases()
      {
         if (null != RoadwayModelTabVM)
         {
            RoadwayModelTabVM.CrossSectionViewModel.initializeDrawing();
            RoadwayModelTabVM.PlanSchematicViewModel.initializeDrawing();
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
         setupCorridors();
      }

   }

}
