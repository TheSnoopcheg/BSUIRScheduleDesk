using BSUIRScheduleDESK.classes;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace BSUIRScheduleDESK.converters
{
    public class ToolTipVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var empl = value as Employee;
            var studGroup = value as StudentGroup;
            if (empl != null)
            {
                return empl.lastName == null ? Visibility.Collapsed : Visibility.Visible;
            }
            if(studGroup != null)
            {
                return studGroup.numberOfStudents == 0 ? Visibility.Collapsed : Visibility.Visible;
            }
            return Visibility.Collapsed;

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
