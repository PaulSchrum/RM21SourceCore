using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ptsCogo.coordinates.CurvilinearCoordinates;
using ptsCogo;
using ptsCogo.Angle;
using System.Collections.ObjectModel;
using System.Windows.Media;

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

      private void DrawCenterLineAnnotationelementsForXS(IRM21cad2dDrawingContext cadContext, CogoStation station)
      {
         cadContext.setElementColor(Color.FromArgb(255, 255, 255, 255));
         cadContext.setElementWeight(1.5);
         cadContext.Draw(0.0, 0.5, 0.0, 8.0);
         cadContext.Draw("C", -0.45, 8.6, 0.0);
         cadContext.Draw("L", -0.2, 8.3, 0.0);
      }

      public void DrawCrossSection(IRM21cad2dDrawingContext cadContext, CogoStation station)
      {
         if (allPGLgroupings != null)
         {
            DrawCenterLineAnnotationelementsForXS(cadContext, station);
            foreach (var pglGrouping in allPGLgroupings)
            {
               pglGrouping.DrawCrossSection(cadContext, station, pglGrouping.myIndex);
            }
         }
      }

      public void DrawPlanViewSchematic(IRM21cad2dDrawingContext cadContext, CogoStation station)
      {
         cadContext.setElementColor(Color.FromArgb(255, 255, 255, 0));  // yellow
         cadContext.setElementWeight(2.25);
      }

   }

}

// I think I need
// class negatableIndexList<T> : List<T>
// which would cover my worries about iterating from -2 to +2
// while skipping zero.


public interface IRM21cad2dDrawingContext
{
   void resetDashArray();
   void addToDashArray(double dashLength);
   void setElementLevel(string LevelName);
   void setElementColor(Color Color);
   void setElementWeight(double Weight);
   void Draw(double X1, double Y1, double X2, double Y2);
   void Draw(string TextContent, double X1, double Y1, double rotationAngle);
}



