using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ptsCogo.Horizontal
{
   public class rm21HorizontalAlignment : HorizontalAlignmentBase
   {
      private List<HorizontalAlignmentBase> allChildSegments = null;

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
         
         if (null == allChildSegments) allChildSegments = new List<HorizontalAlignmentBase>();

         allChildSegments.Add(aLineSeg);
      }

      private void createAddArcSegment(IRM21fundamentalGeometry fundGeomArcSeg)
      {
         ptList = fundGeomArcSeg.getPointList();
         if (3 != ptList.Count)
            throw new Exception("Arc Segment fundamental geometry must have three and only three points.");

         throw new NotImplementedException("RM21 Horizontal Alignment Arc Segment");
      }

      private void createAddEulerSpiralSegment(IRM21fundamentalGeometry fundGeomEulerSpiralSeg)
      {
         ptList = fundGeomEulerSpiralSeg.getPointList();
         if (ptList.Count < 4)
            throw new Exception("Euler Spiral Segment fundamental geometry must have at least four points.");

         throw new NotImplementedException("RM21 Horizontal Alignment Euler Sprial Segment");
      }

      private void restationAlignment()
      {
         Double runningTrueStation = this.BeginStation;
         foreach (var segment in allChildSegments)
         {
            segment.BeginStation = runningTrueStation;
            runningTrueStation += segment.Length;
            segment.EndStation = runningTrueStation;
            this.EndStation = runningTrueStation;
         }
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
   }
}
