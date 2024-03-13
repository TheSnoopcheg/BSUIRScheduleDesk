using System;
using System.Collections.ObjectModel;
using System.Linq;
using BSUIRScheduleDESK.classes;

namespace BSUIRScheduleDESK.models
{
    public class AnnouncementModel
    {
        public readonly ReadOnlyObservableCollection<Announcement> Announcements;
        public AnnouncementModel(ObservableCollection<Announcement> announcements)
        {
            Announcements = new ReadOnlyObservableCollection<Announcement>(new ObservableCollection<Announcement>(announcements.OrderBy(s => DateTime.Parse(s.date!))));
        }
    }
}
