using System;
using System.Globalization;
using System.Windows.Data;

namespace BSUIRScheduleDESK.Converters
{
    public class CenterTransformConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is not double v) return 0; 
            return v / 2;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
