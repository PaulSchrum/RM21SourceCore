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

      private void LoadTINfromVRML(string fileName)
      {
         string line;
         long lineCount = 0;
         if (!(String.Compare(Path.GetExtension(fileName), ".wrl", true) == 0))
         {
            throw new Exception("Filename must have wrl extension.");
         }

         System.IO.StreamReader file = new System.IO.StreamReader(fileName);
         try
         {
            while ((line = file.ReadLine()) != null)
            {
               if (false == validateVRMLfileHeader(line))
                  throw new System.IO.InvalidDataException("File not in VRML2 format.");
               break;
            }

            lineCount++;
            while ((line = file.ReadLine()) != null)
            {
               lineCount++;
               if(line.Equals("IndexedFaceSet"))
                  break;
            }

            while ((line = file.ReadLine()) != null)
            {
               lineCount++;
               if (line.Equals("point"))
               {
                  line = file.ReadLine();  // eat the open brace,  [
                  break;
               }
            }

            ulong ptIndex=0;
            while ((line = file.ReadLine()) != null)
            {
               lineCount++;
               // Read until the close brace,  [
               if(line.Equals("]"))
                  break;
               scratchPoint = convertLineOfDataToPoint(line);
               if (allPoints == null)
               {
                  allPoints = new Dictionary<ulong, ptsDTMpoint>();
                  myBoundingBox = new ptsBoundingBox2d(scratchPoint.x, scratchPoint.y,scratchPoint.x, scratchPoint.y);
               }
               allPoints.Add(ptIndex, scratchPoint);
               ptIndex++;
               myBoundingBox.expandByPoint(scratchPoint.x, scratchPoint.y, scratchPoint.z);
            }

            while ((line = file.ReadLine()) != null)
            {
               lineCount++;
               if (line.Equals("coordIndex"))
               {
                  line = file.ReadLine();  // eat the open brace,  [
                  break;
               }
            }

            allTriangles = new List<ptsDTMtriangle>();
            while ((line = file.ReadLine()) != null)
            {
               lineCount++;
               // Read until the close brace,  [
               if (line.Equals("]"))
                  break;
               scratchTriangle = convertLineOfDataToTriangle(line);
               allTriangles.Add(scratchTriangle);
            }

            allTriangles.Sort();
         }
         finally
         {
            file.Close();
         }
      }

      private ptsDTMtriangle convertLineOfDataToTriangle(string line)
      {
         UInt64 ptIndex1, ptIndex2, ptIndex3;
         string[] parsed = line.Split(',');
         int correction = parsed.Length - 4;
         ptIndex1 = Convert.ToUInt64(parsed[0 + correction]);
         ptIndex2 = Convert.ToUInt64(parsed[1 + correction]);
         ptIndex3 = Convert.ToUInt64(parsed[2 + correction]);
         ptsDTMtriangle triangle = new ptsDTMtriangle(allPoints, ptIndex1, ptIndex2, ptIndex3);
         return triangle;
      }

      private ptsDTMpoint convertLineOfDataToPoint(string line)
      {
         ptsDTMpoint newPt;
         string[] preParsedLine = line.Split(',');
         string[] parsedLine = preParsedLine[preParsedLine.Length-1].Split(' ');

         newPt = new ptsDTMpoint(
            Convert.ToDouble(parsedLine[0]),
            Convert.ToDouble(parsedLine[1]),
            Convert.ToDouble(parsedLine[2]));

         return newPt;
      }

      private bool validateVRMLfileHeader(string line)
      {
         string[] words = line.Split(' ');
         if (words.Length < 2) return false;
         if (!(words[0].Equals("#VRML", StringComparison.OrdinalIgnoreCase))) return false;
         if (!(words[1].Equals("V2.0", StringComparison.OrdinalIgnoreCase))) return false;

         return true;
      }

      public void LoadTextFile(string fileName)
      {
         string extension;
         if (false == File.Exists(fileName))
            throw new FileNotFoundException("File Not Found", fileName);

         extension = Path.GetExtension(fileName);
         if (extension.Equals(".xml", StringComparison.OrdinalIgnoreCase))
            LoadTINfromLandXML(fileName);
         else if (extension.Equals(".wrl", StringComparison.OrdinalIgnoreCase))
            LoadTINfromVRML(fileName);
         else
            throw new Exception("Filename must have xml or wrl extension.");
      }

      private void LoadTINfromLandXML(string fileName)
      {
         if (!(String.Compare(Path.GetExtension(fileName), ".xml", true) == 0))
         {
            throw new Exception("Filename must have xml extension.");
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
                        myBoundingBox = new ptsBoundingBox2d(scratchPoint.x, scratchPoint.y, scratchPoint.x, scratchPoint.y);
                     }
                     allPoints.Add(id, scratchPoint);
                     myBoundingBox.expandByPoint(scratchPoint.x, scratchPoint.y, scratchPoint.z);
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

      public void testGetTriangles(ptsDTMpoint aPoint)
      {

         aStopwatch = new Stopwatch();
         System.Console.WriteLine("given a point, return triangles by BB:");
         aStopwatch.Reset(); aStopwatch.Start();

         List<ptsDTMtriangle> triangleSubset = getTrianglesForPointInBB(aPoint) as List<ptsDTMtriangle>;

         aStopwatch.Stop(); consoleOutStopwatch(aStopwatch);
      }

      internal List<ptsDTMtriangle> getTrianglesForPointInBB(ptsDTMpoint aPoint)
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

         ptsDTMtriangle singleTriangle = getTriangleContaining((ptsDTMpoint) aPoint);

         aStopwatch.Stop(); consoleOutStopwatch(aStopwatch);
      }

      internal ptsDTMtriangle getTriangleContaining(ptsDTMpoint aPoint)
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
         return getElevation((ptsDTMpoint)aPoint);
      }
      public double? getElevation(ptsDTMpoint aPoint)
      {
         ptsDTMtriangle aTriangle = getTriangleContaining(aPoint);
         if (null == aTriangle)
            return null;

         return aTriangle.givenXYgetZ(aPoint);

      }

      public double? getSlope(ptsPoint aPoint)
      {
         return getSlope((ptsDTMpoint)aPoint);
      }

      public double? getSlope(ptsDTMpoint aPoint)
      {
         ptsDTMtriangle aTriangle = getTriangleContaining(aPoint);
         if (null == aTriangle)
            return null;

         return aTriangle.givenXYgetSlopePercent(aPoint);

      }

      public Azimuth getSlopeAzimuth(ptsPoint aPoint)
      {
         return getSlopeAzimuth((ptsDTMpoint)aPoint);
      }

      public Azimuth getSlopeAzimuth(ptsDTMpoint aPoint)
      {
         ptsDTMtriangle aTriangle = getTriangleContaining(aPoint);
         if (null == aTriangle)
            return null;

         return aTriangle.givenXYgetSlopeAzimuth(aPoint);

      }

      public void loadFromXYZtextFile(string fileToOpen)
      {
         //ptsBoundingBox2d fileBB = new ptsBoundingBox2d()
         using (var inputFile = new StreamReader(fileToOpen))
         {
            Double x, y, z;
            String line;
            String[] values;
            while ((line = inputFile.ReadLine()) != null)
            {
               values = line.Split(',');
               if (values.Length != 3) continue;
               var newPt = new ptsDTMpoint(values[0], values[1], values[2]);
               GridDTMhelper.addPoint(newPt);
            }
            int i = 0;
         }
      }
   }

   internal static class GridDTMhelper
   {
      private const long GridSize = 500;
      public static Dictionary<XYtuple, List<ptsDTMpoint>> grid = new Dictionary<XYtuple, List<ptsDTMpoint>>();
      public static void addPoint(ptsDTMpoint pt)
      {
         long xGrid = (long)Math.Floor(pt.x / GridSize);
         long yGrid = (long)Math.Floor(pt.y / GridSize);
         addPoint_(new XYtuple(xGrid, yGrid), pt);
      }

      private static void addPoint_(XYtuple tupl, ptsDTMpoint pt)
      {
         if (false == grid.ContainsKey(tupl))
         {
            var ptList = new List<ptsDTMpoint>();
            ptList.Add(pt);
            grid.Add(tupl, ptList);
            long lng = (long)(int.MaxValue) + 1L;
         }
         else
         {
            grid[tupl].Add(pt);
         }
      }
   }

   internal class XYtuple
   {
      public XYtuple(long x, long y)
      {
         X = x; Y = y;
      }
      public long X { get; set; }
      public long Y { get; set; }
   }
}
