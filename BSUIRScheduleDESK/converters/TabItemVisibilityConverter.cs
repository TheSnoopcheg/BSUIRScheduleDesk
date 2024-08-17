using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using BSUIRScheduleDESK.classes;

namespace BSUIRScheduleDESK.converters
{
    public class TabItemVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return Visibility.Collapsed;
            if( parameter == null ) return Visibility.Collapsed;
            if(value is GroupSchedule v)
            {
                string? param = parameter as string;
                if(param == "Exams")
                {
                    if (v.exams == null || v.exams.Count == 0) return Visibility.Collapsed;
                }
                else if(param == "Schedule")
                {
                    if(v.schedules == null) return Visibility.Collapsed;
                }
                else if(param == "PreviusSchedule")
                {
                    if(v.previousSchedules == null || v.previousTerm == v.currentTerm) return Visibility.Collapsed;
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
