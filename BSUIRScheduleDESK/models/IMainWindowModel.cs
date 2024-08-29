using BSUIRScheduleDESK.classes;
using BSUIRScheduleDESK.services;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace BSUIRScheduleDESK.models
{
    public interface IMainWindowModel
    {
        GroupSchedule Schedule { get; set; }
        ObservableCollection<FavoriteSchedule> FavoriteSchedules { get; }
        Task SaveRecentScheduleAsync(GroupSchedule schedule);
        Task<bool> LoadScheduleAsync(string? url, LoadingType loadingType);
        Task AddFavoriteScheduleAsync(GroupSchedule schedule);
        Task DeleteFavoriteScheduleAsync(FavoriteSchedule schedule);
        Task DeleteFavoriteScheduleAsync(GroupSchedule schedule);
        bool IsScheduleFavorited(string url);
        Task<bool> UpdateScheduleAsync();
    }
}
