﻿using ptsCogo.coordinates.CurvilinearCoordinates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ptsCogo.Angle;
using ptsCogo.coordinates;
using System.ComponentModel;

namespace ptsCogo.Horizontal
{
   public class rm21HorizontalAlignment : HorizontalAlignmentBase
   {
      private List<HorizontalAlignmentBase> allChildSegments = null;
      private List<alignmentDataPacket> alignmentData { get; set; }

      // used in constructing, never kept long term
      // scratch pad member for re-use.  Not part of the data structure.
      private List<HorizontalAlignmentBase> allChildSegments_scratchPad = null;

      // scratch pad member for re-use.  Not part of the data structure.
      List<ptsPoint> ptList = null;

      public rm21HorizontalAlignment() { }

      public rm21HorizontalAlignment(List<IRM21fundamentalGeometry> fundamentalGeometryList,
         String Name, List<Double> stationEquationing)
         : base(stationEquationing)
      {
         if (null == fundamentalGeometryList)
         { throw new NullReferenceException("Fundamental Geometry List may not be null."); }

         this.Name = Name;

         createAllSegments(fundamentalGeometryList);
         orderAllSegments_startToEnd();

         restationAlignment();
      }

      private void createAllSegments(List<IRM21fundamentalGeometry> fundamentalGeometryList)
      {
         foreach (var fgElement in fundamentalGeometryList)
         {
            switch (fgElement.getExpectedType())
            {
               case expectedType.LineSegment:
                  createAddLineSegment(fgElement);
                  break;
               case expectedType.EulerSpiral:
                  createAddEulerSpiralSegment(fgElement);
                  break;
               default:
                  createAddArcSegment(fgElement);
                  break;
            }
         }
      }

      private void createAddLineSegment(IRM21fundamentalGeometry fundGeomLineSeg)
      {
         ptList = fundGeomLineSeg.getPointList();
         if (2 != ptList.Count)
            throw new Exception("Line Segment fundamental geometry must have two and only two points.");

         rm21HorLineSegment aLineSeg = new rm21HorLineSegment(ptList[0], ptList[1]);
         
         if (null == allChildSegments_scratchPad) allChildSegments_scratchPad = new List<HorizontalAlignmentBase>();

         allChildSegments_scratchPad.Add(aLineSeg);
      }

      private void createAddArcSegment(IRM21fundamentalGeometry fundGeomArcSeg)
      {
         ptList = fundGeomArcSeg.getPointList();
         if (3 != ptList.Count)
            throw new Exception("Arc Segment fundamental geometry must have three and only three points.");

         rm21HorArc anArg = new rm21HorArc(ptList[0], ptList[1], ptList[2], fundGeomArcSeg.getExpectedType(),
            fundGeomArcSeg.getDeflectionSign());

         if (null == allChildSegments_scratchPad) allChildSegments_scratchPad = new List<HorizontalAlignmentBase>();

         allChildSegments_scratchPad.Add(anArg);
      }


      private void createAddEulerSpiralSegment(IRM21fundamentalGeometry fundGeomEulerSpiralSeg)
      {
         ptList = fundGeomEulerSpiralSeg.getPointList();
         if (ptList.Count < 4)
            throw new Exception("Euler Spiral Segment fundamental geometry must have at least four points.");

         throw new NotImplementedException("RM21 Horizontal Alignment Euler Sprial Segment");
      }
      
      private void orderAllSegments_startToEnd()
      {
         var candidateFirstItems = new List<HorizontalAlignmentBase>();

         // Figure out which element is first.
         // if more than one element has no prior connection, it is in error.
         foreach (var item in allChildSegments_scratchPad)
         {
            bool hasNoBeginConnection = true;
            foreach (var otherItem in allChildSegments_scratchPad)
            {
               if (item != otherItem)
               {
                  hasNoBeginConnection = !theseConnectAtItemBeginPt(item, otherItem);
                  if (false == hasNoBeginConnection)
                     break;
               }
            }

            if (true == hasNoBeginConnection)
               candidateFirstItems.Add(item);
         }

         if (candidateFirstItems.Count == 1)
         {
            HorizontalAlignmentBase geometricallyFirstElement = candidateFirstItems.FirstOrDefault<HorizontalAlignmentBase>();
            allChildSegments = new List<HorizontalAlignmentBase>();
            allChildSegments.Add(geometricallyFirstElement);
            allChildSegments_scratchPad.Remove(geometricallyFirstElement);
         }
         else if (candidateFirstItems.Count > 1)
         {
            throw new Exception("Disjointed element list not permitted");
         }
         else
         {
            throw new Exception("Can't determine which element is first.");
         }
         // end "Figure out which element is first."
         // if more than one element has no prior connection, it is in error.

         var currentLastItem = allChildSegments.Last<HorizontalAlignmentBase>();
         if (0 == allChildSegments_scratchPad.Count) return;

         while (allChildSegments_scratchPad.Count > 0)
         {
            var Iter = allChildSegments_scratchPad.GetEnumerator();
            while (Iter.MoveNext())
            {
               if (true == theseConnectAtItemBeginPt(Iter.Current, currentLastItem))
               {
                  allChildSegments.Add(Iter.Current);
                  currentLastItem = allChildSegments.Last<HorizontalAlignmentBase>();
                  allChildSegments_scratchPad.Remove(Iter.Current);
                  break;
               }
            }
         }
         allChildSegments_scratchPad = null;

      }

      private bool theseConnectAtItemBeginPt(HorizontalAlignmentBase itemInQuestion, HorizontalAlignmentBase secondItem)
      {
         Double equalityTolerance = 0.00015;

         var distanceToEndVector = itemInQuestion.BeginPoint - secondItem.EndPoint;
         var distanceToBeginVector = itemInQuestion.BeginPoint - secondItem.BeginPoint;
         return (Math.Abs(distanceToEndVector.Length) < equalityTolerance) ||
                (Math.Abs(distanceToBeginVector.Length) < equalityTolerance);
      }

      private void restationAlignment()
      {
         if (null == allChildSegments) return;
         Double runningTrueStation = this.BeginStation;
         foreach (var segment in allChildSegments)
         {
            segment.BeginStation = runningTrueStation;
            runningTrueStation += segment.Length;
            segment.EndStation = runningTrueStation;
            this.EndStation = runningTrueStation;
         }
      }

      public override StringBuilder createTestSetupOfFundamentalGeometry()
      {
         throw new NotImplementedException();
         var sb = new StringBuilder();

         foreach (var item in allChildSegments)
         {
            sb.Append(item.createTestSetupOfFundamentalGeometry());
         }

         return sb;
      }

      public new Double Length
      {
         get { return this.EndStation - this.BeginStation; }
         private set { }
      }

      public void getCrossSectionEndPoints(
         CogoStation station, 
         out ptsPoint leftPt, 
         double leftOffset, 
         out ptsPoint rightPt, 
         double rightOffset)
      {
         leftPt = null;
         rightPt = null;
      }

      public override List<ptsPoint> getPoints(coordinates.CurvilinearCoordinates.StationOffsetElevation anSOE)
      {
         var returnList = new List<ptsPoint>();

         foreach (var segment in allChildSegments)
         {
            returnList.AddRange(segment.getPoints(anSOE));
         }

         return returnList;
      }

      public override List<StationOffsetElevation> getStationOffsetElevation(ptsPoint aPoint)
      {
         var returnList = new List<StationOffsetElevation>();

         foreach (var segment in allChildSegments)
         {
            var SOEforThisSegment = segment.getStationOffsetElevation(aPoint);
            if (null != SOEforThisSegment)
               returnList.AddRange(SOEforThisSegment);
         }

         returnList = returnList.OrderBy(soe => Math.Abs(soe.offset)).ToList<StationOffsetElevation>();

         return returnList;
      }

      public ptsPoint getXYZcoordinates(double station, double offset, double elevation)
      {
         var anSOE = new StationOffsetElevation(station, new Offset(offset), new Elevation(elevation));
         return getXYZcoordinates(anSOE);
      }

      public override ptsPoint getXYZcoordinates(StationOffsetElevation anSOE)
      {
         if (anSOE.station < this.BeginStation || anSOE.station > this.EndStation)
            return null;

         ptsPoint returnPoint = new ptsPoint();
         foreach (var segment in this.allChildSegments)
         {
            if (anSOE.station < segment.EndStation)
            {
               return segment.getXYZcoordinates(anSOE);
            }
         }
         return null;
      }

      public List<CogoStation> getChangePoints()
      {
         List<CogoStation> returnList =
            (from item in allChildSegments
            select (CogoStation) item.BeginStation).ToList();
         returnList.Add((CogoStation) this.EndStation);
         return returnList;
      }

      public HorizontalAlignmentBase GetElementByStation(double testStation)
      {
         HorizontalAlignmentBase returnItem = null;
         foreach (var item in allChildSegments)
         {
            if (testStation <= item.EndStation)
            {
               returnItem = item;
               break;
            }
         }
         return returnItem;
      }

      public void reset(ptsPoint pt1, ptsPoint pt2)
      {
         var lineSeg = new rm21HorLineSegment(pt1, pt2);
         lineSeg.Parent = this;
         allChildSegments = new List<HorizontalAlignmentBase>();
         allChildSegments.Add(lineSeg);
         this.BeginStation = 0.0;
         this.EndStation = lineSeg.EndStation;
         this.Length = this.EndStation - this.BeginStation;
         this.BeginAzimuth = lineSeg.BeginAzimuth;
         this.EndAzimuth = lineSeg.EndAzimuth;
         this.BeginDegreeOfCurve = this.EndDegreeOfCurve = 0.0;
         this.BeginPoint = lineSeg.BeginPoint;
         this.EndPoint = lineSeg.EndPoint;
         restationAlignment();

         alignmentData = new List<alignmentDataPacket>();
         alignmentData.Add(new alignmentDataPacket(0, lineSeg));
      }

      public void appendArc(ptsPoint ArcEndPoint, Double radius)
      {
         var newArc = new rm21HorArc(this.EndPoint, ArcEndPoint, this.EndAzimuth, radius);
         newArc.BeginStation = this.EndStation;
         newArc.EndStation = newArc.BeginStation + newArc.Length;

         newArc.Parent = this;
         this.allChildSegments.Add(newArc);

         this.EndAzimuth = this.BeginAzimuth + newArc.Deflection;
         this.EndPoint = newArc.EndPoint;
         restationAlignment();

         alignmentData.Add(new alignmentDataPacket(alignmentData.Count, newArc));
      }

      public void appendTangent(ptsPoint TangentEndPoint)
      {
         // See "Solving SSA triangles" 
         // http://www.mathsisfun.com/algebra/trig-solving-ssa-triangles.html

         var lastChainItem = allChildSegments.Last();
         if (!(lastChainItem is rm21HorArc))
            throw new Exception("Can't add tangent on a tangent segment.");

         var finalArc = lastChainItem as rm21HorArc;

         var incomingTanRay = new ptsRay(); incomingTanRay.StartPoint = finalArc.BeginPoint;
         incomingTanRay.HorizontalDirection = finalArc.BeginAzimuth;
         int offsetSide = Math.Sign(incomingTanRay.getOffset(TangentEndPoint));
         double rad = finalArc.Radius;
         Azimuth traverseToRevisedCenterPtAzimuth = finalArc.BeginAzimuth + offsetSide * Math.PI/2.0;
         ptsVector traverseToRevisedCenterPt = new ptsVector(traverseToRevisedCenterPtAzimuth, rad);
         ptsPoint revCenterPt = finalArc.BeginPoint + traverseToRevisedCenterPt;

         ptsVector ccToTEPvec = finalArc.ArcCenterPt - TangentEndPoint;

         ptsAngle rho = Math.Asin(rad / ccToTEPvec.Length);
         ptsAngle ninetyDegrees = new ptsAngle();
         ninetyDegrees.setFromDegreesDouble(90.0);
         ptsAngle tau = ninetyDegrees - rho;  

         Azimuth ccToPtVecAz = ccToTEPvec.Azimuth;
         Azimuth arcBegRadAz = finalArc.BeginRadiusVector.Azimuth;

         ptsCogo.Angle.Deflection outerDefl = ccToPtVecAz.minus(arcBegRadAz);
         ptsDegree  defl = Math.Abs((tau - outerDefl).getAsDegreesDouble()) * offsetSide;
         Deflection newDeflection = new Deflection(defl.getAsRadians());
         finalArc.setDeflection(newDeflection: newDeflection);

         var appendedLineSegment = new rm21HorLineSegment(finalArc.EndPoint, TangentEndPoint);
         appendedLineSegment.BeginStation = finalArc.EndStation;
         appendedLineSegment.Parent = this;
         allChildSegments.Add(appendedLineSegment);

         this.EndAzimuth = appendedLineSegment.EndAzimuth;
         this.EndPoint = appendedLineSegment.EndPoint;
         restationAlignment();

         if (alignmentData.Count > 0)
            alignmentData.RemoveAt(alignmentData.Count-1);
         alignmentData.Add(new alignmentDataPacket(alignmentData.Count, finalArc));
         alignmentData.Add(new alignmentDataPacket(alignmentData.Count, appendedLineSegment));
      }

      public override void draw(ILinearElementDrawer drawer)
      {
         foreach (var child in allChildSegments)
         {
            child.draw(drawer);
         }
         drawer.setAlignmentValues(this.alignmentData);  // technical debt: add tests for this
      }

      public void drawTemporary(ILinearElementDrawer drawer)
      {
         drawer.setDrawingStateTemporary();
         this.draw(drawer);
      }

      public void drawPermanent(ILinearElementDrawer drawer)
      {
         drawer.setDrawingStatePermanent();
         this.draw(drawer);
      }

      public long childCount()
      {
         return this.allChildSegments.Count;
      }

      public void removeFinalChildItem()
      {
         allChildSegments.RemoveAt(allChildSegments.Count - 1);
         var newLastElement = allChildSegments[allChildSegments.Count - 1];
         this.EndAzimuth = newLastElement.EndAzimuth;
         this.EndDegreeOfCurve = newLastElement.EndDegreeOfCurve;
         this.EndPoint = newLastElement.EndPoint;
         this.EndStation = newLastElement.EndStation;
         alignmentData.RemoveAt(alignmentData.Count - 1);
      }

      public String getDeflectionOfFinalArc()
      {
         int index;
         for (index = allChildSegments.Count - 1; index >= 0; index--)
         {
            var child = allChildSegments[index];
            if (child is rm21HorArc) return (child as rm21HorArc).Deflection.ToString();
         }
         return String.Empty;
      }
   }

   public sealed class alignmentDataPacket
   {

      public int myIndex { get; set; }
      public Double BeginStationDbl { get; set; }
      public Double Length { get; set; }
      public Double Radius { get; set; }
      public Deflection Deflection { get; set; }
      public Boolean HasChanged { get; set; }

      public alignmentDataPacket(int p, HorizontalAlignmentBase alignmentItem)
      {
         myIndex = p;
         BeginStationDbl = alignmentItem.BeginStation;
         Length = alignmentItem.Length;
         Radius = alignmentItem.Radius;
         Deflection = alignmentItem.Deflection;
         HasChanged = true;
      }
   }

}
