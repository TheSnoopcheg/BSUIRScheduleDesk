using System;
using System.Globalization;
using System.Windows.Data;

namespace BSUIRScheduleDESK.converters
{
    internal class NegativeValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return default;
            if(value is double v)
            {
                return -v;
            }
            return default;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
