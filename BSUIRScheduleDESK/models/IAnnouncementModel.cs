using BSUIRScheduleDESK.classes;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace BSUIRScheduleDESK.models
{
    public interface IAnnouncementModel
    {
        ObservableCollection<Announcement> Announcements { get; }
        Task<bool> LoadAnnouncementsAsync(string? url);
    }
}
