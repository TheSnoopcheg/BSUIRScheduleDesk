using BSUIRScheduleDESK.Classes;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BSUIRScheduleDESK.Services;

public class AnnouncementService : IAnnouncementService
{
    private readonly INetworkService _networkService;
    public AnnouncementService(INetworkService networkService)
    {
        _networkService = networkService;
    }
    public async Task<List<Announcement>?> LoadAnnouncementsAsync(string? url)
    {
        if (int.TryParse(url, out _))
        {
            return await _networkService.GetAsync<List<Announcement>>(
                $"https://iis.bsuir.by/api/v1/announcements/student-groups?name={url}");
        }

        var announcements = await _networkService.GetAsync<AnnouncementPage>(
            $"https://iis.bsuir.by/api/v1/announcements/employees?url-id={url}&dateFrom={DateTime.Today.ToString("yyyy-MM-dd")}");
        
        return announcements?.content;
    }
}
