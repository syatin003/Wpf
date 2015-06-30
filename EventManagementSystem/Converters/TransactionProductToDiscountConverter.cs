using System;
using System.Globalization;
using System.Windows.Data;
using EventManagementSystem.Data.Model;

namespace EventManagementSystem.Converters
{
    public class TransactionProductToDiscountConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return String.Empty;
            double discount = 0;
            var viewFrom = (string) parameter;

            if (String.Equals(viewFrom, "transaction"))
            {
                var product = (TillTransactionProduct) value;
                if (product.SalePrice != null)
                {
                    double quantity = Math.Abs(product.Quantity);
                    if (product.TillProduct.Quantity1 != 1)
                        quantity = (double) (product.Value / product.SalePrice);
                    discount = (double) (product.SalePrice * (decimal)quantity - product.Value);
                }
            }
            else if(String.Equals(viewFrom, "product"))
            {
                var product = (dynamic)value;
                if (product.SalePrice != null)
                {
                    double quantity = Math.Abs(product.Quantity);
                    if (product.TillProduct.Quantity1 != 1)
                        quantity = product.Value / product.SalePrice;
                    discount = product.SalePrice * quantity - product.Value;
                }
            }
           
            return discount.ToString("C");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}