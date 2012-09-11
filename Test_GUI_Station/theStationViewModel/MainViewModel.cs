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
   public class MainViewmodel : INotifyPropertyChanged
   {

      public MainViewmodel()
      {
         _theModel = new ModelOfCogoStation();
         _theModel.Load();
         canAdd = true;
         addToStation = new RelayCommand(addToStationCommand, () => canAdd);
         addend = 50.0;
         // theModel.aStation
      }

      private ModelOfCogoStation _theModel;
      public ModelOfCogoStation TheModel
      {
         get { return _theModel; }
         set
         {
            if (_theModel != value)
            {
               _theModel = value;
               RaisePropertyChanged("TheModel");
            }
         }
      }

      public CogoStation aStation
      {
         get { return TheModel.aStation; }
         set
         {
            if (TheModel.aStation != value)
            {
               TheModel.aStation = value;
               RaisePropertyChanged("aStation");
            }
         }
      }

      private double _addend;
      public double addend
      {
         get { return _addend; }
         set
         {
            if (_addend != value)
            {
               _addend = value;
               RaisePropertyChanged("addend");
            }
         }
      }

      private bool canAdd;
      public ICommand addToStation { get; private set; }
      private void addToStationCommand()
      {
         aStation += addend;
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

   public class StationConverter : System.Windows.Data.IValueConverter
   {
      /// <summary>
      /// Convert from CogoStation to String
      /// </summary>
      /// <param name="value"></param>
      /// <param name="targetType"></param>
      /// <param name="parameter"></param>
      /// <param name="culture"></param>
      /// <returns></returns>
      public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
      {
         return (value as CogoStation).ToString();
      }

      /// <summary>
      /// Convert from String to CogoStation
      /// </summary>
      /// <param name="value"></param>
      /// <param name="targetType"></param>
      /// <param name="parameter"></param>
      /// <param name="culture"></param>
      /// <returns></returns>
      public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
      {
         return CogoStation.convertStringToStation(value as String) as CogoStation;
      }
   }
}
