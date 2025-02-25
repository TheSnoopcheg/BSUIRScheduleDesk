using BSUIRScheduleDESK.Classes;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace BSUIRScheduleDESK.Converters
{
    public class TabItemVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || parameter is not string param) return Visibility.Collapsed;
            if(value is Schedule v)
            {
                if(param == "Exams")
                {
                    if (v.exams == null || v.exams.Count == 0) return Visibility.Collapsed;
                }
                else if(param == "Schedule")
                {
                    if(v.dailyLessons == null || v.dailyLessons.Count == 0) return Visibility.Collapsed;
                }
                else if(param == "PreviousSchedule")
                {
                    if(v.previousDailyLessons == null || v.previousDailyLessons.Count == 0 || v.previousTerm == v.currentTerm) return Visibility.Collapsed;
                }
            }
            return Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
