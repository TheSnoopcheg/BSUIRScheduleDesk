using System;
using System.Threading.Tasks;

namespace BSUIRScheduleDESK.ViewModels
{
    public interface IAnnouncementViewModel
    {
        bool IsAnnouncementsEmpty { get; }
        string CommandUrl { get; }
        Task<bool> SetAnnouncements(string? title, string? url);
        event EventHandler OnRequestClose;
    }
}
