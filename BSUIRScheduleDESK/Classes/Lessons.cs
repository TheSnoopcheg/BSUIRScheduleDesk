using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace BSUIRScheduleDESK.Classes;

public class Lessons
{
    [JsonPropertyName("Понедельник")]
    public List<Lesson>? Monday { get; set; }
    [JsonPropertyName("Вторник")]
    public List<Lesson>? Tuesday { get; set; }
    [JsonPropertyName("Среда")]
    public List<Lesson>? Wednesday { get; set; }
    [JsonPropertyName("Четверг")]
    public List<Lesson>? Thursday { get; set; }
    [JsonPropertyName("Пятница")]
    public List<Lesson>? Friday { get; set; }
    [JsonPropertyName("Суббота")]
    public List<Lesson>? Saturday { get; set; }
    [JsonPropertyName("Воскресенье")]
    public List<Lesson>? Sunday { get; set; }
    public override string ToString() => "Lessons";
    public bool IsEmpty =>
        Monday is not { Count: > 0 } &&
        Tuesday is not { Count: > 0 } &&
        Wednesday is not { Count: > 0 } &&
        Thursday is not { Count: > 0 } &&
        Friday is not { Count: > 0 } &&
        Saturday is not { Count: > 0 } &&
        Sunday is not { Count: > 0 };
    [JsonIgnore]
    public IEnumerable<(Day Day, List<Lesson> Lessons)> ByDay
    {
        get
        {
            if (Monday is { Count: > 0 }) 
                yield return (Day.Monday, Monday);
            if (Tuesday is { Count: > 0 }) 
                yield return (Day.Tuesday, Tuesday);
            if (Wednesday is { Count: > 0 }) 
                yield return (Day.Wednesday, Wednesday);
            if (Thursday is { Count: > 0 }) 
                yield return (Day.Thursday, Thursday);
            if (Friday is { Count: > 0 }) 
                yield return (Day.Friday, Friday);
            if (Saturday is { Count: > 0 }) 
                yield return (Day.Saturday, Saturday);
            if (Sunday is { Count: > 0 }) 
                yield return (Day.Sunday, Sunday);
        }
    }
}
