using System;
using System.Globalization;
using System.Windows.Data;

namespace BSUIRScheduleDESK.templates
{
    public class CenterXConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return 0;
            if(value is double v)
            {
                return v / 2;
            }
            return 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
