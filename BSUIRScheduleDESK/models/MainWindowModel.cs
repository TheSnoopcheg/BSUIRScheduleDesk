using BSUIRScheduleDESK.classes;
using BSUIRScheduleDESK.services;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using BSUIRScheduleDESK.views;
using System.Linq;
using System.Diagnostics;

namespace BSUIRScheduleDESK.models
{
    public class MainWindowModel : IMainWindowModel
    {
        private readonly IScheduleService _scheduleService;
        private readonly INoteService _noteService;
        private readonly INetworkService _networkService;
        private readonly IInternetService _internetService;
        private readonly IFavoriteSchedulesService _favoriteSchedulesService;

        private GroupSchedule? _schedule;
        public GroupSchedule? Schedule
        {
            get => _schedule!;
            set
            {
                _schedule = value;
            }
        }
        private ObservableCollection<FavoriteSchedule> _favoriteSchedules;
        public ObservableCollection<FavoriteSchedule> FavoriteSchedules
        {
            get => _favoriteSchedules;
            set
            {
                _favoriteSchedules = value;
            }
        }
        public async Task SaveRecentScheduleAsync(GroupSchedule schedule)
        {
            await _scheduleService.SaveRecentScheduleAsync(schedule);
        }
        public async Task<bool> LoadScheduleAsync(string? url, LoadingType loadingType)
        {
            GroupSchedule schedule = await _scheduleService.LoadScheduleAsync(url, loadingType);
            if(schedule != null)
            {
                if (loadingType != LoadingType.ServerWP)
                    Schedule = schedule;
                else                                            // shitcoding
                    await AddFavoriteScheduleAsync(schedule);
                return true;
            }
            return false;
        }

        public async Task AddFavoriteScheduleAsync(GroupSchedule schedule)
        { 
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
        public async Task DeleteFavoriteScheduleAsync(GroupSchedule schedule)
        {
            var favSchedule = new FavoriteSchedule { Name = schedule.GetName(), UrlId = schedule.GetUrl() };
            await DeleteFavoriteScheduleAsync(favSchedule);
        }

        public bool IsScheduleFavorited(string url)
        {
            return FavoriteSchedules.Any(s => s.UrlId == url);
        }

        public async Task<bool> UpdateScheduleAsync()
        {
            if (Schedule != null)
            {
                if (await _internetService.CheckServerAccessAsync($"https://iis.bsuir.by/api/v1/schedule/current-week") == ConnectionStatus.Connected)
                {
                    string? url = Schedule.GetUrl();
                    var schedule = await _scheduleService.LoadScheduleAsync(url, LoadingType.Server);
                    if(!Schedule.Compare(schedule))
                    {
                        ModalWindowResult result = ModalWindow.Show($"Расписание [{Schedule.GetName()}] было обновлено. Загрузить?", "Расписание БГУИР", "", ModalWindowButtons.YesNo);
                        if (result == ModalWindowResult.Yes)
                        {
                            schedule.favorited = Schedule.favorited;
                            Schedule = schedule;
                            await _scheduleService.SaveScheduleAsync(Schedule, url);
                            return true;
                        }
                    }
                }
            }
            return false;
        }
        private async Task LoadFavoriteSchedules()
        {
            FavoriteSchedules = await _favoriteSchedulesService.LoadFavoriteSchedulesAsync();
        }
        public MainWindowModel(IScheduleService scheduleService, 
                               INoteService noteService, 
                               INetworkService networkService, 
                               IInternetService internetService, 
                               IFavoriteSchedulesService favoriteSchedulesService)
        {
            _scheduleService = scheduleService;
            _noteService = noteService;
            _networkService = networkService;
            _internetService = internetService;
            _favoriteSchedulesService = favoriteSchedulesService;

            var _ = LoadFavoriteSchedules();
        }
    }
}
