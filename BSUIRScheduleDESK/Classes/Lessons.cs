using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace BSUIRScheduleDESK.Classes
{
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

        public bool IsEmpty { get => Monday == null && Tuesday == null && Wednesday == null && Thursday == null && Friday == null && Saturday == null; }
    }
}
