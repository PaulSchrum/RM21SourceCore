using ptsCogo.Angle;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ptsCogo.Horizontal
{
   public class rm21HorArc : HorizontalAlignmentBase
   {
      public ptsPoint ArcCenterPt {get; protected set;}
      public Double Radius { get; protected set; }
      public ptsVector BeginRadiusVector { get; protected set; }
      public ptsVector EndRadiusVector { get; protected set; }

      public rm21HorArc(ptsPoint begPt, ptsPoint centerPt, ptsPoint endPt, expectedType ExpectedType)
         : base(begPt, endPt)
      {
         this.ArcCenterPt = centerPt;
         this.BeginRadiusVector = this.ArcCenterPt - this.BeginPoint;
         this.EndRadiusVector = this.ArcCenterPt - this.EndPoint;

         this.Radius = this.BeginRadiusVector.Length;
         Double validationRadius = this.EndRadiusVector.Length;
         if (Math.Abs(this.Radius - validationRadius) > 0.00004)
            throw new Exception("Given points do not represent a circle.");

         Double degreesToAdd;
         if ((ExpectedType == expectedType.ArcSegmentInsideSolution) ||
             (ExpectedType == expectedType.ArcHalfCircleDeflectingRight))
         {
            degreesToAdd = 90;
         }
         else
            degreesToAdd = -90;
         
         this.BeginAzimuth = this.BeginRadiusVector.Azimuth + ptsAngle.radiansFromDegree(degreesToAdd);
         this.EndAzimuth = this.EndRadiusVector.Azimuth + ptsAngle.radiansFromDegree(degreesToAdd);

         Deflection = new Deflection(this.EndAzimuth - this.BeginAzimuth);
         if (ExpectedType == expectedType.ArcSegmentOutsideSoluion)
            Deflection = new Deflection((Math.PI * 2.0) - Deflection.getAsRadians());
         
         // applies to English projects only (for now)
         this.BeginDegreeOfCurve = HorizontalAlignmentBase.computeDegreeOfCurve(this.Radius);
         this.EndDegreeOfCurve = this.BeginDegreeOfCurve;

         this.Length = 100.0 * this.Deflection.getAsRadians_NormalizedTo360() / this.BeginDegreeOfCurve.getAsRadians();
      }

   }
}
