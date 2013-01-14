using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ptsCogo
{
   public static class rm21SideHelper
   {
      public static void toggle(rm21Side s)
      {
         if (s == rm21Side.Left)
            s = rm21Side.Right;
         else
            s = rm21Side.Left;
      }
   }

   public enum rm21Side
   {
      Left = -1,
      Right = 1
   }
}
