using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace ptsCogo.ExternalClasses
{
   /// <summary>
   /// Written by Paul Schrum, but inspired by Joel Cochran in
   /// http://www.youtube.com/watch?v=BClf7GZR0DQ
   /// </summary>
   public abstract class ViewModelBase : INotifyPropertyChanged
   {

      /// <summary>
      /// Raises the property changed event.
      /// </summary>
      /// <param name="propertyName">Name of the property.</param>
      protected void RaisePropertyChanged(string propertyName)
      {
         var handler = this.PropertyChanged;
         if (handler != null)
         {
            handler(this, new PropertyChangedEventArgs(propertyName));
         }
      }

      /// <summary>
      /// Occurs when a property value changes.
      /// </summary>
      public event PropertyChangedEventHandler PropertyChanged;
   }
}
