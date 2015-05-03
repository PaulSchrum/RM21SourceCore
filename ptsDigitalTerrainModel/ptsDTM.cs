using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Xml;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Concurrent;
using ptsCogo;
using ptsCogo.Angle;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.IO.Compression;

namespace ptsDigitalTerrainModel
{
   public class ptsDTM
   {
      private Dictionary<UInt64, ptsDTMpoint> allPoints;
      public List<ptsDTMtriangle> allTriangles;
      //private ptsDTMtriangle[] allTriangles;
      private ptsBoundingBox2d myBoundingBox;

      private static readonly string TEMP_POINTS_FILENAME = "⊙Temp.ptsTin";
      private static readonly string TEMP_TRIANGLES_FILENAME = "ΔTemp.ptsTin";

      // temp scratch pad members -- do not serialize
      private ConcurrentBag<ptsDTMtriangle> allTrianglesBag;
      private ptsDTMpoint scratchPoint;
      private ptsDTMtriangle scratchTriangle;
      private UInt64pair scratchUIntPair;
      private ptsDTMtriangleLine scratchTriangleLine;
      private Dictionary<UInt64pair, ptsDTMtriangleLine> triangleLines;
      private long memoryUsed = 0;

      private Dictionary<string, Stopwatch> stpWatches;
      private Stopwatch aStopwatch;
      static Stopwatch LoadTimeStopwatch = new Stopwatch();
      public static readonly String StandardExtension = ".ptsTin";

      private void LoadTINfromVRML(string fileName)
      {
         string line;
         long lineCount = 0;
         if (!(String.Compare(Path.GetExtension(fileName), ".wrl", true) == 0))
         {
            throw new ArgumentException("Filename must have wrl extension.");
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
               scratchPoint = convertLineOfDataToPoint(line, ptIndex);
               if (allPoints == null)
               {
                  createAllpointsCollection();
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

            var allTriangles_ = new List<ptsDTMtriangle>();
            while ((line = file.ReadLine()) != null)
            {
               lineCount++;
               // Read until the close brace,  [
               if (line.Equals("]"))
                  break;
               scratchTriangle = convertLineOfDataToTriangle(line);
               allTriangles_.Add(scratchTriangle);
            }

            allTriangles_.Sort();
            allTriangles = allTriangles_.ToList();
         }
         finally
         {
            file.Close();
         }
      }

      private ptsDTMtriangle convertLineOfDataToTriangle(string line)
      {
         UInt32 ptIndex1, ptIndex2, ptIndex3;
         string[] parsed = line.Split(',');
         int correction = parsed.Length - 4;
         ptIndex1 = Convert.ToUInt32(parsed[0 + correction]);
         ptIndex2 = Convert.ToUInt32(parsed[1 + correction]);
         ptIndex3 = Convert.ToUInt32(parsed[2 + correction]);
         ptsDTMtriangle triangle = new ptsDTMtriangle(allPoints, ptIndex1, ptIndex2, ptIndex3);
         return triangle;
      }

      private ptsDTMpoint convertLineOfDataToPoint(string line, ulong pointIndex)
      {
         ptsDTMpoint newPt;
         string[] preParsedLine = line.Split(',');
         string[] parsedLine = preParsedLine[preParsedLine.Length-1].Split(' ');

         newPt = new ptsDTMpoint(
            pointIndex,
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

      /// <summary>
      /// Creates a tin file
      /// </summary>
      /// <param name="fileName"></param>
      /// <returns></returns>
      public static ptsDTM CreateFromExistingFile(string fileName)
      {
         ptsDTM returnTin = new ptsDTM();

         if(!String.IsNullOrEmpty(fileName))
         {
            String ext = Path.GetExtension(fileName);
            if (ext.Equals(StandardExtension, StringComparison.OrdinalIgnoreCase))
               returnTin = loadAsBinary(fileName);
            else
               returnTin.LoadTextFile(fileName);
         }

         return returnTin;
      }

      /// <summary>
      /// Loads tin from either LandXML or VRML file, depending on the extension passed in.
      /// </summary>
      /// <param name="fileName">Use .xml extension for LandXML. Use .wrl extension for VRML.</param>
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
            throw new ArgumentException("Filename must have xml extension.");
         }

         memoryUsed = GC.GetTotalMemory(true);
         Stopwatch stopwatch = new Stopwatch();
         List<string> trianglesAsStrings;
         setupStopWatches();

         scratchUIntPair = new UInt64pair();
         
         System.Console.WriteLine("Load XML document took:");
         stopwatch.Reset(); stopwatch.Start();
         LoadTimeStopwatch.Reset(); LoadTimeStopwatch.Start();
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
                        createAllpointsCollection();
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
            System.Console.WriteLine(allPoints.Count.ToString() + " Points Total.");
            
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
         //allTriangles = new List<ptsDTMtriangle>(trianglesAsStrings.Count);
         allTrianglesBag = new ConcurrentBag<ptsDTMtriangle>();
         Parallel.ForEach(trianglesAsStrings, refString =>
            {
               allTrianglesBag.Add(new ptsDTMtriangle(allPoints, refString));
            }
            );
         allTriangles = allTrianglesBag.OrderBy(triangle => triangle.point1.x).ToList();
         trianglesAsStrings = null; allTrianglesBag = null;
         GC.Collect(); GC.WaitForPendingFinalizers();
         memoryUsed = GC.GetTotalMemory(true) - memoryUsed;
         LoadTimeStopwatch.Stop();

         stopwatch.Stop();
         System.Console.WriteLine(allTriangles.Count().ToString() + " Total Triangles.");
         consoleOutStopwatch(stopwatch);
         
         //
         //System.Console.WriteLine("Indexing Triangles for adjacency took:");
         //stopwatch.Reset(); stopwatch.Start();
         //generateTriangleLineIndex();  start here
         //stopwatch.Stop(); consoleOutStopwatch(stopwatch);

      }

      private static ptsDTM loadAsBinary(string fileName)
      {
         if (!File.Exists(fileName))
            throw new FileNotFoundException(fileName);

         ptsDTM returnDTM = new ptsDTM();
         returnDTM.allPoints = new Dictionary<ulong, ptsDTMpoint>();
         returnDTM.allTriangles = new List<ptsDTMtriangle>();
         try
         {
            returnDTM.TryDeleteTempFiles();
            GetTempFilesOutOfZip(fileName);
            loadPointsFromBinary(returnDTM.allPoints);
            loadTrianglesFromBinary(returnDTM.allTriangles, returnDTM.allPoints);
            //Task.WaitAll(
            //   Task.Run(() => loadPointsFromBinary(returnDTM.allPoints)),
            //   Task.Run(() =>
            //      loadTrianglesFromBinary(returnDTM.allTriangles, returnDTM.allPoints))
            //   );

         }
         catch { }
         finally { returnDTM.TryDeleteTempFiles(); }
         return returnDTM;
      }

      private static void loadPointsFromBinary(Dictionary<ulong, ptsDTMpoint> allPointsDictionary)
      {
         Byte[] pointsBytes = File.ReadAllBytes(TEMP_POINTS_FILENAME);
         var arraySize = pointsBytes.Length;
         for(int arrayIndex=0; arrayIndex < arraySize; arrayIndex += ptsDTMpoint.getBinarySizeOf())
         {
            var newPoint = ptsDTMpoint.CreateFromBinary(pointsBytes, arrayIndex);
            ulong pointsIndex = newPoint.myIndex;
            allPointsDictionary.Add(pointsIndex, newPoint);
         }
      }

      private static void loadTrianglesFromBinary(
         List<ptsDTMtriangle> allTrianglesList,
         Dictionary<ulong, ptsDTMpoint> allPointsDictionary)
      {
         Byte[] trianglesBytes = File.ReadAllBytes(TEMP_TRIANGLES_FILENAME);
         var arraySize = trianglesBytes.Length;
         for(int arrayIndex=0; arrayIndex < arraySize; arrayIndex += ptsDTMtriangle.getBinarySize())
         {
            try
            {
               allTrianglesList.Add(
                  ptsDTMtriangle.CreateFromBinary(trianglesBytes, arrayIndex, allPointsDictionary));
            }
            catch { }
         }
      }

      private static void GetTempFilesOutOfZip(String fileToOpen)
      {
         using (var zipFile = ZipFile.Open(fileToOpen, ZipArchiveMode.Read))
         {
            zipFile.GetEntry(TEMP_POINTS_FILENAME).ExtractToFile(TEMP_POINTS_FILENAME);
            zipFile.GetEntry(TEMP_TRIANGLES_FILENAME).ExtractToFile(TEMP_TRIANGLES_FILENAME);
         }
      }

      public void saveAsBinary(string filenameToSaveTo, bool overwrite)
      {
         if (!Path.GetExtension(filenameToSaveTo).
            Equals(StandardExtension, StringComparison.OrdinalIgnoreCase))
         {
            throw new ArgumentException(
             String.Format("Filename does not have extension: {0}.", StandardExtension));
         }

         if(File.Exists(filenameToSaveTo) == true)
         {
            if(false == overwrite)
            {
               throw new IOException("File already exists and overwrite is disallowed.");
            }
            else
            {
               File.Delete(filenameToSaveTo);
            }
         }

         GC.Collect();
         try
         {
            TryDeleteTempFiles();
            Task.WaitAll(
               Task.Run(() => savePoints(TEMP_POINTS_FILENAME)),
               Task.Run(() => saveTriangles(TEMP_TRIANGLES_FILENAME))
               );
            putTempFilesInZip(filenameToSaveTo);
         }
         catch (Exception e) { }
         finally
         {
            TryDeleteTempFiles();
         }

         GC.Collect();
      }

      private void putTempFilesInZip(String fileToSave)
      {
         using (var zipFile = ZipFile.Open(fileToSave, ZipArchiveMode.Create))
         {
            zipFile.CreateEntryFromFile(TEMP_POINTS_FILENAME, TEMP_POINTS_FILENAME);
            zipFile.CreateEntryFromFile(TEMP_TRIANGLES_FILENAME, TEMP_TRIANGLES_FILENAME);
         }
      }

      private void TryDeleteTempFiles()
      {
         try
         {
            File.Delete(TEMP_POINTS_FILENAME);
         }
         catch { }
         try
         {
            File.Delete(TEMP_TRIANGLES_FILENAME);
         }
         catch { }
      }

      private void savePoints(String filenameToSaveTo)
      {
         Byte[] pointsByteArray = GetPointsAsByteArray();
         File.WriteAllBytes(filenameToSaveTo, pointsByteArray);
      }

      internal byte[] GetPointsAsByteArray()
      {
         int rowSize = sizeof(ulong) + (3 * sizeof(Double));
         int arraySize = allPoints.Count * (rowSize);
         Byte[] returnArray = new Byte[arraySize];
         int i = 0;
         int rowOffset = 0;
         foreach(var p in this.allPoints)
         {
            rowOffset = 0;
            Buffer.BlockCopy(
               BitConverter.GetBytes(p.Value.myIndex),
               0, returnArray, (i * rowSize) + rowOffset, sizeof(ulong));

            rowOffset += sizeof(ulong);
            Buffer.BlockCopy(
               BitConverter.GetBytes(p.Value.x),
               0, returnArray, (i * rowSize) + rowOffset, sizeof(Double));

            rowOffset += sizeof(ulong);
            Buffer.BlockCopy(
               BitConverter.GetBytes(p.Value.y),
               0, returnArray, (i * rowSize) + rowOffset, sizeof(Double));

            rowOffset += sizeof(ulong);
            Buffer.BlockCopy(
               BitConverter.GetBytes(p.Value.z),
               0, returnArray, (i * rowSize) + rowOffset, sizeof(Double));
            i++;
         }
         return returnArray;
      }

      private void saveTriangles(String filenameToSaveTo)
      {
         Byte[] trianglesByteArray = GetTrianglesAsByteArray();
         File.WriteAllBytes(filenameToSaveTo, trianglesByteArray);
      }

      private byte[] GetTrianglesAsByteArray()
      {
         int arraySize = allTriangles.Count * 3 * sizeof(UInt32);
         Byte[] returnArray = new Byte[arraySize];
         int i = 0;
         int actualOffset;
         foreach (var t in this.allTriangles)
         {
            try
            {
            actualOffset = i * sizeof(UInt32);
            Buffer.BlockCopy(
               BitConverter.GetBytes(t.indices[0]),
               0, returnArray, actualOffset, sizeof(UInt32));
            i++;
            actualOffset = i * sizeof(UInt32);
            Buffer.BlockCopy(
               BitConverter.GetBytes(t.indices[1]),
               0, returnArray, actualOffset, sizeof(UInt32));
            i++;
            actualOffset = i * sizeof(UInt32);
            Buffer.BlockCopy(
               BitConverter.GetBytes(t.indices[2]),
               0, returnArray, actualOffset, sizeof(UInt32));
            i++;

            }
            catch { }
         }
         return returnArray;
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
         return  (from ptsDTMtriangle triangle in allTriangles.AsParallel()
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

      private List<ptsDTMtriangle> localGroupTriangles;
      internal ptsDTMtriangle getTriangleContaining(ptsDTMpoint aPoint)
      {
         if (null == localGroupTriangles)
            localGroupTriangles = getTrianglesForPointInBB(aPoint).AsParallel().ToList();

         ptsDTMtriangle theTriangle = 
            localGroupTriangles.FirstOrDefault(aTrngl => aTrngl.contains(aPoint));

         if (null == theTriangle)
         {
            localGroupTriangles = getTrianglesForPointInBB(aPoint).AsParallel().ToList();
            theTriangle =
               localGroupTriangles.FirstOrDefault(aTrngl => aTrngl.contains(aPoint));
         }

         return theTriangle;
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
            String line;
            String[] values;
            while ((line = inputFile.ReadLine()) != null)
            {
               values = line.Split(',');
               if (values.Length != 3) continue;
               var newPt = new ptsDTMpoint(values[0], values[1], values[2]);
               GridDTMhelper.addPoint(newPt);
            }
         }
      }

      private void createAllpointsCollection()
      {
         allPoints = new Dictionary<UInt64, ptsDTMpoint>();
      }

      public String GenerateSizeSummaryString()
      {
         StringBuilder returnString = new StringBuilder();
         returnString.AppendLine(String.Format(
            "Points: {0:n0} ", allPoints.Count));
         returnString.AppendLine(String.Format("Triangles: {0:n0}", this.allTriangles.Count()));
         returnString.AppendLine(String.Format("Total Memory Used: Approx. {0:n0} MBytes",
            memoryUsed / (1028 * 1028)));
         returnString.AppendLine(String.Format(
            "{0:f4} Average Points per Triangle.",
            (Double)((Double)allPoints.Count / (Double)allTriangles.Count())));
         returnString.AppendLine(String.Format("Total Load Time: {0:f4} seconds",
            (Double) LoadTimeStopwatch.ElapsedMilliseconds / 1000.0));
         return returnString.ToString();
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
