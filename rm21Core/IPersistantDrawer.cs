using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ptsCogo
{
   public interface IPersistantDrawer : IPersistantDrawer_Cogo
   {
      void setCaddSymbologyForRibbon(ptsCogo.rm21Feature feature);
   }
}
