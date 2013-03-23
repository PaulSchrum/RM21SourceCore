using ptsCogo.coordinates.CurvilinearCoordinates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ptsCogo.Horizontal
{
   public class rm21HorizontalAlignment : HorizontalAlignmentBase
   {
      private List<HorizontalAlignmentBase> allChildSegments = null;

      // used in constructing, never kept long term
      // scratch pad member for re-use.  Not part of the data structure.
      private List<HorizontalAlignmentBase> allChildSegments_scratchPad = null;

      // scratch pad member for re-use.  Not part of the data structure.
      List<ptsPoint> ptList = null;

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

         var distanceVector = itemInQuestion.BeginPoint - secondItem.EndPoint;
         return (Math.Abs(distanceVector.Length) < equalityTolerance);
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


   }
}
