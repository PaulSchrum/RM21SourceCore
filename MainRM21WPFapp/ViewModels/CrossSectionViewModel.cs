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
               var aLine = new System.Windows.Shapes.Line() 
               {
                  X1 = 0, Y1 =  0, X2 = 100, Y2 = 10,
                  HorizontalAlignment = HorizontalAlignment.Left,
                  VerticalAlignment = VerticalAlignment.Bottom,
                  StrokeThickness = 3,
                  Stroke = Brushes.Bisque
               }; /* */

               CanvasXfrmd.Canvas.Children.Clear();
               CanvasXfrmd.Add(aLine);

               var aLine2 = new System.Windows.Shapes.Line()
               {
                  X1 = aLine.X2,
                  Y1 = aLine.Y2,
                  X2 = aLine.X2+200,
                  Y2 = aLine.Y2-4,
                  HorizontalAlignment = HorizontalAlignment.Left,
                  VerticalAlignment = VerticalAlignment.Center,
                  StrokeThickness = 1,
                  Stroke = Brushes.YellowGreen
               };
               //CanvasXfrmd.Add(aLine2);

               CadLine testLine = new CadLine(CanvasXfrmd);
               testLine.X1 = 0; testLine.Y1 = 5;
               testLine.X2 = 105; testLine.Y2 = 12;
               testLine.drawOnCanvas();

               TextBox textbox = new TextBox();
               textbox.Text = "Hi There.";
               textbox.LayoutTransform = new ScaleTransform(1.0, -1.0, 0, 0);
               textbox.Background = Brushes.Transparent;
               textbox.Foreground = Brushes.White;
               textbox.BorderThickness = new Thickness(0, 0, 0, 0);
               //textbox.Name = "tb1";
               textbox.HorizontalAlignment = HorizontalAlignment.Left;
               textbox.VerticalAlignment = VerticalAlignment.Bottom;
               textbox.VerticalContentAlignment = VerticalAlignment.Bottom;
               textbox.FontSize = 12.0;

               /* */
               CanvasXfrmd.Canvas.Children.Add(textbox);
               textbox.SetValue(Canvas.TopProperty, 66.4);
               textbox.SetValue(Canvas.LeftProperty, 176.96);
               /* */
            }
         }
      }
   }
}
