using BSUIRScheduleDESK.Classes;
using System.IO;
using System.Text.Json;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace BSUIRScheduleDESK.Services
{
    public class FavoriteSchedulesService : IFavoriteSchedulesService
    {
        public async Task<ObservableCollection<FavoriteSchedule>> LoadFavoriteSchedulesAsync()
        {
            ObservableCollection<FavoriteSchedule>? list = new ObservableCollection<FavoriteSchedule>();
            if (Config.Instance.FavoriteSchedules == null) return list;
            var stream = new MemoryStream(Encoding.Default.GetBytes(Config.Instance.FavoriteSchedules));
            if (stream.Length > 0)
            {
                list = await JsonSerializer.DeserializeAsync<ObservableCollection<FavoriteSchedule>>(stream);
            }
            if (list == null) return default;
            return list;
        }
        public async Task SaveFavoriteSchedulesAsync(ObservableCollection<FavoriteSchedule> schedules)
        {
            string json = string.Empty;
            using (var stream = new MemoryStream())
            {
                await JsonSerializer.SerializeAsync<ObservableCollection<FavoriteSchedule>>(stream, schedules);
                stream.Position = 0;
                using var reader = new StreamReader(stream);
                json = await reader.ReadToEndAsync();
            }
            Config.Instance.FavoriteSchedules = json;
            Config.Instance.Save();
        }
    }
}
