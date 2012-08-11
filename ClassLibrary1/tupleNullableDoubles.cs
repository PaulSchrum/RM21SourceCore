using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ptsCogo
{
   /// <summary>
   /// Use this struct, tupleNullableDoubles for resutls returning from
   /// Profiles.  If the station is off the profile, the double?s are
   /// not valid.  If they are valid but not equal, notSingleValue is
   /// true, and back does not equal ahead.  This will happen most
   /// often when a vpi with vc length == 0.  But most of the time,
   /// notSingleValue == false, which means back == ahead.
   /// 
   /// It is noteworthy that at the beginning station of the profile,
   /// back is null, ahead is non-nul, and notSingleValue == true.  The
   /// same thing happens at the end of a profile, but ahead == null,
   /// back is non-null.
   /// </summary>
   public struct tupleNullableDoubles
   {
#pragma warning disable 0649
      public double? back;
      public double? ahead;
      public bool isSingleValue;
#pragma warning restore 0649

   }
}
