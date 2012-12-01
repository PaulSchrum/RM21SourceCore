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

      public static double? addRecipricals(double? val1, double? val2)
      {
         if (null == val1)
         {
            if (null == val2)
               return null;
            else
               return val2;
         }
         else if (null == val2)
         {
            return val1;
         }
         else
         {
            if (val1 == 0.0)
               return val2;
            else if (val2 == 0.0)
               return val1;
            else
            {
               double? recip1 = 1 / val1;
               double? recip2 = 1 / val2;
               return 1 / (recip1 + recip2);
            }
         }
      }
   }
}
