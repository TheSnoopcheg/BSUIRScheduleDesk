using System;
using System.Collections.Generic;
using System.Linq;

namespace BSUIRScheduleDESK.Classes;

public class Lesson
{
    public Day DayOfWeek { get; set; }
    public List<int>? weekNumber { get; set; }
    public List<StudentGroup>? studentGroups { get; set; }
    public int numSubgroup { get; set; }
    public List<string>? auditories { get; set; }
    public string? startLessonTime { get; set; }
    public string? endLessonTime { get; set; }
    public string? subject { get; set; }
    public string? subjectFullName { get; set; }
    public string? note { get; set; }
    public string? lessonTypeAbbrev { get; set; }
    public string? dateLesson { get; set; }
    public string? startLessonDate { get; set; }
    public string? endLessonDate { get; set; }
    public string? announcementStart { get; set; }
    public string? announcementEnd { get; set; }
    public bool announcement { get; set; }
    public bool split { get; set; }
    public List<Employee>? employees { get; set; }

    public override bool Equals(object? obj)
    {
        return obj is Lesson lesson &&
               DayOfWeek == lesson.DayOfWeek &&
               EqualityComparer<List<int>?>.Default.Equals(weekNumber, lesson.weekNumber) &&
               EqualityComparer<List<StudentGroup>?>.Default.Equals(studentGroups, lesson.studentGroups) &&
               numSubgroup == lesson.numSubgroup &&
               startLessonTime == lesson.startLessonTime &&
               subject == lesson.subject &&
               lessonTypeAbbrev == lesson.lessonTypeAbbrev &&
               EqualityComparer<List<Employee>?>.Default.Equals(employees, lesson.employees);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(DayOfWeek, weekNumber, studentGroups, numSubgroup, startLessonTime, subject, lessonTypeAbbrev, employees);
    }

    public override string ToString()
    {
        return $"{DayOfWeek.ToString()}: {subject} ({startLessonTime}{((weekNumber == null || weekNumber.Count < 1) ? string.Empty : "; weeks: " + string.Join(',', weekNumber))}{((employees == null || employees.Count < 1) ? string.Empty : "; " + employees[0])})";
    }
}
