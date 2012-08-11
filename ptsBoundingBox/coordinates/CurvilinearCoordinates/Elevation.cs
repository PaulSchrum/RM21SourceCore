using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ptsCogo.coordinates.CurvilinearCoordinates
{
   class Elevation
   {
      public Elevation(double newVal) { EL = newVal; }

      public double EL { get; set; }

      public static Elevation operator +(Elevation anEL, Double other){return new Elevation(anEL.EL + other);}
      public static double operator -(Elevation leftOfOperand, Elevation rightOfOperand) { return leftOfOperand.EL - rightOfOperand.EL; }
      public static explicit operator Elevation(double aDouble){return new Elevation(aDouble);}
      public static implicit operator double(Elevation anEL) {return anEL.EL;}
   }
}
