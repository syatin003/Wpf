using System;
using System.Windows;
using System.Windows.Data;

namespace EventManagementSystem.Converters
{
    public class CountToVisibilityConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value != null && value is Int32)
            {
                if (parameter != null && parameter.ToString() == "Invert")
                {
                    return ((Int32)value > 0 ? Visibility.Collapsed : Visibility.Visible);
                }

                else
                {
                    int result;
                    if (parameter != null && Int32.TryParse(parameter.ToString(), out result))
                    {
                        return ((Int32)value > result ? Visibility.Visible : Visibility.Collapsed);
                    }

                    else
                    {
                        return ((Int32)value > 0 ? Visibility.Visible : Visibility.Collapsed);
                    }
                }
            }
            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
