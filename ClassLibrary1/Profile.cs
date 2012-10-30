﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using ptsCogo;
using System.ComponentModel;

namespace ptsCogo
{
   public class Profile //: GenericAlignment
   {
      private List<verticalCurve> allVCs;
      private int vcIndex=0;
      private double stationEqualityTolerance = 0.00005;
      private bool iHaveOneOrMoreVerticalCurves { get; set; }

      public double BeginProfTrueStation {get; private set;}
      public double EndProfTrueStation { get; private set; }

      private vpiList thisAsVpiList_;  // To Do: Make sure all modifications to the data
      // get reflected in thisAsVpiList.
      public vpiList VpiList
      {
         get { return to_vpiList(); }
         set { buildThisFromRawVPIlist(value); }
      }

      private Profile() { }

      //public Profile(CogoStation beginStation, CogoStation endStation, int singleElevation)
      //   : this(beginStation, endStation, (double)singleElevation)
      //{ }

      public Profile(CogoStation beginStation, CogoStation endStation, double singleElevation)
      {
         vpiList aVpiList = new vpiList();
         aVpiList.add(beginStation, singleElevation);
         aVpiList.add(endStation, singleElevation);

         buildThisFromRawVPIlist(aVpiList);
      }

      public Profile(vpiList rawVPIlist)
      {
         buildThisFromRawVPIlist(rawVPIlist);
      }

      private void buildThisFromRawVPIlist(vpiList rawVPIlist)
      {
         iHaveOneOrMoreVerticalCurves = false;
         if (rawVPIlist.Count < 2)
         {
            throw new NotImplementedException("Profile can not have less than 2 VPIs");
         }
         else if (rawVPIlist.Count == 2)
         {
            thisAsVpiList_ = rawVPIlist;

            rawVPI vpi1 = rawVPIlist.getVPIbyIndex(0);
            rawVPI vpi2 = rawVPIlist.getVPIbyIndex(1);

            verticalCurve aNewVerticalCurve = new verticalCurve();
            aNewVerticalCurve.BeginElevation = vpi1.Elevation;

            aNewVerticalCurve.BeginStation = vpi1.Station;
            BeginProfTrueStation = vpi1.Station.trueStation;
            
            aNewVerticalCurve.Length = vpi2.Station - vpi1.Station;
            EndProfTrueStation = vpi2.Station.trueStation;

            aNewVerticalCurve.BeginSlope = (vpi2.Elevation - vpi1.Elevation) /
                                             aNewVerticalCurve.Length;
            aNewVerticalCurve.IsTangent = true;
            aNewVerticalCurve.IsBeginPINC = false;
            aNewVerticalCurve.IsEndPINC = false;

            allVCs = new List<verticalCurve>();
            allVCs.Add(aNewVerticalCurve);
         }
         else
         {
            thisAsVpiList_ = rawVPIlist;

            double g1; double g2;
            Int64 count=0;
            rawVPI vpi1; rawVPI vpi2;
            verticalCurve newVC;

            // Note: These next two lines are here to suppress compiler errors.
            //   The real assignments for vpi1 and 2 are at the end of the foureach loop
            vpi1 = rawVPIlist.getVPIbyIndex(0);
            vpi2 = rawVPIlist.getVPIbyIndex(1);
            
            foreach (rawVPI vpi3 in rawVPIlist.getVPIlist())
            {
               count++;
               if (count > 1)
               {
                  if (count > 2)
                  {
                     if (count == 3)
                     {
                        allVCs = new List<verticalCurve>();
                        BeginProfTrueStation = vpi1.Station.trueStation;
                     }

                     g1 = (vpi2.Elevation - vpi1.Elevation) /
                          (vpi2.Station.trueStation - vpi1.Station.trueStation);

                     g2 = (vpi3.Elevation - vpi2.Elevation) /
                          (vpi3.Station.trueStation - vpi2.Station.trueStation);

                     double incomingTanLen;
                     incomingTanLen = vpi2.getBeginStation() - vpi1.getEndStation();

                     // add a VC for the incoming tangent when necessary
                     if (incomingTanLen > 0.0)
                     {
                        newVC = new verticalCurve();
                        newVC.BeginSlope = g1;
                        newVC.BeginStation = vpi1.getEndStation();
                        newVC.EndSlope = g1;
                        newVC.Length = incomingTanLen;
                        newVC.BeginElevation = vpi2.Elevation + getELchangeAlongSlope(g1, 
                           (vpi1.getEndStation() - vpi2.Station));
                        
                        newVC.IsBeginPINC = false;
                        if (allVCs.Count > 0)
                        {
                           newVC.IsBeginPINC = allVCs.Last<verticalCurve>().IsEndPINC;
                        }

                        newVC.IsEndPINC = false;
                        if (utilFunctions.tolerantCompare(vpi2.Length, 0.0, stationEqualityTolerance) == 0)
                        {
                           newVC.IsEndPINC = true;
                        }
                        
                        allVCs.Add(newVC);
                     }
                     // End: add a VC for the incoming tangent when necessary

                     // add a VC for the current vertical curve if VClen > 0
                     if (vpi2.Length > 0.0)
                     {
                        iHaveOneOrMoreVerticalCurves = true;
                        newVC = new verticalCurve();
                        newVC.BeginSlope = g1;
                        newVC.BeginStation = vpi2.getBeginStation();
                        newVC.EndSlope = g2;
                        newVC.Length = vpi2.Length;
                        newVC.BeginElevation = vpi2.Elevation - getELchangeAlongSlope(g1, newVC.Length / 2.0);
                        allVCs.Add(newVC);
                        EndProfTrueStation = newVC.BeginStation.trueStation + newVC.Length;
                     }
                     // End: add a VC for the current vertical curve if VClen > 0

                     // if this is the final VPI, add a final tangent if necessary
                     if (count == rawVPIlist.Count)
                     {
                        double outgoingTangentLength = vpi3.getBeginStation() - vpi2.getEndStation();
                        if (outgoingTangentLength > 0.0)
                        {
                           newVC = new verticalCurve();
                           newVC.BeginSlope = g2;
                           newVC.BeginStation = vpi2.getEndStation();
                           newVC.EndSlope = g2;
                           newVC.Length = outgoingTangentLength;
                           newVC.BeginElevation = vpi2.Elevation + getELchangeAlongSlope(g2, vpi2.Length / 2.0);
                           
                           newVC.IsBeginPINC = false;
                           if (allVCs.Count > 0)
                           {
                              newVC.IsBeginPINC = allVCs.Last<verticalCurve>().IsEndPINC;
                           }

                           newVC.IsEndPINC = false;

                           allVCs.Add(newVC);
                           EndProfTrueStation = newVC.BeginStation.trueStation + newVC.Length;
                        }
                     }
                     // End: if this is the final VPI, add a final tangent if necessary
                  }
                  vpi1 = vpi2;
               }
               vpi2 = vpi3;
            }
         }
      }

      public void setFromVPIlist(vpiList newVPIlist)
      {
         buildThisFromRawVPIlist(newVPIlist);
      }

      /// <summary>
      /// Warning: currently does not handle profiles with vertical curves
      /// we must implement that feature some day, but not today (9-29-2012)
      /// </summary>
      /// <returns></returns>
      public vpiList to_vpiList()
      {
         if (true == iHaveOneOrMoreVerticalCurves)
            throw new NotImplementedException("Profiles with vertical curves not supported yet.  Only profiles with VPI-NCs.");

         if (allVCs.Count < 1)
            return null;

         int count=allVCs.Count-1;
         vpiList returnList = new vpiList();
         foreach (var profSeg in allVCs)
         {
            count--;
            if (profSeg.Length > 0.0)
            {
               returnList.add(new rawVPI(profSeg.BeginStation, profSeg.BeginElevation));
               if (count == -1)
               {
                  returnList.add(new rawVPI(profSeg.EndStation, profSeg.EndElevation));
               }
            }
         }

         thisAsVpiList_ = returnList;

         return returnList;
      }

      public void addStationAndElevation(CogoStation newStation, double newElevation)
      {
         verticalCurve newVC, otherVC;
         if (iHaveOneOrMoreVerticalCurves == true)
         {
            throw new NotImplementedException("Currently unable to add VPI to a profile with a vertical curve.");
         }
         newVC = new verticalCurve();
         // To Do's
         //Insert new pi after last station
         if (newStation > EndProfTrueStation)
         {
            vcIndex = allVCs.Count - 1;
            otherVC = allVCs[vcIndex];
            newVC.BeginStation = otherVC.EndStation;
            EndProfTrueStation = newStation.trueStation;
            otherVC.IsEndPINC = true;
            newVC.BeginElevation = verticalCurve.getElevation(otherVC, (CogoStation)otherVC.EndStation);
            newVC.IsBeginPINC = true;
            newVC.IsEndPINC = false;
            newVC.IsTangent = true;
            newVC.BeginSlope = (newElevation - newVC.BeginElevation) /
                  (newStation - newVC.BeginStation);
            newVC.EndSlope = newVC.BeginSlope;
            newVC.Length = newStation - newVC.BeginStation;
            allVCs.Add(newVC);
         }
         else if (newStation < BeginProfTrueStation)  //Insert new pi before first station
         {
            vcIndex = 0;
            otherVC = allVCs[vcIndex];
            newVC.BeginStation = newStation;
            BeginProfTrueStation = newStation.trueStation;
            otherVC.IsBeginPINC = true;
            newVC.BeginElevation = newElevation;
            newVC.IsBeginPINC = false;
            newVC.IsEndPINC = true;
            newVC.IsTangent = true;
            newVC.BeginSlope = (otherVC.BeginElevation - newElevation) /
                  (otherVC.BeginStation - newStation);
            newVC.EndSlope = newVC.BeginSlope;
            newVC.Length = otherVC.BeginStation - newStation;
            allVCs.Insert(0, newVC);
         }
         //insert new pi interior to the profile, but one that has no vertical curves
         else
         {  //(CogoStation newStation, double newElevation)
            setIndexToTheCorrectVC(newStation);
            otherVC = allVCs[vcIndex];
            
            // see if new station is already in the profile as a vpi
            if(newStation == otherVC.BeginStation)
            {
               if (vcIndex == 0)  // currently at the first vc
               {
                  throw new NotImplementedException();
               }
               else
               {
                  throw new NotImplementedException();
               }
            }
            else if(newStation == otherVC.EndStation)
            {
               if (vcIndex == allVCs.Count - 1)  // currently at the last vc
               {
                  throw new NotImplementedException();
               }
               else
               {
                  throw new NotImplementedException();
               }
            }  // End: see if new station is already in the profile as a vpi
            else // new station is interior to an existing VC
            {
               CogoStation station1, station2, station3;
               double elevation1, elevation2, elevation3;
               station1 = otherVC.BeginStation; station2 = newStation; station3 = otherVC.EndStation;
               elevation1 = otherVC.BeginElevation; elevation2 = newElevation;
               elevation3 = verticalCurve.getElevation(otherVC, station3);

               otherVC.setVerticalTangent(station1, elevation1, station2, elevation2);
               newVC = new verticalCurve();
               newVC.setVerticalTangent(station2, elevation2, station3, elevation3);

               verticalCurve otherOtherVC;
               if (vcIndex > 0)
               {
                  int VCindex0 = vcIndex - 1;
                  otherOtherVC = allVCs[VCindex0];
                  if (otherOtherVC.EndSlope == otherVC.BeginSlope)
                     otherOtherVC.IsEndPINC = otherVC.IsBeginPINC = false;
                  else
                     otherOtherVC.IsEndPINC = otherVC.IsBeginPINC = true;
               }

               if (vcIndex < allVCs.Count-1)
               {
                  int VCindex3 = vcIndex + 1;
                  otherOtherVC = allVCs[VCindex3];
                  if (otherOtherVC.BeginSlope == otherVC.EndSlope)
                     otherOtherVC.IsBeginPINC = otherVC.IsEndPINC = false;
                  else
                     otherOtherVC.IsBeginPINC = otherVC.IsEndPINC = true;
               }

               allVCs.Insert(vcIndex+1, newVC);
            } // End: new station is interior to an existing VC
         }
         // To Do: dissolve two vc's into one when they are really the same slope/elevation
      }

      public static double getSlopeFromDoubles(double y1, double y2, double x1, double x2)
      {
         return (y2 - y1) / (x2 - x1);
      }

      static public double getELchangeAlongSlope(double grade, double distance)
      {
         return distance * grade;
      }

      private void setIndexToTheCorrectVC(CogoStation aStation)
      {
         while (aStation.trueStation > allVCs[vcIndex].EndStation.trueStation)
         {
            vcIndex++;
            if (vcIndex > allVCs.Count-1)
            {
               vcIndex = allVCs.Count-1;
               throw new IndexOutOfRangeException();
            }
         }
         while (aStation.trueStation < allVCs[vcIndex].BeginStation.trueStation)
         {
            vcIndex--;
            if (vcIndex < 0)
            {
               vcIndex = 0;
               throw new IndexOutOfRangeException();
            }
         }
      }

      public double? getElevation(CogoStation station)
      {
         var resultTND = new tupleNullableDoubles();
         getElevation(station, out resultTND);
         return resultTND.back;
      }

      public void getElevation(CogoStation station, out tupleNullableDoubles theElevation)
      {
         verticalCurve.getSwitchForProfiles callFunction = new verticalCurve.getSwitchForProfiles(verticalCurve.getElevation);
         getValueByDelegate(station, out theElevation, callFunction);
      }

      public void getSlope(CogoStation station, out tupleNullableDoubles theSlope)
      {
         verticalCurve.getSwitchForProfiles callFunction = new verticalCurve.getSwitchForProfiles(verticalCurve.getSlope);
         getValueByDelegate(station, out theSlope, callFunction);
      }

      public void getKvalue(CogoStation station, out tupleNullableDoubles theSlope)
      {
         verticalCurve.getSwitchForProfiles callFunction = new verticalCurve.getSwitchForProfiles(verticalCurve.getKvalue);
         getValueByDelegate(station, out theSlope, callFunction);
      }

      private void getValueByDelegate(CogoStation station, out tupleNullableDoubles theOutValue, verticalCurve.getSwitchForProfiles getFunction)
      {
         if ((station.trueStation < BeginProfTrueStation - stationEqualityTolerance) ||
             (station.trueStation > EndProfTrueStation + stationEqualityTolerance))
         {  // it means we are off the profile
            theOutValue.back = null;
            theOutValue.ahead = null;
            theOutValue.isSingleValue = true;
            return;
         }

         try { setIndexToTheCorrectVC(station); }
         catch (IndexOutOfRangeException) { }
         verticalCurve aVC = allVCs[vcIndex];

         // if we are at the begin station, check to see how we relate to the previous vc
         if (utilFunctions.tolerantCompare(station.trueStation, aVC.BeginStation.trueStation, stationEqualityTolerance) == 0)
         {
            // if we are at the beginning of the profile, split theOutValue
            if (vcIndex == 0)
            {
               theOutValue.back = null;
               theOutValue.ahead = getFunction(aVC, station);
               theOutValue.isSingleValue = false;
            }
            else  // if station is on the boundary between two verticalCurves,
            {     // then see if we need to split theOutValue
               if (getFunction == verticalCurve.getKvalue &&
                   aVC.IsBeginPINC)
               {
                  theOutValue.back = theOutValue.ahead = 0.0;
                  theOutValue.isSingleValue = true;
               }
               else
               {
                  theOutValue.ahead = getFunction(aVC, station);
                  theOutValue.back = getFunction(allVCs[vcIndex - 1], station);
                  if (utilFunctions.tolerantCompare(theOutValue.back, theOutValue.ahead, 0.00005) == 0)
                  {
                     theOutValue.isSingleValue = true;
                  }
                  else theOutValue.isSingleValue = false;
               }
            }
         }
         // End: if we are at the begin station, check to see how we relate to the previous vc
         // if we are at the end station, check to see how we relate to the next vc
         else if (utilFunctions.tolerantCompare(station.trueStation, aVC.EndStation.trueStation, stationEqualityTolerance) == 0)
         {
            // if we are at the end of the profile, split theOutValue
            if (vcIndex == allVCs.Count - 1)
            {
               theOutValue.back = getFunction(aVC, station);
               theOutValue.ahead = null;
               theOutValue.isSingleValue = false;
            }
            else  // if station is on the boundary between two verticalCurves,
            {     // then see if we need to split theOutValue
               if (getFunction == verticalCurve.getKvalue &&
                   aVC.IsEndPINC)
               {
                  theOutValue.back = theOutValue.ahead = 0.0;
                  theOutValue.isSingleValue = true;
               }
               else
               {
                  theOutValue.back = getFunction(aVC, station);
                  theOutValue.ahead = getFunction(allVCs[vcIndex + 1], station);
                  if (utilFunctions.tolerantCompare(theOutValue.back, theOutValue.ahead, 0.00005) == 0)
                  {
                     theOutValue.isSingleValue = true;
                  }
                  else theOutValue.isSingleValue = false;
               }
            }
         }
         // End: if we are at the end station, check to see how we relate to the next vc
         else
         {
            theOutValue.back = getFunction(aVC, station);
            theOutValue.ahead = theOutValue.back;
            theOutValue.isSingleValue = true;
         }
      }

      public bool isOnPINC(CogoStation aStation)
      {
         var aVC = allVCs.FirstOrDefault(vc => utilFunctions.tolerantCompare(vc.BeginStation.trueStation, aStation.trueStation, stationEqualityTolerance) == 0);
         if (aVC == null)
            return false;
         else
            return aVC.IsBeginPINC;
      }

      public int SegmentCount
      {
         get
         {
            if (allVCs == null) return 0;
            return allVCs.Count;
         }
         private set { }
      }

      private class verticalCurve
      {
         private double length_;
         private CogoStation endStation_;
         private bool isTangent_;
         private double slopeRateOfChange_;

         public bool IsTangent 
         { 
            get { return isTangent_; } 
            set 
            {
               isTangent_ = value;
               if (isTangent_ == true)
               {
                  slopeRateOfChange_ = double.PositiveInfinity;
               }
            } 
         }

         public CogoStation BeginStation { get; set; }
         public CogoStation EndStation { get { return endStation_; } private set { } }
         public double BeginElevation { get; set; }
         public double BeginSlope { get; set; }
         public double EndSlope { get; set; }
         public double EndElevation { get { return getElevation(this, EndStation); } }
         public bool IsBeginPINC { get; set; }  // PINC = PI, No Curve.
         public bool IsEndPINC { get; set; }    //  used to detect undefined K values at PINC stations
         public double Kvalue { get { return 0.01 / slopeRateOfChange_; } private set { } }
         public double Length 
         { get { return length_; } 
            set
            { 
               length_ = value;
               if (length_ > 0.0)
               {
                  endStation_ = BeginStation + length_;
                  if (isTangent_ == false)
                  {
                     slopeRateOfChange_ = (EndSlope - BeginSlope) / length_;
                  }
                  else
                  {
                     slopeRateOfChange_ = double.PositiveInfinity;
                  }
               }
               else if (length_ < 0.0)
               {
                  throw new NotSupportedException("Length of vertical curve not allowed to be less than 0.");
               }
               else
                  endStation_ = BeginStation;
            } 
         }

         internal void setVerticalTangent(CogoStation sta1, double EL1, CogoStation sta2, double EL2)
         {
            BeginStation = sta1;
            BeginElevation = EL1;
            BeginSlope = getSlopeFromDoubles(EL1, EL2, sta1.trueStation, sta2.trueStation);
            IsTangent = true;
            EndSlope = BeginSlope;
            Length = sta2 - sta1;
         }

         internal delegate double getSwitchForProfiles(verticalCurve aVC, CogoStation station);

         static public double getElevation(verticalCurve aVC, CogoStation station)
         {
            double theElevation;
            double lenSquared; double lenIntoVC;

            lenIntoVC = station - aVC.BeginStation;
            lenSquared = lenIntoVC * lenIntoVC;

            theElevation = aVC.BeginElevation +
               (lenIntoVC * aVC.BeginSlope);

            if (aVC.IsTangent == false)
               theElevation += (lenSquared / (200.0 * aVC.Kvalue));

            return theElevation;
         }

         static public double getSlope(verticalCurve aVC, CogoStation station)
         {
            double theSlope;
            double lenSquared; double lenIntoVC;

            lenIntoVC = station - aVC.BeginStation;
            lenSquared = lenIntoVC * lenIntoVC;

            theSlope = aVC.BeginSlope +
               (lenIntoVC / (100.0 * aVC.Kvalue));

            return theSlope;
         }

         static public double getKvalue(verticalCurve aVC, CogoStation station)
         {
            return aVC.Kvalue;
         }
      }
   }

   public class vpiList : INotifyPropertyChanged
   {
      private ObservableCollection<rawVPI> theVPIs_;
      public ObservableCollection<rawVPI> theVPIs
      {
         get { return theVPIs_; }
         set
         {
            if (theVPIs_ != value)
            {
               theVPIs_ = value;
               RaisePropertyChanged("theVPIs");
            }
         }
      }



      public vpiList() { theVPIs = new ObservableCollection<rawVPI>(); }
      public vpiList(CogoStation aStation, double anElevation, double aVClength)
      {
         theVPIs = new ObservableCollection<rawVPI>();

      }

      public int Count {get{ return theVPIs.Count; }
         private set{}
      }

      public void add(CogoStation aStation, double anElevation, double aVClength)
      {
         theVPIs.Add(new rawVPI(aStation, anElevation, aVClength));
      }

      public void add(CogoStation aStation, double anElevation)
      {
         theVPIs.Add(new rawVPI(aStation, anElevation, 0.0));
      }

      public void add(double aStation, double anElevation, double aVClength)
      {
         theVPIs.Add(new rawVPI((CogoStation) aStation, anElevation, aVClength));
      }

      public void add(double aStation, double anElevation)
      {
         theVPIs.Add(new rawVPI((CogoStation)aStation, anElevation, 0.0));
      }

      public void add(rawVPI newVPI)
      {
         theVPIs.Add(newVPI);
      }

      public rawVPI getVPIbyIndex(int indx)
      {
         return theVPIs[indx];
      }

      public ObservableCollection<rawVPI> getVPIlist()
      {
         return theVPIs;
      }

      /// <summary>
      /// Raises the property changed event.
      /// </summary>
      /// <param name="propertyName">Name of the property.</param>
      protected void RaisePropertyChanged(string propertyName)
      {
         var handler = this.PropertyChanged;
         if (handler != null)
         {
            handler(this, new PropertyChangedEventArgs(propertyName));
         }
      }

      /// <summary>
      /// Occurs when a property value changes.
      /// </summary>
      public event PropertyChangedEventHandler PropertyChanged;

   }

   public class rawVPI : INotifyPropertyChanged
   {
      public rawVPI (CogoStation aStation, double anElevation, double aVClength)
      {
         Station = aStation;
         Elevation = anElevation;
         Length = aVClength;
      }

      public rawVPI (CogoStation aStation, double anElevation)
      {
         Station = aStation;
         Elevation = anElevation;
         Length = 0.0;
      }

      private CogoStation station_;
      public CogoStation Station
      {  get { return station_; }
         set
         {  if (station_ != value)
            {
               station_ = value;
               RaisePropertyChanged("Station");
         }  }
      }

      private double elevation_;
      public double Elevation
      {
         get { return elevation_; }
         set
         {
            if (elevation_ != value)
            {
               elevation_ = value;
               RaisePropertyChanged("Elevation");
            }
         }
      }

      private double length_;
      public double Length
      {
         get { return length_; }
         set
         {
            if (length_ != value)
            {
               length_ = value;
               RaisePropertyChanged("Length");
            }
         }
      }

      public CogoStation getEndStation()
      {
         return Station + (Length / 2.0);
      }

      public CogoStation getBeginStation()
      {
         return Station - (Length / 2.0);
      }

      /// <summary>
      /// Raises the property changed event.
      /// </summary>
      /// <param name="propertyName">Name of the property.</param>
      protected void RaisePropertyChanged(string propertyName)
      {
         var handler = this.PropertyChanged;
         if (handler != null)
         {
            handler(this, new PropertyChangedEventArgs(propertyName));
         }
      }

      /// <summary>
      /// Occurs when a property value changes.
      /// </summary>
      public event PropertyChangedEventHandler PropertyChanged;


   }

}
