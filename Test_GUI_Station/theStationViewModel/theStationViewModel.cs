using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using Test_GUI_Station.theStationModel;
using ptsCogo;
using System.Windows.Input;


namespace Test_GUI_Station.theStationViewModel
{
   public class theStationViewModel : INotifyPropertyChanged 
   {
      private ModelOfCogoStation theModel;

      public theStationViewModel() { }

      public void setModel(ModelOfCogoStation aModel)
      {
         theModel = aModel;
         //theModel.Load();
         aStation = theModel.aStation;
      }

      public CogoStation aStation
      {
         get { return this.theModel.aStation; }
         set
         {
            if (this.theModel.aStation != value)
            {
               this.theModel.aStation = value;
               this.RaisePropertyChanged("aStation");
            }
         }
      }

      /// <summary>
      /// Raises the property changed event.
      /// </summary>
      /// <param name="propertyName">Name of the property.</param>
      private void RaisePropertyChanged(string propertyName)
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
