using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace BSUIRScheduleDESK.Services
{
    public static class DateService
    {
        public static List<DateTime> GetCurrentWeekDates()
        {
            return GetDatesOfWeekByDate(DateTime.Today);
        }

        public static int GetWeekDiff(DateTime d1, DateTime d2, DayOfWeek startOfWeek = DayOfWeek.Monday)
        {
            var diff = d2.Subtract(d1);

            var weeks = diff.Days / 7;

            var remainingDays = diff.Days % 7;
            var cal = CultureInfo.InvariantCulture.Calendar;
            var d1WeekNo = cal.GetWeekOfYear(d1, CalendarWeekRule.FirstFullWeek, startOfWeek);
            var d1PlusRemainingWeekNo = cal.GetWeekOfYear(d1.AddDays(remainingDays), CalendarWeekRule.FirstFullWeek, startOfWeek);

            if (d1WeekNo != d1PlusRemainingWeekNo)
                weeks++;

            return weeks;

        }
        public static List<DateTime> GetDatesOfWeekByDate(DateTime date)
        {
            CultureInfo culture = CultureInfo.CurrentCulture;
            int weekOffset = culture.DateTimeFormat.FirstDayOfWeek - date.DayOfWeek;
            if (date.DayOfWeek == 0)
                weekOffset -= 7;
            DateTime startOfWeek = date.AddDays(weekOffset);
            return Enumerable.Range(0, 6).Select(i => startOfWeek.AddDays(i)).ToList();
        }
    }
}