using System.Collections.Generic;
using System.Linq;

namespace BSUIRScheduleDESK.Classes
{
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
        public override string ToString()
        {
            return $"{DayOfWeek.ToString()}: {subject} ({startLessonTime}{((weekNumber == null || weekNumber.Count < 1) ? string.Empty : "; weeks: " + string.Join(',', weekNumber))}{((employees == null || employees.Count < 1) ? string.Empty : "; " + employees[0])})";
        }
        public override bool Equals(object? obj)
        {
            if (obj is not Lesson lesson) return false;
            return this.subject == lesson.subject
                && this.startLessonTime == lesson.startLessonTime
                && this.numSubgroup == lesson.numSubgroup
                && this.lessonTypeAbbrev == lesson.lessonTypeAbbrev
                && this.DayOfWeek == lesson.DayOfWeek
                && ((this.employees == null && lesson.employees == null) || Enumerable.SequenceEqual(this.employees!, lesson.employees!))
                && ((this.studentGroups == null && lesson.studentGroups == null) || Enumerable.SequenceEqual(this.studentGroups!, lesson.studentGroups!))
                && ((this.weekNumber == null && lesson.weekNumber == null) || Enumerable.SequenceEqual(this.weekNumber!, lesson.weekNumber!));
        }
    }
}
