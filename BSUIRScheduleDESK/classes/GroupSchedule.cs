using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace BSUIRScheduleDESK.classes
{
    public class GroupSchedule
    {
        [JsonPropertyName("employeeDto")]
        public Employee? employee { get; set; }
        [JsonPropertyName("studentGroupDto")]
        public StudentGroup? studentGroup { get; set; }
        public Schedules? schedules { get; set; }
        public Schedules? previousSchedules { get; set; }
        public string? currentTerm { get; set; }
        public string? previousTerm { get; set; }
        public List<Schedule>? exams { get; set; }
        public string? startDate { get; set; }
        public string? endDate { get; set; }
        public string? startExamsDate { get; set; }
        public string? endExamsDate { get; set; }
        public bool favorited { get; set; }
        public UpdateDate? updateDate { get; set; }
        public string? currentPeriod { get; set; }
        public bool Compare(GroupSchedule right)
        {
            string? lOp = this.studentGroup == null ? this.employee!.urlId : this.studentGroup.name;
            string? rOp = right.studentGroup == null ? right.employee!.urlId : right.studentGroup.name;
            return lOp == rOp;
        }
        public string? GetName()
        {
            return employee == null ? studentGroup?.name : employee.ToString();
        }
        public string? GetUrl()
        {
            return employee == null ? studentGroup?.name : employee.urlId;
        }
    }
    public record class UpdateDate
    {
        public string? lastUpdateDate { get; set; }
    }
}
