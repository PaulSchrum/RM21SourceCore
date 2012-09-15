using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using rm21Core.ExternalClasses;
using rm21Core;

namespace MainRM21WPFapp.ViewModels
{
   public class MainWindowVM : ViewModelBase
   {
      public MainWindowVM()
      {
         theRM21model = new rm21Model();
         theRM21model.allCorridors.Add(new rm21Corridor("-L-"));
         theRM21model.allCorridors.Add(new rm21Corridor("-Y1-"));
         theRM21model.allCorridors.Add(new rm21Corridor("-C1-"));
      }

      private rm21Model theRM21model_;
      public rm21Model theRM21model
      {
         get { return theRM21model_; }
         set
         {
            if (theRM21model_ != value)
            {
               theRM21model_ = value;
               RaisePropertyChanged("theRM21model");
            }
         }
      }

      private rm21Corridor currentCorridor_;
      public rm21Corridor CurrentCorridor
      {
         get { return currentCorridor_; }
         set
         {
            if (currentCorridor_ != value)
            {
               currentCorridor_ = value;
               RaisePropertyChanged("CurrentCorridor");
            }
         }
      }

   }

   public class CorridorNameConverter : System.Windows.Data.IValueConverter
   {
      /// <summary>
      /// Convert from rm21Corridor to String
      /// </summary>
      /// <param name="value"></param>
      /// <param name="targetType"></param>
      /// <param name="parameter"></param>
      /// <param name="culture"></param>
      /// <returns></returns>
      public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
      {
         return (value as rm21Corridor).Name;
      }

      /// <summary>
      /// Convert from String to rm21Corridor 
      /// </summary>
      /// <param name="value"></param>
      /// <param name="targetType"></param>
      /// <param name="parameter"></param>
      /// <param name="culture"></param>
      /// <returns></returns>
      public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
      {
         return null;
      }
   }
}
