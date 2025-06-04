using BSUIRScheduleDESK.Classes;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BSUIRScheduleDESK.Services
{
    public class AnnouncementService : IAnnouncementService
    {
        private readonly INetworkService _networkService;
        public AnnouncementService(INetworkService networkService)
        {
            _networkService = networkService;
        }
        public async Task<List<Announcement>?> LoadAnnouncementsAsync(string? url)
        {
            List<Announcement>? announcements;
            if (int.TryParse(url, out var id))
            {
                announcements = await _networkService.GetAsync<List<Announcement>>($"https://iis.bsuir.by/api/v1/announcements/student-groups?name={url}");
            }
            else
            {
                announcements = await _networkService.GetAsync<List<Announcement>>($"https://iis.bsuir.by/api/v1/announcements/employees?url-id={url}");
            }
            if (announcements != null)
                return announcements;
            else
                return default;
        }
    }
}
