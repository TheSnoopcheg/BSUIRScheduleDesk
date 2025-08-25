using BSUIRScheduleDESK.Classes;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace BSUIRScheduleDESK.Converters;

public class ExamSchedulesConverter : IValueConverter
{
    public static readonly ExamSchedulesConverter Instance = new ExamSchedulesConverter();
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value == null || value is not List<Lesson> schedules) return null;
        schedules = schedules!.OrderBy(s => DateTime.Parse(s.dateLesson!)).ThenBy(s => s.startLessonTime).ToList();
        return schedules!.GroupBy(x => x.dateLesson).GroupBy(x => DateTime.Parse(x.Key!), new MonthComparer());
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
