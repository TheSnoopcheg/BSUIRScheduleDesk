using System;
using System.Globalization;
using System.Windows.Data;

namespace BSUIRScheduleDESK.Converters
{
    public class IsMonthExpandedConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is not DateTime date) return false;
            return date.Month == DateTime.Today.Month;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
