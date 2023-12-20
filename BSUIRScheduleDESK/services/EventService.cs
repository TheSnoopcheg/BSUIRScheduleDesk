using BSUIRScheduleDESK.classes;
using System;

namespace BSUIRScheduleDESK.services
{
    public static class EventService
    {
        public static event Action<GroupSchedule, bool>? ScheduleFavorited;
        public static event Action? SubgroupUpdated;
        public static event Action<FavoriteSchedule>? FavoriteScheduleSelected;
        public static event Action<FavoriteSchedule>? ScheduleUnFavorited;
        public static event Action<int>? WeekUpdated;

        public static void ScheduleFavorited_Invoke(GroupSchedule schedule, bool isProc)
        {
            ScheduleFavorited?.Invoke(schedule, isProc);
        }
        public static void SubgroupUpdated_Invoke()
        {
            SubgroupUpdated?.Invoke();
        }
        public static void FavoriteScheduleSelected_Invoke(FavoriteSchedule schedule)
        {
            FavoriteScheduleSelected?.Invoke(schedule);
        }
        public static void ScheduleUnFavorited_Invoke(FavoriteSchedule schedule)
        {
            ScheduleUnFavorited?.Invoke(schedule);
        }
        public static void WeekUpdated_Invoke(int diff)
        {
            WeekUpdated?.Invoke(diff);
        }
    }
}
