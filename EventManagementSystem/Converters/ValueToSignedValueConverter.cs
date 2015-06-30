using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace EventManagementSystem.Converters
{
    public class ValueToSignedValueConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            Int32 val = (Int32)value;

            if (val == 0) return "+0";
            if (val > 0) return "+" + val.ToString();

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {

            Int32 val;

            if (int.TryParse(value.ToString(), out val))
            {
                if (val == 0) return value;
                if (val > 0) return "+" + val.ToString();
            }

            return value;
        }
    }
}
