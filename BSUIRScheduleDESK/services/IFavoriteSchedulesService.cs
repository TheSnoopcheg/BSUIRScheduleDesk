using BSUIRScheduleDESK.Classes;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace BSUIRScheduleDESK.Services;

public interface IFavoriteSchedulesService
{
    Task SaveFavoriteSchedulesAsync(ObservableCollection<FavoriteSchedule> schedules);
    Task<ObservableCollection<FavoriteSchedule>> LoadFavoriteSchedulesAsync();
}
