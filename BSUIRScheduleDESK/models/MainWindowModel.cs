using BSUIRScheduleDESK.Classes;
using BSUIRScheduleDESK.Views;
using BSUIRScheduleDESK.Services;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Linq;

namespace BSUIRScheduleDESK.Models
{
    public class MainWindowModel : IMainWindowModel
    {
        private readonly IScheduleService _scheduleService;
        private readonly IInternetService _internetService;
        private readonly IFavoriteSchedulesService _favoriteSchedulesService;

        private Schedule? _schedule;
        public Schedule? Schedule
        {
            get => _schedule;
            set
            {
                _schedule = value;
            }
        }
        private ObservableCollection<FavoriteSchedule> _favoriteSchedules = new ObservableCollection<FavoriteSchedule>();
        public ObservableCollection<FavoriteSchedule> FavoriteSchedules
        {
            get => _favoriteSchedules;
            set
            {
                _favoriteSchedules = value;
            }
        }
        public async Task SaveRecentScheduleAsync(Schedule schedule)
        {
            await _scheduleService.SaveRecentScheduleAsync(schedule);
        }
        public async Task<bool> LoadScheduleAsync(string? url, LoadingType loadingType)
        {
            Schedule schedule = await _scheduleService.LoadScheduleAsync(url, loadingType);
            if (schedule == null) return false;

            if (loadingType != LoadingType.ServerWP)
                Schedule = schedule;
            else                                            // shitcoding
                await AddFavoriteScheduleAsync(schedule);
            return true;
        }

        public async Task AddFavoriteScheduleAsync(Schedule schedule)
        {
            if (FavoriteSchedules.Any(s => s.UrlId == schedule.GetUrl())) return;
            var favSchedule = new FavoriteSchedule { Name = schedule.GetName(), UrlId=schedule.GetUrl() };
            FavoriteSchedules.Add(favSchedule);
            await _scheduleService.SaveScheduleAsync(schedule, favSchedule.UrlId);
            await _favoriteSchedulesService.SaveFavoriteSchedulesAsync(FavoriteSchedules);
        }
        public async Task DeleteFavoriteScheduleAsync(FavoriteSchedule schedule)
        {
            var scheduleToDelete = FavoriteSchedules.FirstOrDefault(s => s.UrlId == schedule.UrlId);
            if (scheduleToDelete == null) return;
            FavoriteSchedules.Remove(scheduleToDelete);
            _scheduleService.DeleteSchedule(schedule.UrlId);
            await _favoriteSchedulesService.SaveFavoriteSchedulesAsync(FavoriteSchedules);
        }
        public async Task DeleteFavoriteScheduleAsync(Schedule schedule)
        {
            var favSchedule = new FavoriteSchedule { Name = schedule.GetName(), UrlId = schedule.GetUrl() };
            await DeleteFavoriteScheduleAsync(favSchedule);
        }

        public bool IsScheduleFavorited(string url)
        {
            return FavoriteSchedules.Any(s => s.UrlId == url);
        }

        public async Task<HistoryNote?> UpdateScheduleAsync()
        {
            if (Schedule == null) return null;
            if (await _internetService.CheckServerAccessAsync($"https://iis.bsuir.by/api/v1/schedule/current-week") != ConnectionStatus.Connected) return null;

            string? url = Schedule.GetUrl();
            var newSchedule = await _scheduleService.LoadScheduleAsync(url, LoadingType.ServerWP);
            if (newSchedule == null) return null;
            if (Schedule.GetUrl() != newSchedule.GetUrl()) return null;
            if (!Schedule.Compare(newSchedule))
            {
                ModalWindowResult result = ModalWindow.Show(string.Format(Langs.Language.ScheduleUpdateQuestionFormat, newSchedule.GetName()), Langs.Language.AppName, "", ModalWindowButtons.YesNo);
                if (result == ModalWindowResult.Yes)
                {
                    Difference difference = new Difference();
                    difference.Differences = ObjectComparer.GetDifferences(Schedule, newSchedule, out string? name, out string? namer);
                    HistoryNote note = new HistoryNote() { UpdateDate = System.DateTime.Today, Difference = difference };
                    newSchedule.favorited = Schedule.favorited;
                    Schedule = newSchedule;
                    await _scheduleService.SaveScheduleAsync(Schedule, url);
                    await SaveRecentScheduleAsync(Schedule);
                    return note;
                }
            }

            return null;
        }
        private async Task LoadFavoriteSchedules()
        {
            FavoriteSchedules = await _favoriteSchedulesService.LoadFavoriteSchedulesAsync();
        }
        public MainWindowModel(IScheduleService scheduleService,
                               IInternetService internetService, 
                               IFavoriteSchedulesService favoriteSchedulesService)
        {
            _scheduleService = scheduleService;
            _internetService = internetService;
            _favoriteSchedulesService = favoriteSchedulesService;

            var _ = LoadFavoriteSchedules();
        }
    }
}
