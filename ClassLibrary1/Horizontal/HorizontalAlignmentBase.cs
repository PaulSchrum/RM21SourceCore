using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ptsCogo.Horizontal
{
   public  class HorizontalAlignmentBase : GenericAlignment
   {
      public HorizontalAlignmentBase() : base() { }

      public HorizontalAlignmentBase(List<Double> stationEquationingList) : base(stationEquationingList)
      {

      }

      public virtual Double Length { get; protected set; }
   }
}
