using BSUIRScheduleDESK.Classes;
using System.Threading.Tasks;

namespace BSUIRScheduleDESK.Services;

public enum LoadingType
{
    Server,
    ServerWP,
    Local
}
public interface IScheduleService
{
    Task<Schedule> LoadScheduleAsync(string? url, LoadingType loadingType);
    Task SaveRecentScheduleAsync(Schedule schedule);
    Task SaveScheduleAsync(Schedule schedule, string? path);
    void DeleteSchedule(string? ulr);
    Task UpdateCurrentWeekAsync();
}
