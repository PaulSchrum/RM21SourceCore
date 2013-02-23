using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ptsCogo.Horizontal;
using ptsCogo;

namespace NUnitTestingLibrary.Mocks
{
   public class rm21MockFundamentalGeometry : ptsCogo.Horizontal.IRM21fundamentalGeometry
   {
      private expectedType expectedType_;
      public expectedType expectedType 
      {
         get { return expectedType_; }
         set
         {
            if (null == pointList)
            {
               expectedType_ = value;
            }
            else
            {
               if (pointList.Count == 2)
               { expectedType_ = ptsCogo.Horizontal.expectedType.LineSegment; }
               else if (pointList.Count > 3)
               { expectedType_ = ptsCogo.Horizontal.expectedType.EulerSpiral; }
               else
               {
                  if (true == isArc())
                  {
                     expectedType_ = value;
                  }
                  else
                     expectedType_ = ptsCogo.Horizontal.expectedType.ArcSegmentInsideSolution;
               }
            }
         }
      }

      private List<ptsPoint> pointList_;
      public List<ptsPoint> pointList {
         get { return pointList_; }
         set
         {
            if (null == value) return;
            if (0 == value.Count) return;

            if (value.Count < 2) throw new Exception("Point List must have 2 or more points to be valid.");

            pointList_ = value;
            if (pointList_.Count == 2)
            {
               expectedType = ptsCogo.Horizontal.expectedType.LineSegment;
            }

         }
      }

      public List<ptsPoint> getPointList() { return pointList; }

      public Double getBeginningDegreeOfCurve()
      {
         if (expectedType == expectedType.EulerSpiral)
         {
            throw new NotImplementedException();
         }
         else
            return getDegreeOfCurveNotAspiral();
      }

      private Double getDegreeOfCurveNotAspiral()
      {
         if (true == this.isArc())
         {
            return 100.0 / ptsCogo.ptsAngle.radiansFromDegree(pointList[0].GetHorizontalDistanceTo(pointList[1]));
         }
         else if (expectedType == ptsCogo.Horizontal.expectedType.EulerSpiral)
            throw new Exception("Private method can not compute spiral degree");
         
         return 0.0;
      }

      public Double getEndingDegreeOfCurve()
      {
         if (expectedType == expectedType.EulerSpiral)
         {
            throw new NotImplementedException();
         }
         else
            return getDegreeOfCurveNotAspiral();
      }
      
      public expectedType getExpectedType()
      {
         return expectedType;
      }

      public bool isArc()
      {
         return (this.expectedType == ptsCogo.Horizontal.expectedType.ArcSegmentInsideSolution ||
                 this.expectedType == ptsCogo.Horizontal.expectedType.ArcSegmentOutsideSoluion ||
                 this.expectedType == ptsCogo.Horizontal.expectedType.ArcHalfCircle);
      }
   }
}
