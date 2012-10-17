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
               
               schrumCanvasTransform CanvasXfrmd = 
                  new schrumCanvasTransform(parentVM_.parentVM_.myViewReference.xsCanvas);

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
               CanvasXfrmd.Add(aLine2);

               //String txt = "Text Text";
               //RichTextBox rtb = new RichTextBox();
               //rtb.DataContext = null;
               //rtb.AppendText("Test Text");
               //rtb.FontSize = 0.5;
               //rtb.MinWidth = 50; rtb.MinHeight = 25;
               //rtb.HorizontalAlignment = HorizontalAlignment.Left;
               //rtb.VerticalAlignment = VerticalAlignment.Bottom;
               //rtb.LayoutTransform = new TranslateTransform(100, 50);
               //rtb.Foreground = Brushes.Tomato;
               //rtb.Background = Brushes.Black;
               //rtb.BorderBrush = Brushes.Gold;
               //rtb.RenderTransformOrigin = new Point(77, 22);
               //rtb.IsReadOnly = true;
               //xsCanvas.Children.Add(rtb);

               //y2_ += 20;
            }
         }
      }
   }
}
