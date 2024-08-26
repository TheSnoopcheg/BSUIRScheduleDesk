using BSUIRScheduleDESK.classes;
using BSUIRScheduleDESK.services;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BSUIRScheduleDESK.models
{
    public class FavoriteSchedulesModel
    {
        private readonly ObservableCollection<FavoriteSchedule> _favoriteSchedules = new ObservableCollection<FavoriteSchedule>();
        public readonly ReadOnlyObservableCollection<FavoriteSchedule> FavoriteSchedules;
        public FavoriteSchedulesModel()
        {
            FavoriteSchedules = new ReadOnlyObservableCollection<FavoriteSchedule>(_favoriteSchedules);

            var lfsa = LoadFavoriteSchedulesAsync();
        }
        private async Task LoadFavoriteSchedulesAsync()
        {
            var stream = new MemoryStream(Encoding.Default.GetBytes(Config.Instance.FavoriteSchedules));
            if (stream.Length > 0)
            {
                List<FavoriteSchedule>? list = await JsonSerializer.DeserializeAsync<List<FavoriteSchedule>>(stream);
                foreach (var schedule in list!)
                {
                    _favoriteSchedules.Add(schedule);
                }
            }
        }
        private async void SaveFavoriteSchedulesAsync()
        {
            string json = string.Empty;
            using (var stream = new MemoryStream())
            {
                await JsonSerializer.SerializeAsync<ObservableCollection<FavoriteSchedule>>(stream, _favoriteSchedules);
                stream.Position = 0;
                using var reader = new StreamReader(stream);
                json = await reader.ReadToEndAsync();
            }
            Config.Instance.FavoriteSchedules = json;
            Config.Instance.Save();
        }
        public async void AddSchedule(GroupSchedule groupschedule, bool isProc) // isProc - determines the need to delete a schedule if it already exists
        {
            FavoriteSchedule? schedule = new FavoriteSchedule()
            {
                Name = groupschedule.GetName(),
                UrlId = groupschedule.GetUrl()
            };
            FavoriteSchedule? favoriteSchedule = _favoriteSchedules.FirstOrDefault(u => u.UrlId == schedule.UrlId);
            if(favoriteSchedule != null)
            {
                if (!isProc)
                    return;
                DeleteSchedule(favoriteSchedule);
            }
            else
            {
                _favoriteSchedules.Add(schedule);
                await ScheduleService.SaveSchedule(groupschedule, schedule.UrlId);
            }
            SaveFavoriteSchedulesAsync();
        }
        public void DeleteSchedule(FavoriteSchedule? schedule)
        {
            _favoriteSchedules.Remove(schedule!);
            ScheduleService.DeleteSchedule(schedule);
            SaveFavoriteSchedulesAsync();
        }
        public bool IsScheduleFavorite(string? url)
        {
            return _favoriteSchedules.Any(u => u.UrlId == url);
        }
    }
}
