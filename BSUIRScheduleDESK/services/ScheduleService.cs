using BSUIRScheduleDESK.Classes;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using System;

namespace BSUIRScheduleDESK.Services
{
    
    public class ScheduleService : IScheduleService
    {
        private const string PATH = @"\data\";
        private const string RECENTPATH = "recent";

        private readonly INetworkService _networkService;
        private readonly IInternetService _internetService;

        public ScheduleService(INetworkService networkService, IInternetService internetService)
        {
            _networkService = networkService;
            _internetService = internetService;
        }

        public async Task<Schedule> LoadScheduleAsync(string? url, LoadingType loadingType)
        {
            Schedule? schedule = new Schedule();
            if (loadingType == LoadingType.Server || loadingType == LoadingType.ServerWP)
            {
                schedule = await ServerLoad(url);
            }
            else if (loadingType == LoadingType.Local)
            {
                try
                {
                    schedule = await LocalLoad(url);
                }
                catch
                {
                    schedule = await ServerLoad(url, LoadingType.Local);
                }
            }
            if (schedule != null && loadingType != LoadingType.ServerWP)
            {
                await SaveRecentScheduleAsync(schedule);
            }
            return schedule!;
        }
        private async Task<Schedule> LocalLoad(string? path)
        {
            Schedule? schedule = new Schedule();
            try
            {
                using (FileStream openStream = File.OpenRead($"{Directory.GetCurrentDirectory() + PATH + path}.json"))
                {
                    schedule = await JsonSerializer.DeserializeAsync<Schedule>(openStream);
                    await openStream.DisposeAsync();
                }
            }
            catch
            {
                throw new Exception();
            }
            return schedule!;
        }
        private async Task<Schedule> ServerLoad(string? url, LoadingType preLoadingType = LoadingType.Server)
        {
            Schedule? schedule = new Schedule();
            try
            {
                if (int.TryParse(url, out int numVal))
                {
                    schedule = await _networkService.GetAsync<Schedule>($"https://iis.bsuir.by/api/v1/schedule?studentGroup={url}");
                }
                else
                {
                    schedule = await _networkService.GetAsync<Schedule>($"https://iis.bsuir.by/api/v1/employees/schedule/{url}");
                }
                if (schedule != null)
                {
                    if (preLoadingType == LoadingType.Local)
                    {
                        await SaveScheduleAsync(schedule, url);
                    }
                }
            }
            catch { }

            return schedule!;
        }

        public async Task SaveRecentScheduleAsync(Schedule schedule)
        {
            await SaveScheduleAsync(schedule, RECENTPATH);
        }
        public async Task SaveScheduleAsync(Schedule schedule, string? path)
        {
            using (FileStream createStream = File.Create($"{Directory.GetCurrentDirectory() + PATH + path}.json"))
            {
                await JsonSerializer.SerializeAsync<Schedule>(createStream, schedule);
                await createStream.DisposeAsync();
            }
        }
        public void DeleteSchedule(string? url)
        {
            File.Delete($"{Directory.GetCurrentDirectory() + PATH + url}.json");
        }
        public async Task UpdateCurrentWeekAsync()
        {
            if (await _internetService.CheckServerAccessAsync($"https://iis.bsuir.by/api/v1/schedule/current-week") == ConnectionStatus.Connected)
            {
                int week = await _networkService.GetAsync<int>($"https://iis.bsuir.by/api/v1/schedule/current-week");
                Config.Instance.CurrentWeek = week;
                Config.Instance.Save();
            }
        }
    }
}