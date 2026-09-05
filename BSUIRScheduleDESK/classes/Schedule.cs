using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BSUIRScheduleDESK.Classes;

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
    public List<Lesson> dailyLessons { get; set; } = new List<Lesson>();
    public List<Lesson> previousDailyLessons { get; set; } = new List<Lesson>();
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
    public void CreateDailyLessonsCollections()
    {
        dailyLessons.Clear();
        previousDailyLessons.Clear();

        AddLessonsByDay(lessons, dailyLessons);
        AddLessonsByDay(previousLessons, previousDailyLessons);
    }
    private static void AddLessonsByDay(Lessons? source, List<Lesson> destination)
    {
        if (source is null)
            return;

        foreach(var (day, dayLessons) in source.ByDay)
        {
            foreach(var lesson in dayLessons)
            {
                if (lesson is null) 
                    continue;

                lesson.DayOfWeek = day;
                destination.Add(lesson);
            }
        }
    }
    public async Task CreateDailyLessonCollectionsAsync() => await Task.Run(CreateDailyLessonsCollections);
}
