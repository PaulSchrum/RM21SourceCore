using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ptsDigitalTerrainModel
{
   internal class UInt64pair
   {
      public UInt64 num1;
      public UInt64 num2;

      public override bool Equals(object obj)
      {
         UInt64pair other = obj as UInt64pair;
         if (other == null)
            return false;

         return ((this.num1 == other.num1) && (this.num2 == other.num2));
      }

      public override int GetHashCode()
      {
         return base.GetHashCode();
      }
   }

}
