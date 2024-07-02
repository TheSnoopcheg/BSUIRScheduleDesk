using System;
using System.Globalization;
using System.Windows.Data;
using System.Collections.Generic;

namespace BSUIRScheduleDESK.converters
{
    public class WeekContainsConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values[0] is not List<int> weeks) return false;
            if(values[1] is not int week) return false;
            return weeks.Contains(week);
        }

        public object[] ConvertBack(object value, Type[] targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
