using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BSUIRScheduleDESK.Classes;

public class Schedule
{
    public string? startDate { get; set; }
    public string? endDate { get; set; }
    public string? startExamsDate { get; set; }
    public string? endExamsDate { get; set; }

    [JsonPropertyName("employeeDto")]
    public Employee? employee { get; set; }
    [JsonPropertyName("studentGroupDto")]
    public StudentGroup? studentGroup { get; set; }
    [JsonPropertyName("schedules")]
    public Lessons? lessons { get; set; }
    [JsonPropertyName("nextSchedules")]
    public Lessons? nextLessons { get; set; }
    public string? currentTerm { get; set; }
    public string? nextTerm { get; set; }
    public List<Lesson>? exams { get; set; }
    public string? currentPeriod { get; set; }
    public bool? isZaochOrDist { get; set; }
    
    [JsonIgnore]
    public bool favorited { get; set; }
    public List<Lesson> dailyLessons { get; set; } = [];
    public List<Lesson> nextDailyLessons { get; set; } = [];
    public bool Compare(Schedule? right)
    {
        if (right is null)
            return false;
        JsonElement thisObj = JsonSerializer.SerializeToElement(this);
        JsonElement rightObj = JsonSerializer.SerializeToElement(right);
        return JsonElement.DeepEquals(thisObj, rightObj);
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
        nextDailyLessons.Clear();

        AddLessonsByDay(lessons, dailyLessons);
        AddLessonsByDay(nextLessons, nextDailyLessons);

        lessons = null;
        nextLessons = null;
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
