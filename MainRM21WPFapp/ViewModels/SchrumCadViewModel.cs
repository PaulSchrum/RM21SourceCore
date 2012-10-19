using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Globalization;
using System.Windows.Controls;
using System.Windows;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Shapes;
using System.Windows.Media;

namespace MainRM21WPFapp.ViewModels
{
   public class SchrumCadViewModel : ObservableCollection<schrumCadElementViewModel>, INotifyPropertyChanged
   {

      new public event PropertyChangedEventHandler PropertyChanged;

      protected void RaisePropertyChanged(String str) { OnPropertyChanged(str); }

      protected virtual void OnPropertyChanged(string propertyName)
      {
         if (this.PropertyChanged != null)
            this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
      }
   }

   public class TransformedCanvas
   {
      public TransformedCanvas(Canvas aCanvas)
      {
         if (aCanvas == null)
            throw new NullReferenceException();

         Canvas = aCanvas;
         Canvas.LayoutTransform = new ScaleTransform(1.0, -1.0);
         translateX = Canvas.ActualWidth / 2.0;
         translateY = Canvas.ActualHeight / 2.0;

         inchesPerUnit = 12.0;
         Scale = 1.0;
         verticalExagg = 1.0;
      }

      public Canvas Canvas { get; set; }

      public void Add(Line aLine)
      {
         Line copyOfaLine = new Line();
         copyOfaLine.X1 = aLine.X1;
         copyOfaLine.Y1 = aLine.Y1;
         copyOfaLine.X2 = aLine.X2;
         copyOfaLine.Y2 = aLine.Y2;
         copyOfaLine.HorizontalAlignment = aLine.HorizontalAlignment;
         copyOfaLine.VerticalAlignment = aLine.VerticalAlignment;
         copyOfaLine.StrokeThickness = aLine.StrokeThickness;
         copyOfaLine.Stroke = aLine.Stroke;

         TransformWorldToCanvas(ref copyOfaLine);
         Canvas.Children.Add(copyOfaLine);

      }

      private double scale_;
      public double Scale 
      { get{return scale_;}
         set { scale_ = value; computeRealScale(); }
      }
      private double realScale_ { get; set; }

      private double inchesPerUnit_;
      public double inchesPerUnit { get { return inchesPerUnit_; } set { inchesPerUnit_ = value; computeRealScale(); } }

      public double verticalExagg { get; set; }

      public double translateX { get; set; }
      public double translateY { get; set; }

      private void computeRealScale()
      {
         if (scale_ == 0.0) return;
         realScale_ = 96.0 / inchesPerUnit / scale_;
      }

      private double TransformWorldToCanvasX(double X)
      {
         X += translateX;
         X *= realScale_;
         return X;
      }

      private double TransformWorldToCanvasY(double Y)
      {
         Y += translateY/verticalExagg;
         Y *= (realScale_ * verticalExagg);
         return Y;
      }

      public void TransformWorldToCanvas(ref Point inputPoint)
      {
         inputPoint.X = TransformWorldToCanvasX(inputPoint.X);
         inputPoint.Y = TransformWorldToCanvasY(inputPoint.Y);
      }

      public void TransformWorldToCanvas(ref Line inputLine)
      {
         inputLine.X1 = TransformWorldToCanvasX(inputLine.X1);
         inputLine.Y1 = TransformWorldToCanvasY(inputLine.Y1);
         inputLine.X2 = TransformWorldToCanvasX(inputLine.X2);
         inputLine.Y2 = TransformWorldToCanvasY(inputLine.Y2);
      }

      public Line TransformWorldToCanvas(Line inputLine)
      {
         Line returnLine = new Line();
         returnLine.X1 = TransformWorldToCanvasX(inputLine.X1);
         returnLine.Y1 = TransformWorldToCanvasY(inputLine.Y1);
         returnLine.X2 = TransformWorldToCanvasX(inputLine.X2);
         returnLine.Y2 = TransformWorldToCanvasY(inputLine.Y2);
         returnLine.HorizontalAlignment = inputLine.HorizontalAlignment;
         returnLine.VerticalAlignment = inputLine.VerticalAlignment;
         returnLine.StrokeThickness = inputLine.StrokeThickness;
         returnLine.Stroke = inputLine.Stroke;
         return returnLine;
      }

      public TextBlock TransformWorldToCanvas(TextBlock inputTextBlock)
      {
         TextBlock returnTextBlock = new TextBlock();
         returnTextBlock.FontSize = inputTextBlock.FontSize * realScale_;
         returnTextBlock.Text = inputTextBlock.Text;
         returnTextBlock.FontStyle = inputTextBlock.FontStyle;
         returnTextBlock.Foreground = inputTextBlock.Foreground;
         returnTextBlock.Background = inputTextBlock.Background;
         returnTextBlock.HorizontalAlignment = inputTextBlock.HorizontalAlignment;
         returnTextBlock.VerticalAlignment = inputTextBlock.VerticalAlignment;

         return returnTextBlock;

      }
   }


   /* */
   public class schrumCadElementViewModel
   {
      public Line aLine = new Line();
      public schrumCadElementViewModel(TransformedCanvas transformedCnvas)
      {
         TransformedCanvas = transformedCnvas;
         Stroke = Brushes.White;
         StrokeThickness = 1;

      }

      protected TransformedCanvas TransformedCanvas { get; set; }

      public Brush Stroke { get; set; }
      public double StrokeThickness { get; set; }

      public virtual void drawOnCanvas()
      {

      }
   }

   public class CadLine : schrumCadElementViewModel
   {
      public double X1 { get; set; }
      public double Y1 { get; set; }
      public double X2 { get; set; }
      public double Y2 { get; set; }

      public CadLine(TransformedCanvas transformedCanvas)
         : base(transformedCanvas)
      {

      }

      public Line asWPFLine()
      {
         Line retLine = new Line();

         retLine.Stroke = Stroke;
         retLine.StrokeThickness = StrokeThickness;
         retLine.HorizontalAlignment = HorizontalAlignment.Left;
         retLine.VerticalAlignment = VerticalAlignment.Bottom;
         retLine.X1 = X1;
         retLine.Y1 = Y1;
         retLine.X2 = X2;
         retLine.Y2 = Y2;
         return retLine;
      }

      public override void drawOnCanvas()
      {
         Line wpfLine = asWPFLine();
         base.drawOnCanvas();
         if (TransformedCanvas != null)
            TransformedCanvas.Add(wpfLine);
      }

   }  

}
