using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using rm21Core;
using ptsCogo;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Input;

namespace MainRM21WPFapp.ViewModels
{
   public class CrossSectionViewModel : SchrumCadViewModel
   {
      public CrossSectionViewModel(RoadwayModel_TabVM parentVM) : base()
      {
         parentVM_ = parentVM;
         if (parentVM_ == null) return;

         currentCorridor_ = parentVM_.CurrentCorridor;
         ViewScaleFeetPerInch = 10.0;
         currentCorridor_ = parentVM_.CurrentCorridor;
         AdvanceDistance = 50.0;

         AdvanceStationAheadCmd = new RelayCommand(advanceStationAhead, () => canAdvanceAhead);
         canAdvanceAhead = true;

         AdvanceStationBackCmd = new RelayCommand(advanceStationBack, () => canAdvanceBack);
         canAdvanceBack = true;

      }

      public Canvas xsCanvas{get; set;}
      private TransformedCanvas CanvasXfrmd;

      private RoadwayModel_TabVM parentVM_;

      private rm21Corridor currentCorridor_;

      private double viewScaleFeetPerInch_;
      public double ViewScaleFeetPerInch
      {
         get { return viewScaleFeetPerInch_; }
         set
         {
            if (viewScaleFeetPerInch_ != value)
            {
               viewScaleFeetPerInch_ = value;
               RaisePropertyChanged("ViewScaleFeetPerInch");

               updateTransformedCanvas();
            }
         }
      }


      private double advanceDistance_;
      public double AdvanceDistance
      {
         get { return advanceDistance_; }
         set
         {
            if (advanceDistance_ != value)
            {
               advanceDistance_  = value;
               RaisePropertyChanged("AdvanceDistance");
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

               updateTransformedCanvas();
            }
         }
      }

      private double windowCenterX_;
      public double WindowCenterX
      {
         get { return windowCenterX_; }
         set
         {
            if (windowCenterX_ != value)
            {
               windowCenterX_ = value;
               RaisePropertyChanged("WindowCenterX");

               updateTransformedCanvas();
            }
         }
      }

      private double windowCenterY_;
      public double WindowCenterY
      {
         get { return windowCenterY_; }
         set
         {
            if (windowCenterY_ != value)
            {
               windowCenterY_ = value;
               RaisePropertyChanged("WindowCenterY");

               updateTransformedCanvas();
            }
         }
      }


      private bool canAdvanceAhead;
      public ICommand AdvanceStationAheadCmd { get; private set; }
      private void advanceStationAhead()
      {
         parentVM_.CurrentStation += AdvanceDistance;
      }

      private bool canAdvanceBack;
      public ICommand AdvanceStationBackCmd { get; private set; }
      private void advanceStationBack()
      {
         parentVM_.CurrentStation -= AdvanceDistance;
      }

      private void updateTransformedCanvas()
      {
         if (parentVM_ == null) return;
         if (parentVM_.parentVM_ == null) return;
         if (parentVM_.parentVM_.myViewReference == null) return;
         if (parentVM_.parentVM_.myViewReference.xsCanvas == null) return;
         currentCorridor_ = parentVM_.CurrentCorridor;

         if (CanvasXfrmd == null)
            CanvasXfrmd = 
               new TransformedCanvas(parentVM_.parentVM_.myViewReference.xsCanvas);

         CanvasXfrmd.Scale = ViewScaleFeetPerInch;
         CanvasXfrmd.verticalExagg = 1.0;
         CanvasXfrmd.WindowCenterX = WindowCenterX;
         CanvasXfrmd.WindowCenterY = WindowCenterY;

         CanvasXfrmd.Canvas.Children.Clear();
         currentCorridor_.DrawCrossSection(CanvasXfrmd, new CogoStation(currentStation_));
      }
   }
}
