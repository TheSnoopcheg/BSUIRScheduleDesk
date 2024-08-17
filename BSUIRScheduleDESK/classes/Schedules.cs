using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace BSUIRScheduleDESK.classes
{
    public class Schedules
    {
        [JsonPropertyName("Понедельник")]
        public List<Schedule>? Monday { get; set; }
        [JsonPropertyName("Вторник")]
        public List<Schedule>? Tuesday { get; set; }
        [JsonPropertyName("Среда")]
        public List<Schedule>? Wednesday { get; set; }
        [JsonPropertyName("Четверг")]
        public List<Schedule>? Thursday { get; set; }
        [JsonPropertyName("Пятница")]
        public List<Schedule>? Friday { get; set; }
        [JsonPropertyName("Суббота")]
        public List<Schedule>? Saturday { get; set; }
        [JsonPropertyName("Воскресенье")]
        public List<Schedule>? Sunday { get; set; }

        public bool IsEmpty { get => Monday == null && Tuesday == null && Wednesday == null && Thursday == null && Friday == null && Saturday == null; }
    }
}
