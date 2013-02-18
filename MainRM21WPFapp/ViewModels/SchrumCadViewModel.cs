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
using ptsCogo;

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

      protected ptsCogo.coordinates.CurvilinearCoordinates.StationOffsetElevation mouseSOE_=new ptsCogo.coordinates.CurvilinearCoordinates.StationOffsetElevation();
      protected ptsCogo.coordinates.CurvilinearCoordinates.StationOffsetElevation mouseSOE
      {
         get { return mouseSOE_; }
         set {mouseSOE_ = value;}
      }

      protected Point mouseXYworld { get; set; }
   }

   public class TransformedCanvas: IRM21cad2dDrawingContext
   {
      public TransformedCanvas(Canvas aCanvas)
      {
         if (aCanvas == null)
            throw new NullReferenceException();

         Canvas = aCanvas;
         Canvas.LayoutTransform = new ScaleTransform(1.0, -1.0);
         translateX = Canvas.ActualWidth / 2.0;
         translateY = Canvas.ActualHeight / 2.0;
         WindowCenterX = 0.0;
         WindowCenterY = 0.0;

         inchesPerUnit = 12.0;
         Scale = 1.0;
         verticalExagg = 1.0;

         StrokeThickness_ = 1.0;
         Stroke_ = Brushes.White;
         strokeDashArray_ = new DoubleCollection();

         aheadOrientation_ = 0.0;
      }

      public Canvas Canvas { get; set; }

      private double aheadOrientation_;
      public double aheadOrientation { get{return aheadOrientation_;} set{aheadOrientation_ = value;} }
      public double getAheadOrientationAngle() { return aheadOrientation; }

      public void setElementLevel(string LevelName)
      { }

      public void setElementWeight(double weight)
      {
         StrokeThickness_ = weight;
      }
      protected double StrokeThickness_;

      public double WindowCenterX { get; set; }
      public double WindowCenterY { get; set; }

      public void setElementColor(Color color)
      { Stroke_ = new SolidColorBrush(color); }
      protected SolidColorBrush Stroke_;


      public void resetDashArray()
      {
         strokeDashArray_ = new DoubleCollection();
      }
      public void addToDashArray(double dashLength)
      { strokeDashArray_.Add(dashLength);}
      protected DoubleCollection strokeDashArray_ { get; set; }

      public void Draw(double X1, double Y1, double X2, double Y2)
      {
         Line aLine = new Line();
         aLine.X1 = X1;
         aLine.Y1 = Y1;
         aLine.X2 = X2;
         aLine.Y2 = Y2;
         aLine.HorizontalAlignment = HorizontalAlignment.Left;
         aLine.VerticalAlignment = VerticalAlignment.Bottom;
         aLine.StrokeThickness = StrokeThickness_;
         aLine.Stroke = Stroke_;
         aLine.StrokeDashArray = strokeDashArray_;

         TransformWorldToCanvas(ref aLine);
         Canvas.Children.Add(aLine);
      }

      public void Draw(string textContent, double x1, double y1, double rotationAngle)
      {
         ScaleTransform flipVerticalXform = new ScaleTransform(1.0, -1.0, 0, 0);
         TransformGroup xFormGroup = new TransformGroup();
         xFormGroup.Children.Add(flipVerticalXform);
         xFormGroup.Children.Add(new RotateTransform(rotationAngle));
         TextBox textBox = new TextBox();
         textBox.Text = textContent;
         textBox.LayoutTransform = xFormGroup;
         textBox.Background = Brushes.Transparent;
         textBox.Foreground = Stroke_;
         textBox.BorderThickness = new Thickness(0.0);

         Canvas.Children.Add(textBox);
         positionTextBoxByJustification(textBox, x1, y1, rotationAngle);
      }

      private void positionTextBoxByJustification(TextBox textBox, double X1, double Y1, double rotAngle)
      {
         // for now, the only justification is Center Top
         double tWidth = textBox.ActualWidth + textBox.Margin.Left + textBox.Margin.Right;
         tWidth /= 2.0;  // Center Justification
         double tHeight = textBox.ActualHeight + textBox.Margin.Bottom + textBox.Margin.Top; // Bottom Justification

         //double anglWidthAdjustment = -1.0 * Math.Cos(rotAngle * 180.0 / Math.PI);
         //double anglHeightAdjustment = 

         //double textScale = 1.0;
         double textHeight = 96.0 * textBox.FontSize / 72.0;
         double textWidth = 0.4 * textBox.Text.Length * textHeight;

         double adjustX1 = TransformWorldToCanvasX(X1);
         double adjustY1 = TransformWorldToCanvasY(Y1);
         double cos = Math.Cos(rotAngle * Math.PI / 180.0);
         double sin = Math.Sin(-1 * rotAngle * Math.PI / 180.0);
         double addAdjustX1 = textWidth * Math.Cos(rotAngle * Math.PI / 180.0) / -2.0;
         double addAdjustY1 = textHeight * Math.Sin(-1 * rotAngle * Math.PI / 180.0) / 2.0;
         
         textBox.SetValue(Canvas.LeftProperty, adjustX1 + addAdjustX1);
         textBox.SetValue(Canvas.TopProperty, adjustY1 + addAdjustY1);
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

      public double translateX { get { return Canvas.ActualWidth / 2.0; } set { } }
      public double translateY { get { return Canvas.ActualHeight / 2.0; } set { } }

      private void computeRealScale()
      {
         if (scale_ == 0.0) return;
         realScale_ = 96.0 / scale_;
      }

      private double TransformWorldToCanvasX(double X)
      {
         X -= WindowCenterX;
         X *= realScale_;
         X += translateX;
         return X;
      }

      private double TransformWorldToCanvasY(double Y)
      {
         Y -= WindowCenterY;
         Y *= (realScale_ * verticalExagg);
         Y += translateY;
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

      public double TransformCanvasToWorldX(double X)
      {
         X -= translateX;
         X /= realScale_;
         X += WindowCenterX;
         return X;
      }

      public double TransformCanvasToWorldY(double Y)
      {
         Y -= translateY;
         Y /= (realScale_ * verticalExagg);
         Y += WindowCenterY;
         return Y;
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

      public double X1 { get; set; }
      public double Y1 { get; set; }

      public virtual void drawOnCanvas()
      {

      }
   }

   public class CadLine : schrumCadElementViewModel
   {
      public CadLine(TransformedCanvas transformedCanvas)
         : base(transformedCanvas)
      {      }

      public double X2 { get; set; }
      public double Y2 { get; set; }

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

   }

   public class CadText : schrumCadElementViewModel
   {
      public CadText(TransformedCanvas transformedCanvas)
         : base(transformedCanvas)
      {
         TextSize = 12.0;
         RotationAngle = 0.0;
      }

      public String Text { get; set; }
      public double TextSize { get; set; }
      public double RotationAngle { get; set; }

   }
}
