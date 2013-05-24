using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

namespace ptsCogo
{
   public class rm21Model
   {
      public rm21Model()
      {
         //allCorridors = new ObservableDictionary<string, rm21Corridor>();
         allCorridors = new ObservableCollection<rm21Corridor>();
      }

      //public ObservableDictionary<string, rm21Corridor> allCorridors { get; private set; }
      public ObservableCollection<rm21Corridor> allCorridors { get; private set; }
      public string currentValue { get; set; }

   }
}
