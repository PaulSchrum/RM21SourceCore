using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using ptsCogo.coordinates.CurvilinearCoordinates;
using ptsCogo;
using ptsCogo.Angle;
using System.ComponentModel;



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

         aSOE.elevation += traversedWidth * (double)crossSlope;
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
