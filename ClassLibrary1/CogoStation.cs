using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows.Data;

namespace ptsCogo
{
   [Serializable]
   public class CogoStation : INotifyPropertyChanged, IComparable<CogoStation>
   {
      public GenericAlignment myAlignment { get; internal set; }
      public bool isOnMyAlignemnt { get; internal set; }
      internal double station { get; set; }
      internal int region {get; set;}
      public double extended { get; internal set; }

      private double _trueStation;
      public double trueStation { get { return _trueStation; }
         set
         {
            if (_trueStation == value)
               return;

            _trueStation = value;

            if (null == myAlignment)
            {
               trueStation = _trueStation;
            }
            else
            {
               myAlignment.setStation(this, _trueStation);
            }
            NotifyPropertyChanged("trueStation");
         }
      }

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

      public event PropertyChangedEventHandler PropertyChanged;
      private void NotifyPropertyChanged(String info)
      {
         if (PropertyChanged != null)
         {
            PropertyChanged(this, new PropertyChangedEventArgs(info));
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
            newStation.trueStation = sta.trueStation + addend;
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

      public static bool operator >(CogoStation station, double aDouble)
      {
         return station.trueStation > aDouble;
      }

      public static bool operator >(CogoStation station, CogoStation otherSta)
      {
         return station.trueStation > otherSta.trueStation;
      }

      public static bool operator <(CogoStation station, CogoStation otherSta)
      {
         return station.trueStation < otherSta.trueStation;
      }

      public static bool operator <(CogoStation station, double aDouble)
      {
         return station.trueStation < aDouble;
      }

      public static explicit operator CogoStation(double adbl)
      {
         return new CogoStation(adbl);
      }

      //public static implicit operator CogoStation(double adbl)
      //{
        // return new CogoStation(adbl);
      //}

      public int CompareTo(CogoStation other)
      {
         if (this.trueStation == other.trueStation)
         {
            return 0;
         }
         else if (this.trueStation < other.trueStation)
            return -1;
         else 
            return 1;
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

      /// <summary>
      /// Warning: Rapid Prototype version of this function only.  Is fragile.
      /// Specifically it does not check for 'R' in the case of a region
      /// To Do: Correct this shortcoming
      /// </summary>
      /// <param name="strValue"></param>
      /// <returns></returns>
      public static CogoStation convertStringToStation(string strValue)
      {
         string newStr = strValue;
         int pos = newStr.IndexOf('+');
         if (pos > -1)
            newStr = newStr.Remove(pos, 1);
         
         double dblValue;
         Double.TryParse(newStr, out dblValue);

         return new CogoStation(dblValue);

      }

      /// Desired features not yet implemented
      /// public bool canUseExtendedStatioing {get;set;}
      /// public bool isAtBeginStation {get;internal set;}
      /// public bool isAtEndStation {get;internal set;}
      /// public bool lastOperationWentPastBeginStation {get;internal set;} (may be true after subtract)
      /// public bool lastOperationWentPastEndStation {get;internal set;} (may be true after addition)
      /// public bool suppressBoundaryChecking {get;set;} -- used to increase performance
      /// 
   }
}
