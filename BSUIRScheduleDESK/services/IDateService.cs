using System;
using System.Collections.Generic;

namespace BSUIRScheduleDESK.services
{
    public interface IDateService
    {
        List<DateTime> GetCurrentWeekDates();
        int GetWeekDiff(DateTime d1, DateTime d2, DayOfWeek startOfWeek = DayOfWeek.Monday);
    }
}
