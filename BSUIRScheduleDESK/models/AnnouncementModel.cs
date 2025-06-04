using BSUIRScheduleDESK.Classes;
using BSUIRScheduleDESK.Services;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace BSUIRScheduleDESK.Models
{
    public class AnnouncementModel : IAnnouncementModel
    {
        private readonly IAnnouncementService _announcementService;
        public ObservableCollection<Announcement> _announcements = new ObservableCollection<Announcement>();
        public ObservableCollection<Announcement> Announcements
        {
            get { return _announcements; }
            set
            {
                _announcements = value;
            }
        }

        public AnnouncementModel(IAnnouncementService announcementService)
        {
            _announcementService = announcementService;
        }

        public async Task<bool> LoadAnnouncementsAsync(string? url)
        {
            var announcements = await _announcementService.LoadAnnouncementsAsync(url);
            if (announcements == null) return false;
            Announcements = new ObservableCollection<Announcement>(announcements.OrderBy(a => DateTime.TryParse(a.date, out DateTime date) ? date : DateTime.MinValue));
            return true;
        }
    }
}
