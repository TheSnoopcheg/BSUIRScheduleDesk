using BSUIRScheduleDESK.classes;
using System.Threading.Tasks;

namespace BSUIRScheduleDESK.services
{
    public enum LoadingType
    {
        Server,
        ServerWP,
        Local
    }
    public interface IScheduleService
    {
        Task<GroupSchedule> LoadScheduleAsync(string? url, LoadingType loadingType);
        Task SaveRecentScheduleAsync(GroupSchedule schedule);
        Task SaveScheduleAsync(GroupSchedule schedule, string? path);
        void DeleteSchedule(string? ulr);
        Task UpdateCurrentWeekAsync();
    }
}
