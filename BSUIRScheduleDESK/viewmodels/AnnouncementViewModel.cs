using BSUIRScheduleDESK.models;
using System.Collections.ObjectModel;
using BSUIRScheduleDESK.classes;
using System.Windows.Input;
using System;

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
        private ICommand? loadScheduleCommand;
        public ICommand LoadScheduleCommand
        {
            set { loadScheduleCommand = value; }
            get
            {
                return loadScheduleCommand ??
                    (loadScheduleCommand = new RelayCommand(obj =>
                    {
                        if (obj is not string url) return;
                        Url = url;
                        OnRequestClose(this, new EventArgs());
                    }));
            }
        }
        public event EventHandler OnRequestClose;
        public ReadOnlyObservableCollection<Announcement> Announcements =>_model.Announcements;
        public bool IsEmployeeAnnouncement { get; private set; }
        public string Title { get; private set; }
        public string Url { get; private set; }
    }
}
