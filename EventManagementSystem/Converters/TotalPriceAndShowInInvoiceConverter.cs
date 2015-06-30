using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using EventManagementSystem.Services;

namespace EventManagementSystem.Converters
{
    public class TotalPriceAndShowInInvoiceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool ShowOnInvoice = false;
            bool param = false;
            if (value != null && parameter != null)
            {
                if (Boolean.TryParse(value.ToString(), out ShowOnInvoice) && Boolean.TryParse(parameter.ToString(), out param))
                {
                    if (param && ShowOnInvoice)
                        return Visibility.Visible;
                    else if (!param && !ShowOnInvoice)
                        return Visibility.Visible;
                }
            }
            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
