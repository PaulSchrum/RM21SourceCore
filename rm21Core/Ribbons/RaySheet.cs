using ptsCogo;
using ptsCogo.Angle;
using ptsCogo.coordinates.CurvilinearCoordinates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
