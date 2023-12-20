using System.Collections.ObjectModel;
using BSUIRScheduleDESK.classes;

namespace BSUIRScheduleDESK.models
{
    public class AnnouncementModel
    {
        public readonly ReadOnlyObservableCollection<Announcement> Announcements;
        public AnnouncementModel(ObservableCollection<Announcement> announcements)
        {
            Announcements = new ReadOnlyObservableCollection<Announcement>(announcements);
        }
    }
}
