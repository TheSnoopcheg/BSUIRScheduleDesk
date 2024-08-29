using BSUIRScheduleDESK.classes;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BSUIRScheduleDESK.services
{
    public interface IAnnouncementService
    {
        Task<List<Announcement>> LoadAnnouncementsAsync(string? url);
    }
}
