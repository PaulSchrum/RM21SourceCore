using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ptsCogo.coordinates.CurvilinearCoordinates;
using ptsCogo;
using ptsCogo.Angle;
using System.Collections.ObjectModel;

namespace rm21Core
{
   /// <summary>
   /// This class represents all surfaces controlled by the same chain, 
   ///   including medians.  Eventually this class will be made abstract
   ///   and roadways will be implemented by a subclass, just as riparian
   ///   ways and railways will be.
   /// </summary>
   public class rm21Corridor
   {
      public ObservableCollection<PGLGrouping> allPGLgroupings { get; private set; }

      public rm21Corridor() { }

      public rm21Corridor(string name_)
      {
         Name = name_;
      }

      public string Name { get; set; }

      public void addPGLgrouping(PGLGrouping aPGLgrouping)
      {
         if (aPGLgrouping == null)
            throw new ArgumentNullException();

         if (allPGLgroupings == null)
            allPGLgroupings = new ObservableCollection<PGLGrouping>();

         allPGLgroupings.Add(aPGLgrouping);
      }

      public bool getElevation(ref StationOffsetElevation soePoint)
      {
         foreach (var pglGr in allPGLgroupings)
         {
            int isOnThisPGLgrp = pglGr.getElevationFromSOE(ref soePoint);
            if (isOnThisPGLgrp == 0)
               return true;
         }

         return false;
      }

      public override string ToString()
      {
         return Name;
      }

   }

}

// I think I need
// class negatableIndexList<T> : List<T>
// which would cover my worries about iterating from -2 to +2
// while skipping zero.


