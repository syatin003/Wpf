using System;
using System.Windows;
using System.Windows.Data;

namespace EventManagementSystem.Converters
{
    public class NullToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is string)
            {
                return (string.IsNullOrEmpty((string)value) || string.IsNullOrWhiteSpace((string)value)) ? Visibility.Collapsed : Visibility.Visible;
            }

            if (parameter != null && parameter.ToString() == "Invert")
                return value == null ? Visibility.Visible : Visibility.Collapsed;

            return value == null ? Visibility.Collapsed : Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
