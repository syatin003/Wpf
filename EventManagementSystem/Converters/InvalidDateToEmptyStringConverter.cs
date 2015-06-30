using System;
using System.Globalization;
using System.Windows.Data;

namespace EventManagementSystem.Converters
{
    public class InvalidDateToEmptyStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var dateValue = (DateTime?)value;

            if (dateValue == null) return String.Empty;
            if (dateValue == default(DateTime)) return String.Empty;

            return dateValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var dateValue = (DateTime?)value;

            if (dateValue == null) return String.Empty;
            if (dateValue == default(DateTime)) return String.Empty;

            return dateValue;
        }
    }
}
