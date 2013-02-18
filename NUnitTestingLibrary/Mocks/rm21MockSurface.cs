using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using rm21Core;
using ptsCogo;

namespace NUnitTestingLibrary.Mocks
{
   public class rm21MockSurface : Irm21surface
   {
      public Profile theProfile {get; set;}

      public rm21MockSurface()
      {
         theProfile = null;
      }

      public Profile getSectionProfile(ptsPoint BeginPoint, double startStation, ptsPoint EndPoint)
      {
         if (null == theProfile)
         {
            theProfile = new Profile(startStation, Math.Abs(startStation), 10.0);
         }

         return theProfile;
      }
   }
}
