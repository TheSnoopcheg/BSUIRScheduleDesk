using BSUIRScheduleDESK.classes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace BSUIRScheduleDESK.templates
{
    public class ExamSchedulesConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return null;
            List<Schedule>? schedules = value as List<Schedule>;
            schedules = schedules!.OrderBy(s => DateTime.Parse(s.dateLesson!)).ThenBy(s => s.startLessonTime).ToList();
            return schedules!.GroupBy(x => x.dateLesson).GroupBy(x => DateTime.Parse(x.Key!), new MonthComparer());
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
