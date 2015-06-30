using System;
using System.Globalization;
using System.Windows.Data;

namespace EventManagementSystem.Converters
{
    public class DiscountConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            double retailPrice = 0;
            double salePrice = 0;

            double.TryParse(values[0].ToString(), out retailPrice);
            double.TryParse(values[1].ToString(), out salePrice);

            double discount = retailPrice - salePrice;

            return discount.ToString("C");
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}