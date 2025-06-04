using BSUIRScheduleDESK.Classes;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BSUIRScheduleDESK.Services
{
    public interface IAnnouncementService
    {
        Task<List<Announcement>?> LoadAnnouncementsAsync(string? url);
    }
}
