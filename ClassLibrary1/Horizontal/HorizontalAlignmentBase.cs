using ptsCogo.Angle;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ptsCogo.Horizontal
{
   public  class HorizontalAlignmentBase : GenericAlignment
   {
      public HorizontalAlignmentBase() : base() { }

      public HorizontalAlignmentBase(ptsPoint begPt, ptsPoint endPt)
         : base()
      {
         BeginPoint = begPt;
         EndPoint = endPt;

      }

      public HorizontalAlignmentBase(List<Double> stationEquationingList) : base(stationEquationingList)
      {

      }

      public virtual ptsPoint BeginPoint { get; protected set; }
      public virtual ptsPoint EndPoint { get; protected set; }

      public virtual Azimuth BeginAzimuth { get; protected set; }
      public virtual Azimuth EndAzimuth { get; protected set; }

      public virtual ptsAngle BeginDegreeOfCurve { get; protected set; }
      public virtual ptsAngle EndDegreeOfCurve { get; protected set; }

      public virtual Deflection Deflection { get; protected set; }
      public virtual Double Length { get; protected set; }

      static HorizontalAlignmentBase(){degreeOfCurveLength = 100;}
      static public Double degreeOfCurveLength { get; set; }
      static public ptsAngle computeDegreeOfCurve(Double radius)
      {
         return new ptsAngle(radius, degreeOfCurveLength);
      }

      public virtual StringBuilder createTestSetupOfFundamentalGeometry() { return null; }

      /* * /
      public bool isUnitUSsurveyFoot { get { return thisUnit == Unit.SurveyFoot; } set { thisUnit = Unit.SurveyFoot; } }
      public bool isUnitFoot { get; set; }
      public bool isUnitMeter { get; set; }

      private enum Unit{Meter, Foot, SurveyFoot}
      private HorizontalAlignmentBase.Unit thisUnit { get; set; }  /*  */
   }
}
