using BSUIRScheduleDESK.classes;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using System;

namespace BSUIRScheduleDESK.services
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

        public async Task<GroupSchedule> LoadScheduleAsync(string? url, LoadingType loadingType)
        {
            GroupSchedule? schedule = new GroupSchedule();
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
        private async Task<GroupSchedule> LocalLoad(string? path)
        {
            GroupSchedule? schedule = new GroupSchedule();
            try
            {
                using (FileStream openStream = File.OpenRead($"{Directory.GetCurrentDirectory() + PATH + path}.json"))
                {
                    schedule = await JsonSerializer.DeserializeAsync<GroupSchedule>(openStream);
                    await openStream.DisposeAsync();
                }
            }
            catch
            {
                throw new Exception();
            }
            return schedule!;
        }
        private async Task<GroupSchedule> ServerLoad(string? url, LoadingType preLoadingType = LoadingType.Server)
        {
            GroupSchedule? schedule = new GroupSchedule();
            try
            {
                if (int.TryParse(url, out int numVal))
                {
                    schedule = await _networkService.GetAsync<GroupSchedule>($"https://iis.bsuir.by/api/v1/schedule?studentGroup={url}");
                }
                else
                {
                    schedule = await _networkService.GetAsync<GroupSchedule>($"https://iis.bsuir.by/api/v1/employees/schedule/{url}");
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

        public async Task SaveRecentScheduleAsync(GroupSchedule schedule)
        {
            await SaveScheduleAsync(schedule, RECENTPATH);
        }
        public async Task SaveScheduleAsync(GroupSchedule schedule, string? path)
        {
            using (FileStream createStream = File.Create($"{Directory.GetCurrentDirectory() + PATH + path}.json"))
            {
                await JsonSerializer.SerializeAsync<GroupSchedule>(createStream, schedule);
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