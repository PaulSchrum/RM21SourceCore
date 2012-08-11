using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ptsCogo
{
   public class utilFunctions
   {
      public static int tolerantCompare(double value1, double value2, double tolerance)
      {
         double diff = value2 - value1;
         if (Math.Abs(diff) < tolerance)
         {
            return 0;
         }

         if (value2 > value1) return 1;

         return -1;
      }

      public static int tolerantCompare(double? value1, double? value2, double tolerance)
      {
         if (value1 == null || value2 == null) return -1;
         double diff = (double)value2 - (double)value1;
         if (Math.Abs(diff) < tolerance)
         {
            return 0;
         }

         if (value2 > value1) return 1;

         return -1;
      }
   }
}
