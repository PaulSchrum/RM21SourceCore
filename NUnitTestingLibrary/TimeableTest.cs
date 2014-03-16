using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace NUnitTestingLibrary
{
   public class TimeableTest
   {
      private Stopwatch sw = new Stopwatch();
      public Stopwatch stopwatch { get { return sw; } private set { } }

      public void timerStart()
      {
         this.stopwatch.Start();
      }

      public void timerStopAndPrint( 
         [CallerLineNumber] int lineNumber=0,
         [CallerMemberName] string mbrName=null
         )
      {
         stopwatch.Stop();
         System.Console.WriteLine("Timespan: " + 
            stopwatch.Elapsed.ToString() + " Line " +
            lineNumber.ToString() + " of " +
            mbrName);
         stopwatch.Reset(); stopwatch.Start();
      }
   }
}
