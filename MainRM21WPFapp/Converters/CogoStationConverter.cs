using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using ptsCogo;

namespace MainRM21WPFapp.Converters
{
   public class CogoStationConverter : IValueConverter
   {
      public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
      {
         return CogoStation.convertStringToStation((String) value);
      }

      public object Convert (object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
      {
         return ((CogoStation)value).ToString();
      }
   }
}
