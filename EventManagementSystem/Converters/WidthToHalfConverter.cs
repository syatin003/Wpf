using System;
using System.Globalization;
using System.Windows.Data;

namespace EventManagementSystem.Converters
{
    public class WidthToHalfConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var actualWidth = (double)value;           
            return actualWidth/2;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
