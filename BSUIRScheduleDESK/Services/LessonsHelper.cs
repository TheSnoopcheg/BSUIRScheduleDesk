using BSUIRScheduleDESK.Classes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BSUIRScheduleDESK.Services
{
    public static class LessonsHelper
    {
        public static int CheckLessonsCount(IEnumerable<Lesson> lessons, Lesson lesson, DateTime givenDate, int weekNum)
        {
            int result = 0;
            List<DateTime> dates = DateService.GetDatesOfWeekByDate(givenDate);
            var list = lessons
                .Where(l => l.subject == lesson.subject
                    && l.lessonTypeAbbrev == lesson.lessonTypeAbbrev
                    && l.numSubgroup == lesson.numSubgroup
                    && l.weekNumber != null
                    && Enumerable.SequenceEqual(l.studentGroups!, lesson.studentGroups!)
                    && Enumerable.SequenceEqual(l.employees!, lesson.employees!)
                    && DateTime.TryParse(l.startLessonDate, out _)
                    && TimeOnly.TryParse(l.startLessonTime, out _))
                .OrderBy(l => DateTime.Parse(l.startLessonDate!));

            if (!list.Any())
                return result;

            DateTime firstLessonDate = DateTime.Parse(list.First().startLessonDate!);

            if (givenDate < firstLessonDate)
                return result;
            
            Span<int> lessonPerWeek = stackalloc int[4]
            {
                list.Count(l => l.weekNumber!.Contains(1)),
                list.Count(l => l.weekNumber!.Contains(2)),
                list.Count(l => l.weekNumber!.Contains(3)),
                list.Count(l => l.weekNumber!.Contains(4))
            };

            // Getting the number of lessons for a given day
            result += list.Count((Func<Lesson, bool>)(l => (int)l.DayOfWeek + 1 == (int)givenDate.DayOfWeek
                                && TimeOnly.Parse(lesson.startLessonTime!) >= TimeOnly.Parse(l.startLessonTime!)
                                && l.weekNumber!.Contains(weekNum)));
            // Getting the number of lessons on a given week
            // Since we need to check the time for get the correct num for a given day
            // we also need to take into account that there may be lessons at a later time but days before
            // That's why we separate the logic for today and another part of the week
            result += list.Count((Func<Lesson, bool>)(l => (int)l.DayOfWeek + 1 < (int)givenDate.DayOfWeek
                                && l.weekNumber!.Contains(weekNum)
                                && DateTime.Parse(l.startLessonDate!) <= givenDate));

            if (dates.Contains(firstLessonDate))
                return result;

            while (true)
            {
                ChangeWeekDates(dates, -1);
                weekNum = GetNewWeekNum(weekNum, -1);
                if (dates.Contains(firstLessonDate))
                    break;
                result += lessonPerWeek[weekNum - 1];
            }

            // Getting the number of lessons for the first week
            // Since some lessons may contain weeknum of the first week but start later, we need to do this outside the loop
            result += list.Count((Func<Lesson, bool>)(l => (int)l.DayOfWeek + 1 <= (int)dates.Last().DayOfWeek
                                && l.weekNumber!.Contains(weekNum)
                                && DateTime.Parse(l.startLessonDate!) >= firstLessonDate));

            return result;
        }
        private static void ChangeWeekDates(List<DateTime> dates, int diff)
        {
            for(int i = 0; i < dates.Count; i++)
            {
                dates[i] = dates[i].AddDays(diff * 7);
            }
        }
        private static int GetNewWeekNum(int weekNum, int diff)
        {
            diff = diff % 4;
            return (weekNum + diff + 3) % 4 + 1;
        }
        private static int GetWeekDiff(DateTime d1, DateTime d2)
        {
            var diff = d2.Subtract(d1);

            var weeks = diff.Days / 7;

            return weeks;
        }
    }
}
