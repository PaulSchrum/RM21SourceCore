using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;


namespace ptsCogo
{
   public interface Irm21TreeViewItemable
   {
      string getHashName();
      ObservableCollection<Irm21TreeViewItemable> getChildren();
   }
}
