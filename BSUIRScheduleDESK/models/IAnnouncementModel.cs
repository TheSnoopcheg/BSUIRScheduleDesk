using BSUIRScheduleDESK.Classes;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace BSUIRScheduleDESK.Models;

public interface IAnnouncementModel
{
    ObservableCollection<Announcement> Announcements { get; }
    Task<bool> LoadAnnouncementsAsync(string? url);
}
