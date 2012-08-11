using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Xml;
using System.Runtime.Serialization.Formatters.Binary;
using ptsCogo;
using ptsCogo.Angle;

namespace ptsDigitalTerrainModel
{
   [Serializable]
   public class ptsDTM
   {
      // Substantive members - Do serialize
      private Dictionary<UInt64, ptsDTMpoint> allPoints;
      private List<ptsDTMtriangle> allTriangles;
      private ptsBoundingBox2d myBoundingBox;

      // temp scratch pad members -- do not serialize
      [NonSerialized]
      private ptsDTMpoint scratchPoint;
      [NonSerialized]
      private ptsDTMtriangle scratchTriangle;
      [NonSerialized]
      private UInt64pair scratchUIntPair;
      [NonSerialized]
      private ptsDTMtriangleLine scratchTriangleLine;
      [NonSerialized]
      private Dictionary<UInt64pair, ptsDTMtriangleLine> triangleLines;

      [NonSerialized]
      private Dictionary<string, Stopwatch> stpWatches;
      [NonSerialized]
      private Stopwatch aStopwatch;

      private ptsDTM() { }

      // Constructor to construct from an xml file
      public ptsDTM(string fileName)
      {
         if (!(String.Compare(Path.GetExtension(fileName), "xml", true) == 0))
         {
            //throw new notAnXMLfileException();
         }

         Stopwatch stopwatch = new Stopwatch();
         List<string> trianglesAsStrings;
         setupStopWatches();

         scratchUIntPair = new UInt64pair();
         
         System.Console.WriteLine("Load XML document took:");
         stopwatch.Reset();
         stopwatch.Start();
         using (XmlTextReader reader = new XmlTextReader(fileName))
         {
            stopwatch.Stop();  consoleOutStopwatch(stopwatch);
            System.Console.WriteLine("Seeking Pnts collection took:");
            stopwatch.Reset();    stopwatch.Start();
            reader.MoveToContent();
            reader.ReadToDescendant("Surface");
            string astr = reader.GetAttribute("name");

            // Read Points
            reader.ReadToDescendant("Pnts");
            stopwatch.Stop();  consoleOutStopwatch(stopwatch);
            
            System.Console.WriteLine("Loading All Points took:");
            stopwatch.Reset();    stopwatch.Start();
            reader.Read();
            while (!(reader.Name.Equals("Pnts") && reader.NodeType.Equals(XmlNodeType.EndElement)))
            {
               UInt64 id;
               if (reader.NodeType.Equals(XmlNodeType.Element))
               {
                  UInt64.TryParse(reader.GetAttribute("id"), out id);
                  reader.Read();
                  if (reader.NodeType.Equals(XmlNodeType.Text))
                  {
                     scratchPoint = new ptsDTMpoint(reader.Value, id);
                     if (allPoints == null)
                     {
                        allPoints = new Dictionary<ulong, ptsDTMpoint>();
                        myBoundingBox = new ptsBoundingBox2d(scratchPoint);
                     }
                     allPoints.Add(id, scratchPoint);
                     myBoundingBox.expandByPoint(scratchPoint);
                  }
               }
               reader.Read();
            }

            // Read Triangles, but only as strings
            stopwatch.Stop();  consoleOutStopwatch(stopwatch);
            
            System.Console.WriteLine("Loading Triangle Reference Strings took:");
            stopwatch.Reset();    stopwatch.Start();
            trianglesAsStrings = new List<string>();
            if (!(reader.Name.Equals("Faces")))
            {
               reader.ReadToFollowing("Faces");
            }
            reader.Read();
            while (!(reader.Name.Equals("Faces") && reader.NodeType.Equals(XmlNodeType.EndElement)))
            {
               if (reader.NodeType.Equals(XmlNodeType.Text))
               {
                  trianglesAsStrings.Add(reader.Value);
               }
               reader.Read();
            }
            reader.Close();
            stopwatch.Stop();  consoleOutStopwatch(stopwatch);
            
            System.Console.WriteLine("Generating Triangle Collection took:");
            stopwatch.Reset();    stopwatch.Start();
         }

         
         // assemble the allTriangles collection
         UInt64 counter = 0;
         allTriangles = new List<ptsDTMtriangle>();   //(int)trianglesAsStrings.Count);
         foreach (string refString in trianglesAsStrings)
         {
            scratchTriangle = new ptsDTMtriangle(allPoints, refString);
            allTriangles.Add(scratchTriangle);
            counter++;
         }
         trianglesAsStrings = null;
         GC.Collect(); GC.WaitForPendingFinalizers();

         stopwatch.Stop(); consoleOutStopwatch(stopwatch);
         
         System.Console.WriteLine("Sorting Triangle Collection in x took:");
         stopwatch.Reset(); stopwatch.Start();
         allTriangles.Sort();
         stopwatch.Stop(); consoleOutStopwatch(stopwatch);

         //
         //System.Console.WriteLine("Indexing Triangles for adjacency took:");
         //stopwatch.Reset(); stopwatch.Start();
         //generateTriangleLineIndex();  start here
         //stopwatch.Stop(); consoleOutStopwatch(stopwatch);

      }

      public void saveAsBinary(string filenameToSaveTo)
      {
         BinaryFormatter binFrmtr = new BinaryFormatter();
         using
         (Stream fstream = 
            new FileStream(filenameToSaveTo, FileMode.Create, FileAccess.Write, FileShare.None))
         {
            binFrmtr.Serialize(fstream, this);
         }
      }

      static public ptsDTM loadFromBinary(string filenameToLoad)
      {
         BinaryFormatter binFrmtr = new BinaryFormatter();
         using
         (Stream fstream = File.OpenRead(filenameToLoad))
         {
            ptsDTM aDTM = new ptsDTM();
            try{   aDTM = (ptsDTM)binFrmtr.Deserialize(fstream);    }
                                                   #pragma warning disable 0168
            catch (InvalidCastException e)
                                                   #pragma warning restore 0168
            {  return null;  }

            foreach (var triangle in aDTM.allTriangles)
            {
               triangle.computeBoundingBox();
            }
            return aDTM;
         }
         //return null;
      }

      private void setupStopWatches()
      {
         stpWatches = new Dictionary<string, Stopwatch>();
         stpWatches.Add("Process Points", new Stopwatch());
         stpWatches.Add("Process Triangles", new Stopwatch());
      }

      private void consoleOutStopwatch(Stopwatch anSW)
      {
         TimeSpan ts = anSW.Elapsed;

         // Format and display the TimeSpan value.
         string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
             ts.Hours, ts.Minutes, ts.Seconds,
             ts.Milliseconds / 10);
         Console.WriteLine("RunTime " + elapsedTime);
         Console.WriteLine();
      }

      private bool addTriangleLine(UInt64 ndx1, UInt64 ndx2, ptsDTMtriangle aTriangle)
      {
         if (ndx1 == 0 || ndx2 == 0 || aTriangle == null)
            return false;

         if (ndx1 < ndx2)
         {
            scratchUIntPair.num1 = ndx1;
            scratchUIntPair.num2 = ndx2;
         }
         else
         {
            scratchUIntPair.num1 = ndx2;
            scratchUIntPair.num2 = ndx1;
         }

         if (triangleLines == null)
         {
            triangleLines = new Dictionary<UInt64pair, ptsDTMtriangleLine>();
            scratchTriangleLine = new ptsDTMtriangleLine(allPoints[ndx1], allPoints[ndx2], aTriangle);
            triangleLines.Add(scratchUIntPair, scratchTriangleLine);

            return true;
         }

         bool tryGetSucces = triangleLines.TryGetValue(scratchUIntPair, out scratchTriangleLine);
         if (tryGetSucces == false)  // we must add this line to the collection
         {
            scratchTriangleLine = new ptsDTMtriangleLine(allPoints[ndx1], allPoints[ndx2], aTriangle);
            triangleLines.Add(scratchUIntPair, scratchTriangleLine);
            return true;
         }
         else
         {
            if (scratchTriangleLine.theOtherTriangle == null)
            {
               scratchTriangleLine.theOtherTriangle = aTriangle;
               return true;
            }
            else
            {
               return false;
            }
         }
      }

      public void testGetTriangles(ptsPoint aPoint)
      {

         aStopwatch = new Stopwatch();
         System.Console.WriteLine("given a point, return triangles by BB:");
         aStopwatch.Reset(); aStopwatch.Start();

         List<ptsDTMtriangle> triangleSubset = getTrianglesForPointInBB(aPoint) as List<ptsDTMtriangle>;

         aStopwatch.Stop(); consoleOutStopwatch(aStopwatch);
      }

      internal List<ptsDTMtriangle> getTrianglesForPointInBB(ptsPoint aPoint)
      {
         return  (from ptsDTMtriangle triangle in allTriangles
                          where triangle.isPointInBoundingBox(aPoint)
                          select triangle).ToList<ptsDTMtriangle>();
      }

      public void testGetTriangle(ptsPoint aPoint)
      {
         aStopwatch = new Stopwatch();
         System.Console.WriteLine("given a point, return containing Triangle:");
         aStopwatch.Reset(); aStopwatch.Start();

         ptsDTMtriangle singleTriangle = getTriangleContaining(aPoint);

         aStopwatch.Stop(); consoleOutStopwatch(aStopwatch);
      }

      internal ptsDTMtriangle getTriangleContaining(ptsPoint aPoint)
      {
         bool found = false;
         ptsDTMtriangle theTriangle = null;
         foreach(ptsDTMtriangle aTriangle in getTrianglesForPointInBB(aPoint) as List<ptsDTMtriangle>)
         {
            if (aTriangle.contains(aPoint))
            {
               found = true;
               theTriangle = aTriangle;
               break;
            }
         }
         return found == true ? theTriangle : null;
      }

      public double? getElevation(ptsPoint aPoint)
      {
         ptsDTMtriangle aTriangle = getTriangleContaining(aPoint);
         if (null == aTriangle)
            return null;

         return aTriangle.givenXYgetZ(aPoint);

      }

      public double? getSlope(ptsPoint aPoint)
      {
         ptsDTMtriangle aTriangle = getTriangleContaining(aPoint);
         if (null == aTriangle)
            return null;

         return aTriangle.givenXYgetSlopePercent(aPoint);

      }

      public Azimuth getSlopeAzimuth(ptsPoint aPoint)
      {
         ptsDTMtriangle aTriangle = getTriangleContaining(aPoint);
         if (null == aTriangle)
            return null;

         return aTriangle.givenXYgetSlopeAzimuth(aPoint);

      }
   }
   
}
