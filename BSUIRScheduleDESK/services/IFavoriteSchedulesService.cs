using BSUIRScheduleDESK.classes;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace BSUIRScheduleDESK.services
{
    public interface IFavoriteSchedulesService
    {
        Task SaveFavoriteSchedulesAsync(ObservableCollection<FavoriteSchedule> schedules);
        Task<ObservableCollection<FavoriteSchedule>> LoadFavoriteSchedulesAsync();
    }
}
