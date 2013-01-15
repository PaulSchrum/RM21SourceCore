using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ptsCogo
{
   public class rm21Side
   {
      public rm21RightLeftSide RLside { get; set; }
      public rm21InOutSide InOutside { get; set; }

      public int Sign()
      {
         return (int) RLside * (int) InOutside;
      }

      public override string ToString()
      {
         return InOutside.GetType().ToString() + " " + RLside.GetType().ToString();
      }
   }

   public static class rm21SideHelper
   {
      public static void toggle(rm21RightLeftSide s)
      {
         if (s == rm21RightLeftSide.Left)
            s = rm21RightLeftSide.Right;
         else
            s = rm21RightLeftSide.Left;
      }
   }

   public enum rm21RightLeftSide
   {
      Left = -1,
      Right = 1
   }

   public enum rm21InOutSide
   {
      Inside = -1,
      Outside = 1
   }
}
