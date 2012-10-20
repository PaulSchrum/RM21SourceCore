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
               
               if (CanvasXfrmd == null)
                  CanvasXfrmd = 
                     new TransformedCanvas(parentVM_.parentVM_.myViewReference.xsCanvas);

               CanvasXfrmd.Scale = 10.0; CanvasXfrmd.verticalExagg = 5.0;

               CanvasXfrmd.Canvas.Children.Clear();

               CadLine testLine = new CadLine(CanvasXfrmd);
               testLine.X1 = 0; testLine.Y1 = 5;
               testLine.X2 = 105; testLine.Y2 = 12;
               testLine.drawOnCanvas();

               CadText CadText = new CadText(CanvasXfrmd);
               CadText.Text = "Text Item 3.";
               CadText.X1 = 100.0; CadText.Y1 = 1.0;
               CadText.RotationAngle = -33;
               CadText.drawOnCanvas();
            }
         }
      }
   }
}
