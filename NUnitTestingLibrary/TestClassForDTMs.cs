using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using ptsCogo;
using ptsDigitalTerrainModel;
using ptsCogo.Angle;
using ptsCogo.coordinates;


namespace NUnitTestingLibrary
{
   [TestFixture]
   class TestClassForDTMs
   {
      private ptsDTM aDTM;
      private string pathRM21SourceCode;
      private string NUnitTestingData;
      private string vrmlTestFileName;
      private string fullname;

      [SetUp]
      public void setupDTMtests()
      {
         pathRM21SourceCode = AppDomain.CurrentDomain.BaseDirectory + @"..\..\..\";
         NUnitTestingData = pathRM21SourceCode + @"NUnitTestingLibrary\TestingData\";
         vrmlTestFileName = "B4656_rm21_mesh_test.wrl";
         fullname = NUnitTestingData + vrmlTestFileName;

         // verify that the file exists


         aDTM = new ptsDTM();
         aDTM.LoadTextFile(NUnitTestingData + vrmlTestFileName);
      }

      [Test]
      public void FakeTest()
      {
         int i = 0;
         Assert.True(true);
      }

   }
}
