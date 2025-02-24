using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BSUIRScheduleDESK.Classes
{
    public class Schedule
    {
        [JsonPropertyName("employeeDto")]
        public Employee? employee { get; set; }
        [JsonPropertyName("studentGroupDto")]
        public StudentGroup? studentGroup { get; set; }
        [JsonPropertyName("schedules")]
        public Lessons? lessons { get; set; }
        [JsonPropertyName("previousSchedules")]
        public Lessons? previousLessons { get; set; }
        public List<DailyLesson> dailyLessons { get; set; } = new List<DailyLesson>();
        public List<DailyLesson> previousDailyLessons { get; set; } = new List<DailyLesson>();
        public string? currentTerm { get; set; }
        public string? previousTerm { get; set; }
        public List<Lesson>? exams { get; set; }
        public string? startDate { get; set; }
        public string? endDate { get; set; }
        public string? startExamsDate { get; set; }
        public string? endExamsDate { get; set; }
        [JsonIgnore]
        public bool favorited { get; set; }
        public string? currentPeriod { get; set; }
        public bool Compare(Schedule right)
        {
            string thisObj = JsonSerializer.Serialize(this);
            string rightObj = JsonSerializer.Serialize(right);
            return thisObj.Equals(rightObj);
        }
        public string? GetName()
        {
            return employee == null ? studentGroup?.name : employee.ToString();
        }
        public string? GetUrl()
        {
            return employee == null ? studentGroup?.name : employee.urlId;
        }
        public override string ToString()
        {
            return GetName() ?? string.Empty;
        }
        public async Task CreateDailyLessonConllections() => await Task.Run(() =>
        {
            if(lessons != null && !lessons.IsEmpty)
            {
                int counter = 0;
                foreach(var prop in lessons.GetType().GetProperties())
                {
                    if(prop.GetValue(lessons) is not IEnumerable<Lesson> list || list.Count() == 0)
                    {
                        counter++;
                        continue;
                    }

                    foreach(var item in list)
                    {
                        dailyLessons.Add(new DailyLesson { Day = (Day)counter, Lesson = item });
                    }
                    counter++;
                }
                lessons = null;
            }

            if(previousLessons != null && !previousLessons.IsEmpty)
            {
                int counter = 0;
                foreach(var prop in previousLessons.GetType().GetProperties())
                {
                    if(prop.GetValue(previousLessons) is not IEnumerable<Lesson> list || list.Count() == 0)
                    {
                        counter++;
                        continue;
                    }

                    foreach(var item in list)
                    {
                        previousDailyLessons.Add(new DailyLesson { Day = (Day)counter, Lesson = item });
                    }
                    counter++;
                }
                previousLessons = null;
            }
        });
    }
}
