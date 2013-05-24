using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ptsCogo;

namespace ptsCogo
{
   public interface Irm21surface
   {
      Profile getSectionProfile(ptsPoint BeginPoint, double startStation, ptsPoint EndPoint);
      void setSectionProfileForMocking(Profile aProfileForMocking);
   }
}
