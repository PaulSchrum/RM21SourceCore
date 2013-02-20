using ptsCogo;
using ptsCogo.Angle;
using ptsCogo.coordinates.CurvilinearCoordinates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using ptsCogo.coordinates;

namespace rm21Core.Ribbons
{
   public class RaySheet : RaySheetBase
   {
      public RaySheet (CogoStation beginStation, CogoStation endStation, Slope initialSlope)
      {
         interpretWidths = new Profile(beginStation, endStation, (double) enmWidthInterpret.HorizontalOnly);
         Widths = null;
         interpretCrossSlopes = new Profile(beginStation, endStation, (double)enmCrossSlopeInterpret.xPercentage);
         CrossSlopes = new Profile(beginStation, endStation, initialSlope);
         LiederLineHeight = 5.0;
      }

      public override void DrawPlanViewSchematic(IRM21cad2dDrawingContext cadContext,
               int whichSide)
      {

      }

      public override double? getActualWidth(CogoStation aStation, out tupleNullableDoubles result)
      {
         double? ret = null;
         result.ahead = null;
         result.back = null;
         result.isSingleValue = true;
         return ret;
      }

      public override void moveToOuterEdge(ref StationOffsetElevation aSOE, int whichSide)
      {
         //double traversedWidth;
         double? theWidth;
         tupleNullableDoubles result;
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

         ptsRay thisAsRay = new ptsRay(); 
         thisAsRay.StartPoint = new ptsPoint(aSOE.offset, 0.0, aSOE.elevation);
         thisAsRay.Slope = (double)theCrossSlope;
         thisAsRay.advanceDirection = whichSide;
         Profile rayAsProfile = this.MyParentPGLgrouping.ParentCorridor.existingGroundProfile.getIntersections(thisAsRay).FirstOrDefault<Profile>();

         theWidth = Math.Abs(rayAsProfile.EndProfTrueStation - rayAsProfile.BeginProfTrueStation);
         if (theWidth == null) theWidth = 0.0;

         aSOE.offset += theWidth * whichSide;

         if (theCrossSlope == null) theCrossSlope = 0.0;
         aSOE.elevation += theCrossSlope * theWidth;
      }

      public override void DrawCrossSection(IRM21cad2dDrawingContext cadContext,
         ref StationOffsetElevation aSOE, int whichSide)
      {
         double LLH = LiederLineHeight;
         double ribbonWidth;
         double X1 = aSOE.offset;
         double Y1 = aSOE.elevation;
         this.moveToOuterEdge(ref aSOE, whichSide);
         if (X1 == aSOE.offset && Y1 == aSOE.elevation) return;

         cadContext.Draw(X1, Y1, aSOE.offset, aSOE.elevation);
         return;
      }
         /*
         setupCrossSectionDrawing(cadContext);
         ribbonWidth = Math.Abs(aSOE.offset - X1);
         cadContext.setElementWeight(0.8);
         cadContext.setElementColor(Color.FromArgb(124, 255, 255, 255));
         cadContext.Draw(X1, LLH + 0.5, aSOE.offset, LLH + 0.5);
         cadContext.Draw(aSOE.offset, 0.5, aSOE.offset, LLH + 1.5);
         string widthStr = (Math.Round(ribbonWidth * 10) / 10).ToString();
         cadContext.Draw(widthStr, X1 + whichSide * ribbonWidth / 2, LLH + 0.5, 0.0);

         if (false == SuppressSlopeText)
         {
            Slope mySlope = new Slope((aSOE.elevation - Y1) / (aSOE.offset - X1));
            if (whichSide > 0)
               cadContext.Draw(mySlope.ToString(),
                  (X1 + aSOE.offset) / 2,
                  (Y1 + aSOE.elevation) / 2,
                  mySlope.getAsDegrees());
            else
               cadContext.Draw(mySlope.FlipDirection().ToString(),
                  (X1 + aSOE.offset) / 2,
                  (Y1 + aSOE.elevation) / 2,
                  mySlope.getAsDegrees());
         }
         SuppressSlopeText = false;
      }

      /* * /
      public override double? getActualWidth(CogoStation aStation, out tupleNullableDoubles result)
      {

      }
      /* */

      /* * /
      public override void moveToOuterEdge(ref StationOffsetElevation aSOE, int whichSide)
      {
         //double traversedWidth;
         tupleNullableDoubles result;
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

         double? theWidth;
         List<double> allRayProfileIntersections;
         List<Profile> a = this.MyParentPGLgrouping.ParentCorridor.TargetSurfaceXSProfiles;
         foreach (var xsPfl in a)
         {
            //allIntersections = xsPfl.intersectRay(thisOffset, thisSlope, myProgressionDirection.Sign());
            if (allRayProfileIntersections != null)
            {
               theWidth = allRayProfileIntersections.FirstOrDefault();
               break;
            }
         }

         aSOE.offset += theWidth * whichSide;

         if (theCrossSlope == null) theCrossSlope = 0.0;
         aSOE.elevation += theCrossSlope * theWidth;
      }   /* */
   }
}
