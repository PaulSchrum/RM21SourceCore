using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ptsCogo
{
   [Serializable]
   public class CogoStation
   {
      public GenericAlignment myAlignment { get; internal set; }
      public bool isOnMyAlignemnt { get; internal set; }
      public double station { get; internal set; }
      public int region {get; internal set;}
      public double extended { get; internal set; }
      public double trueStation { get; internal set; }

      private static int plusOffset_ = 2;
      public static int plusOffset { get { return plusOffset_; } set { plusOffset = value > 0 ? value : 2; } }

      public CogoStation() 
      {
         myAlignment = null; station = 0; region = 1; isOnMyAlignemnt = true;
      }

      public CogoStation(double aStationDbl) : this()
      {
         station = aStationDbl; trueStation = aStationDbl;
      }

      internal CogoStation(GenericAlignment anAlignment) { myAlignment = anAlignment; }
      public CogoStation(CogoStation other)
      {
         myAlignment = other.myAlignment;
         isOnMyAlignemnt = other.isOnMyAlignemnt;
         station = other.station;
         region = other.region;
         extended = other.extended;
         trueStation = other.trueStation;
         
      }

      internal void setFromTrueStation(double newTrueStationValue)
      {
         if (null == myAlignment)
         {
            station = newTrueStationValue;
            trueStation = newTrueStationValue;
         }
         else
         {
            myAlignment.setStation(this, newTrueStationValue);
         }
      }

      public static CogoStation operator +(CogoStation sta, double addend)
      {
         if (null == sta.myAlignment)
         {
            return new CogoStation(sta.trueStation + addend);
         }
         else
         {
            CogoStation newStation = sta.myAlignment.newStation();
            newStation.setFromTrueStation(sta.trueStation + addend);
            return newStation;
         }
      }

      public static CogoStation operator -(CogoStation sta, double addend)
      {
         return sta + (-1 * addend);
      }

      public static double operator -(CogoStation sta1, CogoStation sta2)
      {
         // to do later, throw exception if they are on different alignments
         return sta1.trueStation - sta2.trueStation;
      }

      public override string ToString()
      {
         int leftOfPlus = (int) station / (int) (Math.Pow(10.0, plusOffset));
         double leftOfPlusDbl = leftOfPlus * Math.Pow(10.0, plusOffset);
         double rightOfPlusDbl = station - leftOfPlusDbl;
         StringBuilder retString = new StringBuilder();
         retString.AppendFormat("{0}+{1:00.00} R {2} ext. {3:f2}", leftOfPlus, rightOfPlusDbl, region, extended);
         return retString.ToString();
      }

      /// Desired features not yet implemented
      /// public bool canUseExtendedStatioing {get;set;}
      /// public bool isAtBeginStation {get;internal set;}
      /// public bool isAtEndStation {get;internal set;}
      /// public bool lastOperationWentPastBeginStation {get;internal set;} (may be true after subtract)
      /// public bool lastOperationWentPastEndStation {get;internal set;} (may be true after addition)
      /// public bool suppressBoundaryChecking {get;set;} -- used to increase performance

   }
}
