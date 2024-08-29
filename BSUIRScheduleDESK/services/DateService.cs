using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace BSUIRScheduleDESK.services
{
    public class DateService : IDateService
    {
        public List<DateTime> GetCurrentWeekDates()
        {
            DateTime today = DateTime.Today;
            CultureInfo culture = CultureInfo.CurrentCulture;
            int weekOffset = culture.DateTimeFormat.FirstDayOfWeek - today.DayOfWeek;
            if (today.DayOfWeek == 0)
                weekOffset -= 7;
            DateTime startOfWeek = today.AddDays(weekOffset);
            return Enumerable.Range(0, 6).Select(i => startOfWeek.AddDays(i)).ToList();
        } 

        public int GetWeekDiff(DateTime d1, DateTime d2, DayOfWeek startOfWeek = DayOfWeek.Monday)
        {
            var diff = d2.Subtract(d1);

            var weeks = (int)diff.Days / 7;

            var remainingDays = diff.Days % 7;
            var cal = CultureInfo.InvariantCulture.Calendar;
            var d1WeekNo = cal.GetWeekOfYear(d1, CalendarWeekRule.FirstFullWeek, startOfWeek);
            var d1PlusRemainingWeekNo = cal.GetWeekOfYear(d1.AddDays(remainingDays), CalendarWeekRule.FirstFullWeek, startOfWeek);

            if (d1WeekNo != d1PlusRemainingWeekNo)
                weeks++;

            return weeks;
        }
    }
}
