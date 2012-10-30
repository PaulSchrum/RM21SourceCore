﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using ptsCogo.coordinates.CurvilinearCoordinates;
using ptsCogo;
using ptsCogo.Angle;
using System.ComponentModel;
using System.Windows.Media;



namespace rm21Core
{
   public abstract class ribbonBase : IRibbonLike, INotifyPropertyChanged
   {
      private int myIndex_ { get; set; }

      //private Profile width_;
      public Profile Widths { get; private set; }
      
      internal Profile interpretWidths { get; set; }
      public Profile CrossSlopes { get; private set; }
      internal Profile interpretCrossSlopes { get; set; }

      private tupleNullableDoubles resultScratchpad;

      public ribbonBase() { }

      public ribbonBase(CogoStation beginStation, CogoStation endStation, double initialWidth, Slope initialSlope)
      {
         interpretWidths = new Profile(beginStation, endStation, (double) enmWidthInterpret.HorizontalOnly);
         Widths = new Profile(beginStation, endStation, initialWidth);
         interpretCrossSlopes = new Profile(beginStation, endStation, (double)enmCrossSlopeInterpret.xPercentage);
         CrossSlopes = new Profile(beginStation, endStation, initialSlope);
         LiederLineHeight = 5.0;
      }

      public virtual void addWidenedSegment(CogoStation beginOpenTaperStation, CogoStation endOpenTaperStation, double newTotalWidth,
                                    CogoStation beginCloseTaperStation, CogoStation endCloseTaperStation)
      {
         double startWidth, endWidth;
         Widths.getElevation(beginOpenTaperStation, out resultScratchpad);
         startWidth = (double)resultScratchpad.back;
         Widths.getElevation(endCloseTaperStation, out resultScratchpad);
         endWidth = (double)resultScratchpad.back;

         Widths.addStationAndElevation(beginOpenTaperStation, startWidth);
         Widths.addStationAndElevation(endOpenTaperStation, newTotalWidth);
         Widths.addStationAndElevation(beginCloseTaperStation, newTotalWidth);
         Widths.addStationAndElevation(endCloseTaperStation, endWidth);

      }

      public virtual void addCrossSlopeChangedSegment(CogoStation beginOpenTaperStation, CogoStation endOpenTaperStation, double crossSlope,
                                    CogoStation beginCloseTaperStation, CogoStation endCloseTaperStation)
      {
         double startCrossSlope, endCrossSlope;
         CrossSlopes.getElevation(beginOpenTaperStation, out resultScratchpad);
         startCrossSlope = (double)resultScratchpad.back;
         CrossSlopes.getElevation(endCloseTaperStation, out resultScratchpad);
         endCrossSlope = (double)resultScratchpad.back;

         CrossSlopes.addStationAndElevation(beginOpenTaperStation, startCrossSlope);
         CrossSlopes.addStationAndElevation(endOpenTaperStation, crossSlope);
         CrossSlopes.addStationAndElevation(beginCloseTaperStation, crossSlope);
         CrossSlopes.addStationAndElevation(endCloseTaperStation, endCrossSlope);

      }

      public virtual void moveToOuterEdge(ref StationOffsetElevation aSOE, int whichSide)
      {
         //double traversedWidth;
         tupleNullableDoubles result;
         double? theWidth = getActualWidth((CogoStation)aSOE.station, out result);
         if (result.isSingleValue == false)
         { throw new NotImplementedException("Width discontinuity is not allowed. This happens at station = " + aSOE.station); }

         double? theCrossSlope = getCrossSlope((CogoStation)aSOE.station, out result);
         if (result.isSingleValue == false)
         { throw new NotImplementedException("Cross slope discontinuity is not allowed. This happens at station = " + aSOE.station); }

         if (theWidth == null) theWidth = 0.0;

         aSOE.offset += theWidth * whichSide;

         if (theCrossSlope == null) theCrossSlope = 0.0;
         aSOE.elevation += theCrossSlope * theWidth;
      }

      public virtual void accumulateRibbonTraversal(ref StationOffsetElevation aSOE)
      {
         double traversedWidth;
         tupleNullableDoubles result;
         double? availableWidth = getActualWidth((CogoStation) aSOE.station, out result);
         if (result.isSingleValue == false)
         { throw new NotImplementedException("Width discontinuity is not allowed. This happens at station = " + aSOE.station);}

         double? crossSlope = getCrossSlope((CogoStation)aSOE.station, out result);
         if (result.isSingleValue == false)
         { throw new NotImplementedException("Cross slope discontinuity is not allowed. This happens at station = " + aSOE.station); }

         if (availableWidth == null) availableWidth = 0.0;

         if ((double)availableWidth > aSOE.offset)
         {
            traversedWidth = aSOE.offset;
            aSOE.offset = 0.0;
         }
         else
         {
            traversedWidth = (double) availableWidth;
            aSOE.offset -= traversedWidth;
         }

         if (crossSlope == null) crossSlope = 0.0;
         aSOE.elevation += traversedWidth * (double)crossSlope;
      }

      protected double LiederLineHeight { get; set; }
      protected bool SuppressSlopeText { get; set; }

      public void setupCrossSectionDrawing(IRM21cad2dDrawingContext cadContext)
      {
         cadContext.resetDashArray();
      }

      public virtual void DrawCrossSection(IRM21cad2dDrawingContext cadContext, 
         ref StationOffsetElevation aSOE, int whichSide)
      {
         double LLH = LiederLineHeight;
         double ribbonWidth;
         double X1 = aSOE.offset;
         double Y1 = aSOE.elevation;
         this.moveToOuterEdge(ref aSOE, whichSide);
         if (X1 == aSOE.offset && Y1 == aSOE.elevation) return;

         cadContext.Draw(X1, Y1, aSOE.offset, aSOE.elevation);

         setupCrossSectionDrawing(cadContext);
         ribbonWidth = Math.Abs(aSOE.offset - X1);
         cadContext.setElementWeight(0.8);
         cadContext.setElementColor(Color.FromArgb(124, 255, 255, 255));
         cadContext.Draw(X1, LLH + 0.5, aSOE.offset, LLH + 0.5);
         cadContext.Draw(aSOE.offset, 0.5, aSOE.offset, LLH + 1.5);
         string widthStr = (Math.Round(ribbonWidth*10)/10).ToString();
         cadContext.Draw(widthStr, X1 + whichSide * ribbonWidth / 2, LLH + 0.5, 0.0);

         if (false == SuppressSlopeText)
         {
            Slope mySlope = new Slope((aSOE.elevation - Y1) / (aSOE.offset - X1));
            if (whichSide > 0)
               cadContext.Draw(mySlope.ToString(),
                  (X1 + aSOE.offset) / 2,
                  (Y1 + aSOE.elevation) / 2,
                  mySlope.getAsDegrees());
            else
               cadContext.Draw(mySlope.FlipDirection().ToString(),
                  (X1 + aSOE.offset) / 2,
                  (Y1 + aSOE.elevation) / 2,
                  mySlope.getAsDegrees());
         }
         SuppressSlopeText = false;
      }


      public virtual void DrawPlanViewSchematic(IRM21cad2dDrawingContext cadContext,
         int whichSide)
      {

      }
      
      public virtual double? getActualWidth(CogoStation aStation)
      {
         tupleNullableDoubles rslt = new tupleNullableDoubles();
         return getActualWidth(aStation, out rslt);
      }

      public virtual double? getActualWidth(CogoStation aStation, out tupleNullableDoubles result)
      {
         Widths.getElevation(aStation, out result);
         if (result.back != null && result.isSingleValue == false)
         {
            return result.ahead;
         }
         return result.back;
      }

      public virtual double? getCrossSlope(CogoStation aStation, out tupleNullableDoubles result)
      {
         CrossSlopes.getElevation(aStation, out result);
         if (result.back != null && result.isSingleValue == false)
         {
            return result.ahead;
         }
         return result.back;
      }

      public virtual int getChildRibbonCount()
      {
         return 1;
      }

      public IRibbonLike getChildRibbonByIndex(int index)
      {
         return this;
      }

      public CogoStation BeginStation
      {
         get { return (CogoStation) Widths.beginProfTrueStation; }
         private set { }
      }

      public CogoStation EndStation
      {
         get { return (CogoStation)Widths.endProfTrueStation; }
         private set { }
      }

      public int getMyIndex() { return MyIndex; }
      public void setMyIndex(int index) { MyIndex = index; }
      public int MyIndex { get { return myIndex_; } set { myIndex_ = value; } }
      public void incrementMyIndex()
      {
         if (myIndex_ < 0)
            myIndex_--;
         else
            myIndex_++;
      }

      public virtual string getHashName(){return "not implemented: Class = ribbonBase";}
      public ObservableCollection<Irm21TreeViewItemable> getChildren() { return null; }

      /// <summary>
      /// Raises the property changed event.
      /// </summary>
      /// <param name="propertyName">Name of the property.</param>
      protected void RaisePropertyChanged(string propertyName)
      {
         var handler = this.PropertyChanged;
         if (handler != null)
         {
            handler(this, new PropertyChangedEventArgs(propertyName));
         }
      }

      /// <summary>
      /// Occurs when a property value changes.
      /// </summary>
      public event PropertyChangedEventHandler PropertyChanged;
   }

   public enum enmWidthInterpret
   {
      HorizontalOnly = 1, // most common case: the elevation in Profile Widths is the horizontal width of the ribbon
      VerticalOnly = 2,   // the elevation in Profile Widths is the height of the ribbon
      Hybrid = 3,         // when slope < 45 degrees, interpret as width; when slope > 45 degrees, intepret as height
      LengthAlong = 4,    // length along the element given its slope: the tangent instead of the cosine
      RaySheet = 5,       // the ribbon is really a ray sheet. Resolve down the slope of the ray until intersecting another surface
   }

   public enum enmCrossSlopeInterpret
   {
      xPercentage = 1,
      xTo1 = 2,
      degrees = 3,
      yTo1 = 4,
      yPercentage = 5,
      straightLineInRelativeSpace = 6,
      straightLineInWorldSpace = 7
   }
}
