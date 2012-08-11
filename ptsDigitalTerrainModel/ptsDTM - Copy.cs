using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;

namespace ptsDigitalTerrainModel
{
   public class ptsDTM
   {
      // Substantive members - Do serialize
      private Dictionary<UInt64, ptsDTMpoint> allPoints;
      private List<ptsDTMtriangle> allTriangles_genList;
      private ptsDTMtriangle[] allTriangles;

      // temp scratch pad members -- do not serialize
      private ptsDTMpoint scratchPoint;
      private ptsDTMtriangle scratchTriangle;

      private Dictionary<string, Stopwatch> stpWatches;

      // Constructor to construct from an xml file
      public ptsDTM(string fileName)
      {
         if (!(String.Compare(Path.GetExtension(fileName), "xml", true) == 0))
         {
            //throw new notAnXMLfileException();
         }

         setupStopWatches();

         System.Console.WriteLine("");
         System.Console.WriteLine("Load XML document");
         XDocument tinXMLdoc = XDocument.Load(fileName);
         System.Console.WriteLine("Seeking Pnts collection");
         XElement root = tinXMLdoc.Elements().ToList<XElement>()[0];
         List<XElement> allXelements = root.Elements().ToList<XElement>();
         foreach (XElement anElement in allXelements)
         {
            if (String.Compare(anElement.Name.ToString(), "Surfaces") == 0)
            {
               System.Console.WriteLine("Found Surfaces Collection");
               XElement surfacesXE = anElement;
               XElement srface = surfacesXE.Elements().ToList<XElement>()[0];
               List<XElement> srfcElements = srface.Elements().ToList<XElement>();
               foreach (XElement aSubElement in srfcElements)
               {
                  if (String.Compare(aSubElement.Name.ToString(), "Definition") == 0)
                  {
                     List<XElement> dfinitionElements = aSubElement.Elements().ToList<XElement>();
                     foreach (XElement subDef in dfinitionElements)
                     {
                        System.Console.WriteLine(subDef.Name);
                        if (String.Compare(subDef.Name.ToString(), "Pnts") == 0)
                        {
                           System.Console.WriteLine("Found Pnts collection");
                           stpWatches["Process Points"].Start();
                           List<XElement> pnts = subDef.Elements().ToList<XElement>();
                           foreach (XElement pnt in pnts)
                           {
                              scratchPoint = new ptsDTMpoint(pnt.Value);
                              UInt64 index;
                              UInt64.TryParse(pnt.FirstAttribute.Value, out index);
                              if (null == allPoints)
                                 allPoints = new Dictionary<UInt64, ptsDTMpoint>();
                              allPoints.Add(index, scratchPoint);
                           }
                           pnts = null;
                           GC.Collect();   GC.WaitForPendingFinalizers();
                           stpWatches["Process Points"].Stop();
                           System.Console.WriteLine("Processing Points took:");
                           consoleOutStopwatch(stpWatches["Process Points"]);
                        }
                        if (String.Compare(subDef.Name.ToString(), "Faces") == 0)
                        {
                           ulong count;
                           count = 0;
                           stpWatches["Process Triangles"].Start();
                           List<XElement> faces = subDef.Elements().ToList<XElement>();
                           foreach (XElement aFace in faces)
                           {
                              count++;
                              scratchTriangle = new ptsDTMtriangle(allPoints, aFace.Value.ToString());
                              if (null == allTriangles_genList)
                                 allTriangles_genList = new List<ptsDTMtriangle>();
                              allTriangles_genList.Add(scratchTriangle);
                              if ((count % 100000) == 0) System.Console.WriteLine("{0} Triangles so far.", count);
                              //To Do: start here: Add function to get Indexes of points
                              //    and assign to each triangle
                              // or just hold the indices for now -- not sure -- sleep on it
                           }
                           System.Console.WriteLine("Total Triangles = {0}", count);
                           System.Console.WriteLine("Freeing Faces collection");
                           faces = null;
                           GC.Collect();   GC.WaitForPendingFinalizers();
                           System.Console.WriteLine("Copying triangle list to array");
                           allTriangles = allTriangles_genList.ToArray<ptsDTMtriangle>();
                           System.Console.WriteLine("Freeing triangle list");
                           allTriangles_genList = null;
                           GC.Collect();   GC.WaitForPendingFinalizers();
                           
                           System.Console.WriteLine("Sorting Triangle Array in x");
                           Array.Sort(allTriangles);
                           
                           System.Console.WriteLine("Sorting Triangle Array in y");
                           //not yet implemented

                           stpWatches["Process Triangles"].Stop();
                           System.Console.WriteLine("Processing Triangles took:");
                           consoleOutStopwatch(stpWatches["Process Triangles"]);
                        }

                        // all processing is complete by this point
                     }
                  }
               }
            }
         }
         tinXMLdoc = null;
         GC.Collect();   GC.WaitForPendingFinalizers();
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
      }
   }

   public class notAnXMLfileException : Exception
   {
      notAnXMLfileException() : base("File is not an XML file. Can not construct DTM from this.")
      {}
   }
}
