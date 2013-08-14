using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using ptsCogo.coordinates.CurvilinearCoordinates;
using ptsCogo;
using ptsCogo.Angle;
using System.ComponentModel;
using System.Windows.Media;
using ptsCogo.Horizontal;



namespace ptsCogo
{
   public abstract class ribbonBase : IRibbonLike, INotifyPropertyChanged
   {
      private PGLGrouping myParentPGLgrouping_;
      public PGLGrouping MyParentPGLgrouping
      {
         get { return myParentPGLgrouping_; }
         set
         {
            myParentPGLgrouping_ = value;
         }
      }

      public rm21HorizontalAlignment GoverningAlignment
      {
         get
         {
            if (null == MyParentPGLgrouping) return null;
            return MyParentPGLgrouping.GoverningAlignment;
         }
         private set { }
      }

      private int myIndex_ { get; set; }
      protected rm21Side myProgressionDirection { get; set; }
      private bool progressionDirectionHasBeenSet=false;

      private IRibbonLike nextRibbonInward_;
      protected IRibbonLike nextRibbonInward
      {
         get { return nextRibbonInward_; }
         private set 
         {
            if (null != nextRibbonInward_)
               nextRibbonInward_.onOffsetsChanged -= computeOffsetsProfile;

            nextRibbonInward_ = value;
            if (null != nextRibbonInward_)
            {
               nextRibbonInward_.onOffsetsChanged += computeOffsetsProfile;
            }

            computeOffsetsProfile_();
         } 
      }
      public event EventHandler onOffsetsChanged;

      public Slope NominalCrossSlope { get; set; }
      public Profile CrossSlopes { get; protected set; }
      internal Profile interpretCrossSlopes { get; set; }

      public Double NominalWidth { get; set; }
      public Profile Widths { get; protected set; }
      internal Profile interpretWidths { get; set; }
      internal Profile myOffsets { get; set; }

      public void resetWidths()
      {
         Widths = new Profile(this.BeginStation, this.EndStation, NominalWidth);
      }

      public void resetCrossSlopes()
      {
         CrossSlopes = new Profile(this.BeginStation, this.EndStation, NominalCrossSlope);
      }

      private tupleNullableDoubles resultScratchpad;

      public ribbonBase() { }

      /// <summary>
      /// 
      /// </summary>
      /// <param name="parentPGLg">Required non-null</param>
      /// <param name="initialWidth"></param>
      /// <param name="initialSlope">Required non-null</param>
      public ribbonBase(PGLGrouping parentPGLg, Double initialWidth, Slope initialSlope)
      {
         MyParentPGLgrouping = parentPGLg;
         NominalWidth = initialWidth;
         resetWidths();
         NominalCrossSlope = initialSlope;
         resetCrossSlopes();
      }

      public ribbonBase(CogoStation beginStation, CogoStation endStation, double initialWidth, Slope initialSlope)
      {
         if (null == beginStation) throw new ArgumentNullException("beginStation");
         if (null == endStation) throw new ArgumentNullException("endStation");

         interpretWidths = new Profile(beginStation, endStation, (double) enmWidthInterpret.HorizontalOnly);
         NominalWidth = initialWidth;
         Widths = new Profile(beginStation, endStation, initialWidth);
         interpretCrossSlopes = new Profile(beginStation, endStation, (double)enmCrossSlopeInterpret.xPercentage);
         NominalCrossSlope = initialSlope;
         resetCrossSlopes();
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

         Widths.appendStationAndElevation(beginOpenTaperStation, startWidth);
         Widths.appendStationAndElevation(endOpenTaperStation, newTotalWidth);
         Widths.appendStationAndElevation(beginCloseTaperStation, newTotalWidth);
         Widths.appendStationAndElevation(endCloseTaperStation, endWidth);

         computeOffsetsProfile_();
      }

      public virtual void addCrossSlopeChangedSegment(CogoStation beginOpenTaperStation, CogoStation endOpenTaperStation, double crossSlope,
                                    CogoStation beginCloseTaperStation, CogoStation endCloseTaperStation)
      {
         double startCrossSlope, endCrossSlope;
         CrossSlopes.getElevation(beginOpenTaperStation, out resultScratchpad);
         startCrossSlope = (double)resultScratchpad.back;
         CrossSlopes.getElevation(endCloseTaperStation, out resultScratchpad);
         endCrossSlope = (double)resultScratchpad.back;

         CrossSlopes.appendStationAndElevation(beginOpenTaperStation, startCrossSlope);
         CrossSlopes.appendStationAndElevation(endOpenTaperStation, crossSlope);
         CrossSlopes.appendStationAndElevation(beginCloseTaperStation, crossSlope);
         CrossSlopes.appendStationAndElevation(endCloseTaperStation, endCrossSlope);

      }

      public virtual void moveToOuterEdge(ref StationOffsetElevation aSOE, int whichSide)
      {
         //double traversedWidth;
         tupleNullableDoubles result;
         double? theWidth = getActualWidth((CogoStation)aSOE.station, out result);
         if (result.isSingleValue == false)
         {
            if (result.ahead !=null)
            {
               theWidth = result.ahead;
            }
            else if (result.back != null)
            {
               theWidth = result.back;
            }
            else
            {
               throw new NotImplementedException("Width discontinuity is not allowed. This happens at station = " + aSOE.station);
            }
         }

         double? theCrossSlope = getCrossSlope((CogoStation)aSOE.station, out result);
         if (result.isSingleValue == false)
         {
            if (result.ahead != null)
            {
               theCrossSlope = result.ahead;
            }
            else if (result.back != null)
            {
               theCrossSlope = result.back;
            }
            else
            {
               throw new NotImplementedException("Width discontinuity is not allowed. This happens at station = " + aSOE.station);
            }
         }

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
         if (aSOE.elevation == null) aSOE.elevation = new Elevation(0.0);
         aSOE.elevation += traversedWidth * (double)crossSlope;
      }

      protected double LiederLineHeight { get; set; }
      protected bool SuppressSlopeText { get; set; }

      protected void computeOffsetsProfile(Object sender, EventArgs e)
      {
         computeOffsetsProfile_();
      }

      protected void computeOffsetsProfile_()
      {
         if (this is ptsCogo.Ribbons.RaySheetBase)
            return;
         Profile innerOffsetsProfile = null;
         if (nextRibbonInward is ribbonBase)
         {
            innerOffsetsProfile = nextRibbonInward.getOffsetProfile();
         }
         myOffsets = Profile.arithmaticAddProfile(innerOffsetsProfile, Widths, getMyScaleFactor());
         // later optimization: do not fire event if Offsets does not really change
         var tempOnOffsetsChange = onOffsetsChanged;
         if (tempOnOffsetsChange != null)
            tempOnOffsetsChange(this, new EventArgs());
      }

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
                  mySlope.getAsDegreesDouble());
            else
               cadContext.Draw(mySlope.FlipDirection().ToString(),
                  (X1 + aSOE.offset) / 2,
                  (Y1 + aSOE.elevation) / 2,
                  mySlope.getAsDegreesDouble());
         }
         SuppressSlopeText = false;
      }


      public virtual void DrawPlanViewSchematic(IRM21cad2dDrawingContext cadContext,
         int whichSide)
      {
         if (null != this.myOffsets) 
            this.myOffsets.draw(cadContext);
      }

      public virtual void PersistantDraw(IPersistantDrawer drawer)
      {
         StationOffsetElevation soe1 = null;
         StationOffsetElevation soe2 = new StationOffsetElevation();
         var drawPoints = getOutsideEdgeDrawPoints();
         if (null == drawPoints) return;
         foreach (var sta2 in drawPoints)
         {
            soe2 = this.getOutsideEdgeStationOffsetElevation(sta2);
            if (soe1 != null)
            {
               Double aveStation = (sta2.trueStation + soe1.station) / 2.0;
               HorizontalAlignmentBase HAelement = 
                  this.GoverningAlignment.GetElementByStation(aveStation);
               HAelement.drawHorizontalByOffset(drawer, soe1, soe2);
            }
            soe1 = soe2;
         }
      }

      protected List<CogoStation> getOutsideEdgeDrawPoints()
      {
         if (null == this.GoverningAlignment) return null;
         if (null == this.myOffsets) return null;
         var drawPointsPrelim = this.GoverningAlignment.getChangePoints();
         drawPointsPrelim.AddRange(this.myOffsets.getChangePoints());

         var retList = drawPointsPrelim.OrderBy(sta => sta.trueStation).ToList();
         for (int i = retList.Count - 2; i >= 0; i--)
         {
            if (0 ==
                     utilFunctions.tolerantCompare(retList[i].trueStation,
                     retList[i + 1].trueStation, 0.00001))
            {
               retList.RemoveAt(i + 1);
            }
         }

         return retList;
      }

      public StationOffsetElevation getOutsideEdgeStationOffsetElevation(CogoStation sta)
      {
         StationOffsetElevation returnItem = new StationOffsetElevation();
         returnItem.station = sta.trueStation;
         returnItem.offset = this.myOffsets.getElevation(sta);
         returnItem.elevation = 0.0;

         return returnItem;
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

      public virtual Slope getCrossSlope(CogoStation aStation) 
      {
         tupleNullableDoubles rslt = new tupleNullableDoubles();
         Double? xSlope = getCrossSlope(aStation, out rslt);
         if (xSlope == null) return null;
         else return new Slope((Double)xSlope);
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
         get 
         { 
            if (null == GoverningAlignment)
               return (CogoStation) Widths.BeginProfTrueStation;

            return (CogoStation) GoverningAlignment.BeginStation;
         }
         private set { }
      }

      public CogoStation EndStation
      {
         get
         {
            if (null == GoverningAlignment)
               return (CogoStation) Widths.EndProfTrueStation;

            return (CogoStation) GoverningAlignment.EndStation;
         }
         private set { }
      }

      public void setMyProgressionDirection(rm21Side side) 
      { 
         myProgressionDirection = side;
         progressionDirectionHasBeenSet = true;
      }

      public virtual int getMyScaleFactor() 
      {
         if (progressionDirectionHasBeenSet == false)
            return 1;

         return myProgressionDirection.Sign();
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

      public void setInsideRibbon(IRibbonLike insideRibbon)
      {
         nextRibbonInward = insideRibbon;
      }

      public virtual Profile getOffsetProfile()
      {
         return myOffsets;
      }

      public void setPGLgroupingParent(PGLGrouping pglGrouping)
      {
         MyParentPGLgrouping = pglGrouping;
      }

      public virtual string getHashName() { return "getHashName() is not implemented for Class = ribbonBase"; }
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
