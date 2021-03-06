﻿using System;
using System.Globalization;
using System.Windows.Data;

namespace EventManagementSystem.Converters
{
    public class MetersToFeetConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (double) value*3.2808;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
