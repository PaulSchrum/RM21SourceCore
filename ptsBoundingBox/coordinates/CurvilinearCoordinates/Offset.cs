using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ptsCogo.coordinates.CurvilinearCoordinates
{
   class Offset
   {
      public Offset(double newVal) { OFST = newVal; }

      public double OFST { get; set; }

      private static String formatString;
      public override string ToString()
      {
         formatString = "0.00";
         if (OFST < 0.0)
         {
            return String.Format(formatString + " LT", -OFST);
         }
         else if (OFST > 0.0)
         {
            return String.Format(formatString + " RT", OFST);
         }
         else
         {
            return String.Format(formatString, OFST);
         }
      }

      public static Offset operator +(Offset anEL, Double other) { return new Offset(anEL.OFST + other); }
      public static double operator -(Offset leftOfOperand, Offset rightOfOperand) { return leftOfOperand.OFST - rightOfOperand.OFST; }
      public static explicit operator Offset(double aDouble) { return new Offset(aDouble); }
      public static implicit operator double(Offset anEL) { return anEL.OFST; }
   }
}
