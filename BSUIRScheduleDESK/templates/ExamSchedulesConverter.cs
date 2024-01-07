using BSUIRScheduleDESK.classes;
using System;
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
            return schedules!.GroupBy(x => x.dateLesson);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
