using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ptsCogo
{
   public static class ExtensionMethods
   {
      public static LinkedList<T> ToLinkedList<T>(this IEnumerable<T> source)
      {
         if (source == null)
         {
            throw new ArgumentNullException("source");
         }
         return new LinkedList<T>(source);
      }
   }
}
