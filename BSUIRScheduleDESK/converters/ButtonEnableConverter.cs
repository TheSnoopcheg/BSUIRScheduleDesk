using System;
using System.Globalization;
using System.Windows.Data;

namespace BSUIRScheduleDESK.Converters
{
    internal class ButtonEnableConverter : IValueConverter, IMultiValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is not int selectedIndex) return false;
            if (selectedIndex == -1) return false;
            return true;
        }

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values == null 
                || values.Length < 3 
                || values[0] is not DateTime selectedDate 
                || values[1] is not string text 
                || values[2] is not bool isEditing) return false;
            if (text == string.Empty) return false;
            if (isEditing == true) return false;
            return true;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
