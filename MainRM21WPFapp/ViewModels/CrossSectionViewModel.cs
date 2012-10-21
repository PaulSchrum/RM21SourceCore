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

namespace MainRM21WPFapp.ViewModels
{
   public class CrossSectionViewModel : SchrumCadViewModel
   {
      public CrossSectionViewModel(RoadwayModel_TabVM parentVM) : base()
      {
         parentVM_ = parentVM;
         currentCorridor_ = parentVM_.CurrentCorridor;
      }

      public Canvas xsCanvas{get; set;}
      private TransformedCanvas CanvasXfrmd;

      private RoadwayModel_TabVM parentVM_;

      private rm21Corridor currentCorridor_;

      private double currentStation_;
      public double CurrentStation 
      {
         get { return currentStation_; }
         set
         {
            if (currentStation_ != value)
            {
               currentStation_ = value;
               RaisePropertyChanged("CurrentStation");
               /* */

               if (parentVM_.parentVM_.myViewReference == null)  return;
               if (parentVM_.CurrentCorridor == null) return;
               currentCorridor_ = parentVM_.CurrentCorridor;
               
               if (CanvasXfrmd == null)
                  CanvasXfrmd = 
                     new TransformedCanvas(parentVM_.parentVM_.myViewReference.xsCanvas);

               CanvasXfrmd.Scale = 10.0;
               CanvasXfrmd.verticalExagg = 1.0;

               CanvasXfrmd.Canvas.Children.Clear();
               currentCorridor_.DrawCrossSection(CanvasXfrmd, new CogoStation(currentStation_));

            }
         }
      }
   }
}
