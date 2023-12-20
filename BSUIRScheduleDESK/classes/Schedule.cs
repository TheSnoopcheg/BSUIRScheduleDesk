using System.Collections.Generic;

namespace BSUIRScheduleDESK.classes
{
    public class Schedule
    {
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
    }
}
