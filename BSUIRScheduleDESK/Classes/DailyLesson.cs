namespace BSUIRScheduleDESK.Classes
{
    public class DailyLesson
    {
        public Day Day { get; set; }
        public Lesson? Lesson { get; set; }

        public override bool Equals(object? obj)
        {
            if (obj is not DailyLesson lesson) return false;
            return Day == lesson.Day && Lesson!.Equals(lesson.Lesson);
        }
    }
}
