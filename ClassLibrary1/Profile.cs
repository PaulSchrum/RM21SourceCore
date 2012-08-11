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

      public double beginProfTrueStation {get; private set;}
      public double endProfTrueStation { get; private set; }

      private Profile() { }
      public Profile(vpiList rawVPIlist)
      {
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
                        allVCs.Add(newVC);
                     }
                     // End: add a VC for the incoming tangent when necessary

                     // add a VC for the current vertical curve if VClen > 0
                     if (vpi2.length > 0.0)
                     {
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
                     if (count == allVCs.Count)
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

      static public double getELchangeAlongSlope(double grade, double distance)
      {
         return distance * grade;
      }

      private void setIndexToTheRightVC(CogoStation aStation)
      {
         while (aStation.trueStation > allVCs[vcIndex].endStation.trueStation)
         {
            vcIndex++;
            if (vcIndex == allVCs.Count)
            {
               vcIndex = allVCs.Count;
               throw new IndexOutOfRangeException();
            }
         }
         while (aStation.trueStation < allVCs[vcIndex].beginStation.trueStation)
         {
            vcIndex--;
            if (vcIndex == 0)
            {
               vcIndex = 0;
               throw new IndexOutOfRangeException();
            }
         }
      }


      //This function should be adjusted to use a delegate plus wrapper functions
      public void getElevation(CogoStation station, ref tupleNullableDoubles theElevation)
      {
         if ((station.trueStation < beginProfTrueStation - stationEqualityTolerance) ||
             (station.trueStation > endProfTrueStation + stationEqualityTolerance))
         {
            theElevation.back = null;
            theElevation.ahead = null;
            theElevation.isSingleValue = true;
         }

         setIndexToTheRightVC(station);
         verticalCurve aVC = allVCs[vcIndex];

         // if we are at the begin station, check to see how we relate to the previous vc
         if (utilFunctions.tolerantCompare(station.trueStation, aVC.beginStation.trueStation, stationEqualityTolerance) == 0)
         {
            // if we are at the beginning of the profile, split theElevation
            if (vcIndex == 0)
            {
               theElevation.back = null;
               theElevation.ahead = aVC.getElevation(station);
               theElevation.isSingleValue = false;
            }
            else  // if station is on the boundary between two verticalCurves,
            {     // then see if we need to split theElevation
               theElevation.ahead = aVC.getElevation(station);
               theElevation.back = allVCs[vcIndex - 1].getElevation(station);
               if (utilFunctions.tolerantCompare(theElevation.back, theElevation.ahead, 0.00005) == 0)
               {
                  theElevation.isSingleValue = true;
               }
               else theElevation.isSingleValue = false;
            }
         }
         // End: if we are at the begin station, check to see how we relate to the previous vc
         // if we are at the end station, check to see how we relate to the next vc
         else if (utilFunctions.tolerantCompare(station.trueStation, aVC.endStation.trueStation, stationEqualityTolerance) == 0)
         {
            // if we are at the end of the profile, split theElevation
            if (vcIndex == allVCs.Count - 1)
            {
               theElevation.back = aVC.getElevation(station);
               theElevation.ahead = null;
               theElevation.isSingleValue = false;
            }
            else  // if station is on the boundary between two verticalCurves,
            {     // then see if we need to split theElevation
               theElevation.back = aVC.getElevation(station);
               theElevation.ahead = allVCs[vcIndex + 1].getElevation(station);
               if (utilFunctions.tolerantCompare(theElevation.back, theElevation.ahead, 0.00005) == 0)
               {
                  theElevation.isSingleValue = true;
               }
               else theElevation.isSingleValue = false;
            }
         }
         // End: if we are at the end station, check to see how we relate to the next vc
         else
         {
            theElevation.back = aVC.getElevation(station);
            theElevation.ahead = theElevation.back;
            theElevation.isSingleValue = true;
         }
      }

      public void getSlope(CogoStation station, ref tupleNullableDoubles theSlope)
      {
         theSlope.back = 1.22;
         theSlope.ahead = theSlope.back;
         theSlope.isSingleValue = false;
      }

      public void getKvalue(CogoStation station, ref tupleNullableDoubles theKvalue)
      {
         theKvalue.back = 1.22;
         theKvalue.ahead = theKvalue.back;
         theKvalue.isSingleValue = false;
      }

      private class verticalCurve
      {
         private double length_;
         private double kValue_;
         private CogoStation endStation_;
         private bool isTangent_;

         public bool isTangent 
         { 
            get { return isTangent_; } 
            set 
            {
               isTangent_ = value;
               if (isTangent_ == true)
               {
                  kValue_ = Double.MaxValue;
               }
            } 
         }

         public CogoStation beginStation { get; set; }
         public CogoStation endStation { get { return endStation_; } private set { } }
         public double beginElevation { get; set; }
         public double beginSlope { get; set; }
         public double endSlope { get; set; }
         public double kValue { get { return kValue_; } private set { } }
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
                     kValue_ = (endSlope - beginSlope) / length_;
                  }
                  else
                  {
                     kValue_ = Double.MaxValue;
                  }
               }
               else
               {
                  throw new NotSupportedException("Length of vertical curve not allowed to be less than 0.");
               }
            } 
         }

         public double getElevation(CogoStation station)
         {
            double theElevation;
            double lenSquared; double lenIntoVC;

            lenIntoVC = station - beginStation;
            lenSquared = lenIntoVC * lenIntoVC;

            theElevation = beginElevation +
               (lenIntoVC * beginSlope);

            if (isTangent == false)
               theElevation += (kValue * lenSquared / 2.0);

            return theElevation;

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
