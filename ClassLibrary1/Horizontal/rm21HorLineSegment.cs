using ptsCogo.coordinates.CurvilinearCoordinates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ptsCogo.Angle;

namespace ptsCogo.Horizontal
{
   public class rm21HorLineSegment : HorizontalAlignmentBase
   {
      public rm21HorLineSegment(ptsPoint begPt, ptsPoint endPt)
         : base(begPt, endPt)
      {
         //this.BeginBearing = do this later
         //this.EndBearing = do this later
         this.BeginDegreeOfCurve = 0.0;
         this.EndDegreeOfCurve = 0.0;
      }

      public override Azimuth BeginAzimuth 
      {
         get
         {
            return new Azimuth(this.BeginPoint, this.EndPoint);
         }
      }
      public override Azimuth EndAzimuth
      {
         get { return this.BeginAzimuth; }
      }

      public override Double Length
      {
         get
         {
            return BeginPoint.GetHorizontalDistanceTo(EndPoint);
         }
         protected set { }
      }

      public override StringBuilder createTestSetupOfFundamentalGeometry()
      {
         StringBuilder returnSB = new StringBuilder();

         return returnSB;
      }

      public override List<StationOffsetElevation> getStationOffsetElevation(ptsPoint interestPoint)
      {
         ptsVector BeginToInterestPtVector = new ptsVector(this.BeginPoint, interestPoint);
         Deflection BeginToInterestDeflection = new Deflection(this.BeginAzimuth, BeginToInterestPtVector.Azimuth, true);
         if (Math.Abs(BeginToInterestDeflection.getAsDegrees()) > 90.0)
            return null;

         ptsVector EndToInterestPtVector = new ptsVector(this.EndPoint, interestPoint);
         Deflection EndToInterestDeflection = new Deflection(this.EndAzimuth, EndToInterestPtVector.Azimuth, true);
         if (Math.Abs(EndToInterestDeflection.getAsDegrees()) < 90.0)
            return null;

         Double length = BeginToInterestPtVector.Length * Math.Cos(BeginToInterestDeflection.getAsRadians());
         Double theStation = this.BeginStation + length;

         Double offset = BeginToInterestPtVector.Length * Math.Sin(BeginToInterestDeflection.getAsRadians());

         var soe = new StationOffsetElevation(this.BeginStation + length, offset, 0.0);
         var returnList = new List<StationOffsetElevation>();
         returnList.Add(soe);
         return returnList;
      }

      public override ptsPoint getXYZcoordinates(StationOffsetElevation anSOE)
      {
         Double piOver2 = Math.PI / 2.0;
         ptsVector alongVector = new ptsVector(this.BeginAzimuth, anSOE.station - this.BeginStation);
         ptsPoint returnPoint = this.BeginPoint + alongVector;
         returnPoint.z = anSOE.elevation.EL;

         ptsVector perpandicularVector = new ptsVector();
         Azimuth perpandicularAzimuth = this.BeginAzimuth + piOver2;
         if (anSOE.offset.OFST < 0.0)
         {
            perpandicularAzimuth = this.BeginAzimuth - piOver2;
         }
         returnPoint = returnPoint + new ptsVector(perpandicularAzimuth, anSOE.offset.OFST);

         return returnPoint;
      }
   }
}
