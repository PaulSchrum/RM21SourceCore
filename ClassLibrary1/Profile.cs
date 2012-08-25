using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ptsCogo;

namespace ptsCogo
{
   public class Profile //: GenericAlignment
   {
      private List<verticalCurve> allVCs;
      private int vcIndex=0;
      private double stationEqualityTolerance = 0.00005;
      private bool iHaveOneOrMoreVerticalCurves { get; set; }

      public double beginProfTrueStation {get; private set;}
      public double endProfTrueStation { get; private set; }

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
            rawVPI vpi1 = rawVPIlist.getVPIbyIndex(0);
            rawVPI vpi2 = rawVPIlist.getVPIbyIndex(1);

            verticalCurve aNewVerticalCurve = new verticalCurve();
            aNewVerticalCurve.beginElevation = vpi1.elevation;

            aNewVerticalCurve.beginStation = vpi1.station;
            beginProfTrueStation = vpi1.station.trueStation;
            aNewVerticalCurve.length = vpi2.station - vpi1.station;
            endProfTrueStation = vpi2.station.trueStation;

            aNewVerticalCurve.beginSlope = (vpi2.elevation - vpi1.elevation) /
                                             aNewVerticalCurve.length;
            aNewVerticalCurve.isTangent = true;
            aNewVerticalCurve.beginIsPINC = false;
            aNewVerticalCurve.endIsPINC = false;

            allVCs = new List<verticalCurve>();
            allVCs.Add(aNewVerticalCurve);
         }
         else
         {
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
                        beginProfTrueStation = vpi1.station.trueStation;
                     }

                     g1 = (vpi2.elevation - vpi1.elevation) /
                          (vpi2.station.trueStation - vpi1.station.trueStation);

                     g2 = (vpi3.elevation - vpi2.elevation) /
                          (vpi3.station.trueStation - vpi2.station.trueStation);

                     double incomingTanLen;
                     incomingTanLen = vpi2.getBeginStation() - vpi1.getEndStation();

                     // add a VC for the incoming tangent when necessary
                     if (incomingTanLen > 0.0)
                     {
                        newVC = new verticalCurve();
                        newVC.beginSlope = g1;
                        newVC.beginStation = vpi1.getEndStation();
                        newVC.endSlope = g1;
                        newVC.length = incomingTanLen;
                        newVC.beginElevation = vpi2.elevation + getELchangeAlongSlope(g1, 
                           (vpi1.getEndStation() - vpi2.station));
                        
                        newVC.beginIsPINC = false;
                        if (allVCs.Count > 0)
                        {
                           newVC.beginIsPINC = allVCs.Last<verticalCurve>().endIsPINC;
                        }

                        newVC.endIsPINC = false;
                        if (utilFunctions.tolerantCompare(vpi2.length, 0.0, stationEqualityTolerance) == 0)
                        {
                           newVC.endIsPINC = true;
                        }
                        
                        allVCs.Add(newVC);
                     }
                     // End: add a VC for the incoming tangent when necessary

                     // add a VC for the current vertical curve if VClen > 0
                     if (vpi2.length > 0.0)
                     {
                        iHaveOneOrMoreVerticalCurves = true;
                        newVC = new verticalCurve();
                        newVC.beginSlope = g1;
                        newVC.beginStation = vpi2.getBeginStation();
                        newVC.endSlope = g2;
                        newVC.length = vpi2.length;
                        newVC.beginElevation = vpi2.elevation - getELchangeAlongSlope(g1, newVC.length / 2.0);
                        allVCs.Add(newVC);
                        endProfTrueStation = newVC.beginStation.trueStation + newVC.length;
                     }
                     // End: add a VC for the current vertical curve if VClen > 0

                     // if this is the final VPI, add a final tangent if necessary
                     if (count == rawVPIlist.Count)
                     {
                        double outgoingTangentLength = vpi3.getBeginStation() - vpi2.getEndStation();
                        if (outgoingTangentLength > 0.0)
                        {
                           newVC = new verticalCurve();
                           newVC.beginSlope = g2;
                           newVC.beginStation = vpi2.getEndStation();
                           newVC.endSlope = g2;
                           newVC.length = outgoingTangentLength;
                           newVC.beginElevation = vpi2.elevation + getELchangeAlongSlope(g2, vpi2.length / 2.0);
                           
                           newVC.beginIsPINC = false;
                           if (allVCs.Count > 0)
                           {
                              newVC.beginIsPINC = allVCs.Last<verticalCurve>().endIsPINC;
                           }

                           newVC.endIsPINC = false;

                           allVCs.Add(newVC);
                           endProfTrueStation = newVC.beginStation.trueStation + newVC.length;
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
         if (newStation > endProfTrueStation)
         {
            vcIndex = allVCs.Count - 1;
            otherVC = allVCs[vcIndex];
            newVC.beginStation = otherVC.endStation;
            endProfTrueStation = newStation.trueStation;
            otherVC.endIsPINC = true;
            newVC.beginElevation = verticalCurve.getElevation(otherVC, (CogoStation)otherVC.endStation);
            newVC.beginIsPINC = true;
            newVC.endIsPINC = false;
            newVC.isTangent = true;
            newVC.beginSlope = (newElevation - newVC.beginElevation) /
                  (newStation - newVC.beginStation);
            newVC.endSlope = newVC.beginSlope;
            newVC.length = newStation - newVC.beginStation;
            allVCs.Add(newVC);
         }
         else if (newStation < beginProfTrueStation)  //Insert new pi before first station
         {
            vcIndex = 0;
            otherVC = allVCs[vcIndex];
            newVC.beginStation = newStation;
            beginProfTrueStation = newStation.trueStation;
            otherVC.beginIsPINC = true;
            newVC.beginElevation = newElevation;
            newVC.beginIsPINC = false;
            newVC.endIsPINC = true;
            newVC.isTangent = true;
            newVC.beginSlope = (otherVC.beginElevation - newElevation) /
                  (otherVC.beginStation - newStation);
            newVC.endSlope = newVC.beginSlope;
            newVC.length = otherVC.beginStation - newStation;
            allVCs.Insert(0, newVC);
         }
         //insert new pi interior to the profile, but one that has no vertical curves
         else
         {  //(CogoStation newStation, double newElevation)
            setIndexToTheCorrectVC(newStation);
            otherVC = allVCs[vcIndex];
            
            // see if new station is already in the profile as a vpi
            if(newStation == otherVC.beginStation)
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
            else if(newStation == otherVC.endStation)
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
               station1 = otherVC.beginStation; station2 = newStation; station3 = otherVC.endStation;
               elevation1 = otherVC.beginElevation; elevation2 = newElevation;
               elevation3 = verticalCurve.getElevation(otherVC, station3);

               otherVC.setVerticalTangent(station1, elevation1, station2, elevation2);
               newVC = new verticalCurve();
               newVC.setVerticalTangent(station2, elevation2, station3, elevation3);

               verticalCurve otherOtherVC;
               if (vcIndex > 0)
               {
                  int VCindex0 = vcIndex - 1;
                  otherOtherVC = allVCs[VCindex0];
                  if (otherOtherVC.endSlope == otherVC.beginSlope)
                     otherOtherVC.endIsPINC = otherVC.beginIsPINC = false;
                  else
                     otherOtherVC.endIsPINC = otherVC.beginIsPINC = true;
               }

               if (vcIndex < allVCs.Count-1)
               {
                  int VCindex3 = vcIndex + 1;
                  otherOtherVC = allVCs[VCindex3];
                  if (otherOtherVC.beginSlope == otherVC.endSlope)
                     otherOtherVC.beginIsPINC = otherVC.endIsPINC = false;
                  else
                     otherOtherVC.beginIsPINC = otherVC.endIsPINC = true;
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
         while (aStation.trueStation > allVCs[vcIndex].endStation.trueStation)
         {
            vcIndex++;
            if (vcIndex > allVCs.Count-1)
            {
               vcIndex = allVCs.Count;
               throw new IndexOutOfRangeException();
            }
         }
         while (aStation.trueStation < allVCs[vcIndex].beginStation.trueStation)
         {
            vcIndex--;
            if (vcIndex < 0)
            {
               vcIndex = 0;
               throw new IndexOutOfRangeException();
            }
         }
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
         if ((station.trueStation < beginProfTrueStation - stationEqualityTolerance) ||
             (station.trueStation > endProfTrueStation + stationEqualityTolerance))
         {
            theOutValue.back = null;
            theOutValue.ahead = null;
            theOutValue.isSingleValue = true;
         }

         setIndexToTheCorrectVC(station);
         verticalCurve aVC = allVCs[vcIndex];

         // if we are at the begin station, check to see how we relate to the previous vc
         if (utilFunctions.tolerantCompare(station.trueStation, aVC.beginStation.trueStation, stationEqualityTolerance) == 0)
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
                   aVC.beginIsPINC)
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
         else if (utilFunctions.tolerantCompare(station.trueStation, aVC.endStation.trueStation, stationEqualityTolerance) == 0)
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
                   aVC.endIsPINC)
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
         var aVC = allVCs.FirstOrDefault(vc => utilFunctions.tolerantCompare(vc.beginStation.trueStation, aStation.trueStation, stationEqualityTolerance) == 0);
         if (aVC == null)
            return false;
         else
            return aVC.beginIsPINC;
      }

      private class verticalCurve
      {
         private double length_;
         private CogoStation endStation_;
         private bool isTangent_;
         private double kValue_;

         public bool isTangent 
         { 
            get { return isTangent_; } 
            set 
            {
               isTangent_ = value;
               if (isTangent_ == true)
               {
                  kValue = double.PositiveInfinity;
               }
            } 
         }

         public CogoStation beginStation { get; set; }
         public CogoStation endStation { get { return endStation_; } private set { } }
         public double beginElevation { get; set; }
         public double beginSlope { get; set; }
         public double endSlope { get; set; }
         public bool beginIsPINC { get; set; }  // PINC = PI, No Curve.
         public bool endIsPINC { get; set; }    //  used to detect undefined K values at PINC stations
         public double kValue { get { return 0.01 / kValue_; } private set { kValue_ = value; } }
         public double length 
         { get { return length_; } 
            set
            { 
               length_ = value;
               if (length_ > 0.0)
               {
                  endStation_ = beginStation + length_;
                  if (isTangent_ == false)
                  {
                     kValue =  (endSlope - beginSlope) / length_;
                  }
                  else
                  {
                     kValue = double.PositiveInfinity ;
                  }
               }
               else
               {
                  throw new NotSupportedException("Length of vertical curve not allowed to be less than 0.");
               }
            } 
         }

         internal void setVerticalTangent(CogoStation sta1, double EL1, CogoStation sta2, double EL2)
         {
            beginStation = sta1;
            beginElevation = EL1;
            beginSlope = getSlopeFromDoubles(EL1, EL2, sta1.trueStation, sta2.trueStation);
            isTangent = true;
            endSlope = beginSlope;
            length = sta2 - sta1;
         }

         internal delegate double getSwitchForProfiles(verticalCurve aVC, CogoStation station);

         static public double getElevation(verticalCurve aVC, CogoStation station)
         {
            double theElevation;
            double lenSquared; double lenIntoVC;

            lenIntoVC = station - aVC.beginStation;
            lenSquared = lenIntoVC * lenIntoVC;

            theElevation = aVC.beginElevation +
               (lenIntoVC * aVC.beginSlope);

            if (aVC.isTangent == false)
               theElevation += (lenSquared / (200.0 * aVC.kValue));

            return theElevation;
         }

         static public double getSlope(verticalCurve aVC, CogoStation station)
         {
            double theSlope;
            double lenSquared; double lenIntoVC;

            lenIntoVC = station - aVC.beginStation;
            lenSquared = lenIntoVC * lenIntoVC;

            theSlope = aVC.beginSlope +
               (lenIntoVC / (100.0 * aVC.kValue));

            return theSlope;
         }

         static public double getKvalue(verticalCurve aVC, CogoStation station)
         {
            return aVC.kValue;
         }
      }
   }

   public class vpiList
   {
      private List<rawVPI> theVPIs;
      public vpiList() { theVPIs = new List<rawVPI>(); }
      public vpiList(CogoStation aStation, double anElevation, double aVClength)
      {
         theVPIs = new List<rawVPI>();

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

      internal List<rawVPI> getVPIlist()
      {
         return theVPIs;
      }
   }

   public class rawVPI
   {
      public rawVPI (CogoStation aStation, double anElevation, double aVClength)
      {
         station = aStation;
         elevation = anElevation;
         length = aVClength;
      }

      public rawVPI (CogoStation aStation, double anElevation)
      {
         station = aStation;
         elevation = anElevation;
         length = 0.0;
      }

      public CogoStation station { get; set; }
      public double elevation { get; set; }
      public double length { get; set; }

      public CogoStation getEndStation()
      {
         return station + (length / 2.0);
      }

      public CogoStation getBeginStation()
      {
         return station - (length / 2.0);
      }
   }

}
