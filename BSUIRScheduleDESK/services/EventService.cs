using BSUIRScheduleDESK.classes;
using System;

namespace BSUIRScheduleDESK.services
{
    public static class EventService
    {
        public static event Action<GroupSchedule, bool>? ScheduleFavorited;
        public static event Action<FavoriteSchedule>? FavoriteScheduleSelected;
        public static event Action<FavoriteSchedule>? ScheduleUnFavorited;
        public static event Action? CurrentWeekUpdated;
        public static event Action? ScheduleLoaded;
        public static event Action? ColorsUpdated;
        public static void ColorsUpdated_Invoke()
        {
            ColorsUpdated?.Invoke();
        }
        public static void ScheduleLoaded_Invoke()
        {
            ScheduleLoaded?.Invoke();
        }
        public static void CurrentWeekUpdated_Invoke()
        {
            CurrentWeekUpdated!.Invoke();
        }
        public static void ScheduleFavorited_Invoke(GroupSchedule schedule, bool isProc)
        {
            ScheduleFavorited?.Invoke(schedule, isProc);
        }
        public static void FavoriteScheduleSelected_Invoke(FavoriteSchedule schedule)
        {
            FavoriteScheduleSelected?.Invoke(schedule);
        }
        public static void ScheduleUnFavorited_Invoke(FavoriteSchedule schedule)
        {
            ScheduleUnFavorited?.Invoke(schedule);
        }
    }
}
