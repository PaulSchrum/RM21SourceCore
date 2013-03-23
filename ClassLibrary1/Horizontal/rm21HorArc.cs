using ptsCogo.Angle;
using ptsCogo.coordinates.CurvilinearCoordinates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ptsCogo.Horizontal
{
   public class rm21HorArc : HorizontalAlignmentBase
   {
      private int deflDirection { get; set; }
      public ptsPoint ArcCenterPt {get; protected set;}
      public Double Radius { get; protected set; }
      public ptsVector BeginRadiusVector { get; protected set; }
      public ptsVector EndRadiusVector { get; protected set; }

      public rm21HorArc(ptsPoint begPt, ptsPoint centerPt, ptsPoint endPt, expectedType ExpectedType,
         int deflectionDirection)
         : base(begPt, endPt)
      {
         this.deflDirection = deflectionDirection;
         this.ArcCenterPt = centerPt;
         this.BeginRadiusVector = this.ArcCenterPt - this.BeginPoint;
         this.EndRadiusVector = this.ArcCenterPt - this.EndPoint;

         this.Radius = this.BeginRadiusVector.Length;
         Double validationRadius = this.EndRadiusVector.Length;
         if (Math.Abs(this.Radius - validationRadius) > 0.00014)
            throw new Exception("Given points do not represent a circle.");

         Double degreesToAdd;
         if ((ExpectedType == expectedType.ArcSegmentInsideSolution) ||
             (ExpectedType == expectedType.ArcHalfCircle && deflectionDirection > 0))
         {
            degreesToAdd = 90;
         }
         else
            degreesToAdd = -90;
         
         this.BeginAzimuth = this.BeginRadiusVector.Azimuth + ptsAngle.radiansFromDegree(degreesToAdd);
         this.EndAzimuth = this.EndRadiusVector.Azimuth + ptsAngle.radiansFromDegree(degreesToAdd);

         // applies to English projects only (for now)
         this.BeginDegreeOfCurve = HorizontalAlignmentBase.computeDegreeOfCurve(this.Radius);
         this.EndDegreeOfCurve = this.BeginDegreeOfCurve;

         if (ExpectedType == expectedType.ArcSegmentOutsideSoluion)
         {
            computeDeflectionForOutsideSolutionCurve();
            this.Length = 100.0 * this.Deflection.getAsRadians() / this.BeginDegreeOfCurve.getAsRadians();
            this.Length = Math.Abs(this.Length);
         }
         else
         {
            Deflection = new Deflection(this.BeginAzimuth, this.EndAzimuth, true);
            this.Length = 100.0 * this.Deflection.getAsRadians() / this.BeginDegreeOfCurve.getAsRadians();
            this.Length = Math.Abs(this.Length);
         }
      }

      private void computeDeflectionForOutsideSolutionCurve()
      {
         Double radVector1Az = this.BeginRadiusVector.Azimuth.getAsDegrees();
         Double radVector2Az = this.EndRadiusVector.Azimuth.getAsDegrees();

         int quadrantAZ1 = Azimuth.getQuadrant(radVector1Az);
         int quadrantAZ2 = Azimuth.getQuadrant(radVector2Az);

         Double defl = radVector2Az - radVector1Az;

         if (1 == this.deflDirection)
         {
            if (defl < 0.0) defl += 360.0;
         }
         else
         {
            defl = defl - 360.0;
         }

         Deflection = Deflection.ctorDeflectionFromAngle(defl);

      }

      public override StringBuilder createTestSetupOfFundamentalGeometry()
      {
         throw new NotImplementedException();
      }

      public override List<StationOffsetElevation> getStationOffsetElevation(ptsPoint interestPoint)
      {
         ptsVector arcCenterToInterestPtVector = new ptsVector(this.ArcCenterPt, interestPoint);
         Deflection deflToInterestPt = new Deflection(this.BeginRadiusVector.Azimuth, arcCenterToInterestPtVector.Azimuth, true);
         int arcDeflDirection = Math.Sign(this.Deflection.getAsDegrees());
         if (arcDeflDirection * deflToInterestPt.getAsDegrees() < 0.0)
         {
            return null;
         }
         else if (Math.Abs(this.Deflection.getAsDegrees()) - Math.Abs(deflToInterestPt.getAsDegrees()) < 0.0)
         {
            return null;
         }

         Double interestLength = this.Length * deflToInterestPt.getAsRadians() / this.Deflection.getAsRadians();
         Offset offset =  new Offset(arcDeflDirection * (this.Radius - arcCenterToInterestPtVector.Length));

         var soe = new StationOffsetElevation(this.BeginStation + interestLength, offset, 0.0);
         var returnList = new List<StationOffsetElevation>();
         returnList.Add(soe);
         return returnList;
      }

   }
}
