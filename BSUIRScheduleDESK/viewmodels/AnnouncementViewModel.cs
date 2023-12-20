using BSUIRScheduleDESK.models;
using System.Collections.ObjectModel;
using BSUIRScheduleDESK.classes;

namespace BSUIRScheduleDESK.viewmodels
{
    public class AnnouncementViewModel : Notifier
    {
        readonly AnnouncementModel _model;
        public AnnouncementViewModel(ObservableCollection<Announcement> announcements, string? title)
        {
            _model = new AnnouncementModel(announcements);
            Title = title!;
            if (int.TryParse(title, out int id))
            {
                IsEmployeeAnnouncement = false;
            }
            else
                IsEmployeeAnnouncement = true;
        }
        public ReadOnlyObservableCollection<Announcement> Announcements =>_model.Announcements;
        public bool IsEmployeeAnnouncement { get; private set; }
        public string Title { get; private set; }
    }
}
