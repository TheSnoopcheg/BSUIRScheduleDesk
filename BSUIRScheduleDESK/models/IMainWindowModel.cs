using BSUIRScheduleDESK.Classes;
using BSUIRScheduleDESK.Services;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace BSUIRScheduleDESK.Models
{
    public interface IMainWindowModel
    {
        Schedule? Schedule { get; set; }
        ObservableCollection<FavoriteSchedule> FavoriteSchedules { get; }
        Task SaveRecentScheduleAsync(Schedule schedule);
        Task<bool> LoadScheduleAsync(string? url, LoadingType loadingType);
        Task AddFavoriteScheduleAsync(Schedule schedule);
        Task DeleteFavoriteScheduleAsync(FavoriteSchedule schedule);
        Task DeleteFavoriteScheduleAsync(Schedule schedule);
        bool IsScheduleFavorited(string url);
        Task<bool> UpdateScheduleAsync();
    }
}
