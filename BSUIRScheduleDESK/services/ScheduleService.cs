using BSUIRScheduleDESK.classes;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using System;

namespace BSUIRScheduleDESK.services
{
    public static class ScheduleService
    {
        private const string PATH = @"\data\";
        private const string RECENTPATH = "recent";
        public enum LoadingType
        {
            Server,
            ServerWL,
            Local
        }

        public static async Task<GroupSchedule> LoadSchedule(string? url, LoadingType loadingType)
        {
            GroupSchedule? schedule = new GroupSchedule();
            if (loadingType == LoadingType.Server || loadingType == LoadingType.ServerWL)
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
            if (schedule != null && loadingType != LoadingType.ServerWL)
            {
                await SaveRecentSchedule(schedule);
            }
            return schedule!;
        }
        private static async Task<GroupSchedule> LocalLoad(string? path)
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
        private static async Task<GroupSchedule> ServerLoad(string? url, LoadingType preLoadingType = LoadingType.Server)
        {
            GroupSchedule? schedule = new GroupSchedule();
            try
            {
                if (int.TryParse(url, out int numVal))
                {
                    schedule = await NetworkService.GetAsync<GroupSchedule>($"https://iis.bsuir.by/api/v1/schedule?studentGroup={url}");
                }
                else
                {
                    schedule = await NetworkService.GetAsync<GroupSchedule>($"https://iis.bsuir.by/api/v1/employees/schedule/{url}");
                }
                if (schedule != null)
                {
                    schedule.updateDate = await GetLastUpdate(url);
                    if (preLoadingType == LoadingType.Local)
                    {
                        await SaveSchedule(schedule, url);
                    }
                }
            }
            catch { }

            return schedule!;
        }

        public static async Task SaveRecentSchedule(GroupSchedule schedule)
        {
            await SaveSchedule(schedule, RECENTPATH);
        }
        public static async Task SaveSchedule(GroupSchedule schedule, string? path)
        {
            using (FileStream createStream = File.Create($"{Directory.GetCurrentDirectory() + PATH + path}.json"))
            {
                await JsonSerializer.SerializeAsync<GroupSchedule>(createStream, schedule);
                await createStream.DisposeAsync();
            }
        }
        public static void DeleteSchedule(FavoriteSchedule? schedule)
        {
            File.Delete($"{Directory.GetCurrentDirectory() + PATH + schedule?.UrlId}.json");
        }
        public static async Task<UpdateDate> GetLastUpdate(string? url)
        {
            if (await Internet.CheckServerAccess($"https://iis.bsuir.by/api/v1/schedule/current-week") == Internet.ConnectionStatus.Connected)
            {
                if (int.TryParse(url, out var id))
                {
                    return await NetworkService.GetAsync<UpdateDate>($"https://iis.bsuir.by/api/v1/last-update-date/student-group?groupNumber={url}");
                }
                else
                {
                    return await NetworkService.GetAsync<UpdateDate>($"https://iis.bsuir.by/api/v1/last-update-date/employee?url-id={url}");
                }
            }
            return default;
        }
    }
}