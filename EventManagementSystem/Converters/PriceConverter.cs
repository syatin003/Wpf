using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using EventManagementSystem.Data.Model;

namespace EventManagementSystem.Converters
{
    public class PriceConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            double retailPrice = 0;

            var product = (TillProduct)values[0];

            if (product.Price1L1 == null && product.Price1L2 == null && product.Price1L3 == null &&
                product.Price1L4 == null
                && product.Price2L1 == null && product.Price2L2 == null && product.Price2L3 == null &&
                product.Price2L4 == null
                && product.Price3L1 == null && product.Price3L2 == null && product.Price3L3 == null &&
                product.Price3L4 == null)
                return string.Empty;
            var details = (IEnumerable<TillTransactionDetail>)values[1];
            var detail = details.FirstOrDefault(det => det.File == 1 & det.ItemRecord == product.Record);

            if (values.Any(v => v == null))
                return string.Empty;

            if (Math.Abs(detail.Quantity) == product.Quantity1)
            {
                switch (detail.PriceLevel)
                {
                    case 1:
                        retailPrice = (double)product.Price1L1;
                        break;
                    case 2:
                        retailPrice = (double)product.Price1L2;
                        break;
                    case 3:
                        retailPrice = (double)product.Price1L3;
                        break;
                    case 4:
                        retailPrice = (double)product.Price1L4;
                        break;
                }
            }
            else if (Math.Abs(detail.Quantity) == product.Quantity2)
            {
                switch (detail.PriceLevel)
                {
                    case 1:
                        retailPrice = (double)product.Price2L1;
                        break;
                    case 2:
                        retailPrice = (double)product.Price2L2;
                        break;
                    case 3:
                        retailPrice = (double)product.Price2L3;
                        break;
                    case 4:
                        retailPrice = (double)product.Price2L4;
                        break;
                }
            }
            else
            {
                switch (detail.PriceLevel)
                {
                    case 1:
                        retailPrice = (double)product.Price3L1;
                        break;
                    case 2:
                        retailPrice = (double)product.Price3L2;
                        break;
                    case 3:
                        retailPrice = (double)product.Price3L3;
                        break;
                    case 4:
                        retailPrice = (double)product.Price3L4;
                        break;
                }
            }

            if (detail.Quantity < 0)
                retailPrice *= -1;

            return retailPrice.ToString("C");
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
