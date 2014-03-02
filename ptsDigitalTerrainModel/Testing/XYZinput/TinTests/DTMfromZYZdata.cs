using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ptsDigitalTerrainModel;

namespace TinTests
{
   [TestClass]
   public class DTMfromZYZdata
   {
      private String pathRM21SourceCode;
      [TestInitialize]
      public void setup()
      {
         pathRM21SourceCode = AppDomain.CurrentDomain.BaseDirectory + @"\..\..\..\";
      }

      private ptsDTM asheboroDTM = null;
      private bool AsheboroDataLoaded = false;
      private void loadAsheboroData()
      {
         if (AsheboroDataLoaded == false)
         {
            AsheboroDataLoaded = true;

            String fileToOpen = pathRM21SourceCode +
//               @"\ptsDigitalTerrainModel\Testing\XYZinput\" +
               "RandolphSmallSample.xyz.txt";

            asheboroDTM = new ptsDTM();
            asheboroDTM.loadFromXYZtextFile(fileToOpen);
         }
      }

      [TestMethod]
      public void SmallDataset_createsCorrectDTM()
      {
         loadAsheboroData();
      }
   }
}
