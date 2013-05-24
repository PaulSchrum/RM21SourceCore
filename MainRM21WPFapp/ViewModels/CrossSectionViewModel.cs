using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using ptsCogo;
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

         isPortWindowMoving = false;
         startMovingPoint.X = startMovingPoint.Y = 0.0;

         currentCorridor_ = parentVM_.CurrentCorridor;
         ViewScaleFeetPerInch = 10.0;
         currentCorridor_ = parentVM_.CurrentCorridor;
         AdvanceDistance = 4.25;

         AdvanceStationAheadCmd = new RelayCommand(advanceStationAhead, () => canAdvanceAhead);
         canAdvanceAhead = true;

         AdvanceStationBackCmd = new RelayCommand(advanceStationBack, () => canAdvanceBack);
         canAdvanceBack = true;

      }

      public Canvas xsCanvas{get; set;}
      public TransformedCanvas CanvasXfrmd;

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

               if (isPortWindowMoving == false)
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

      /* Handle Mouse moving of the canvaw view center */
      private bool isPortWindowMoving;
      private Point startMovingPoint;

      public void MouseMove(object sender, MouseEventArgs e)
      {
         if (isPortWindowMoving == false) return;
         if (null == CanvasXfrmd) return;
         if (!(sender is Canvas)) return;
         if (sender != CanvasXfrmd.Canvas) return;

         if (e.LeftButton == MouseButtonState.Pressed)
         {
            Point newMousePoint = e.GetPosition(CanvasXfrmd.Canvas);
            WindowCenterX = (startMovingPoint.X - newMousePoint.X) / ViewScaleFeetPerInch;
            WindowCenterY = (startMovingPoint.Y - newMousePoint.Y) / ViewScaleFeetPerInch;
            //System.Diagnostics.Debug.Print("XS WindowCenter X: {0}   Y: {1}", WindowCenterX, WindowCenterY);
         }
      }

      public void MouseLeftButtonDown(object sender, MouseEventArgs e)
      {
         if (null == CanvasXfrmd) return;
         if (!(sender is Canvas)) return;
         if (sender != CanvasXfrmd.Canvas) return;

         isPortWindowMoving = true;
         startMovingPoint = e.GetPosition(CanvasXfrmd.Canvas);
      }

      public void MouseLeftButtonUp(object sender, MouseEventArgs e)
      {
         if (null == CanvasXfrmd) return;
         if (!(sender is Canvas)) return;
         if (sender != CanvasXfrmd.Canvas) return;

         isPortWindowMoving = false;
         startMovingPoint.X = startMovingPoint.Y = 0.0; 
      }
      /* end Handle Mouse moving of the canvaw view center */

      /* Handle mouse scroll wheel as a way to zoom the canvas */
      public void MouseWheel(object sender, MouseEventArgs e)
      {
         if (null == CanvasXfrmd) return;
         if (!(sender is Canvas)) return;
         if (sender != CanvasXfrmd.Canvas) return;

         System.Windows.Input.MouseWheelEventArgs wheel = e as System.Windows.Input.MouseWheelEventArgs;
         if (wheel.Delta > 0)
            ViewScaleFeetPerInch /= Math.Sqrt(2.0);
         else if (wheel.Delta < 0)
            ViewScaleFeetPerInch *= Math.Sqrt(2.0);

      }
      /* end Handle mouse scroll wheel as a way to zoom the canvas */
      
      /* Handle mouse scroll over station text */
      public void StationTextMouseWheel(object sender, MouseEventArgs e)
      {
         if (null == CanvasXfrmd) return;

         System.Windows.Input.MouseWheelEventArgs wheel = e as System.Windows.Input.MouseWheelEventArgs;
         if (wheel.Delta > 0 && canAdvanceAhead == true)
            advanceStationAhead();
         else if (wheel.Delta < 0 && canAdvanceBack == true)
            advanceStationBack();

      }
      /* end Handle mouse scroll over station text */

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

      internal void initializeDrawing()
      {
         updateTransformedCanvas();
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
