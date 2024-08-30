using BSUIRScheduleDESK.Models;
using BSUIRScheduleDESK.Classes;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System;
using System.Threading.Tasks;

namespace BSUIRScheduleDESK.ViewModels
{
    public class AnnouncementViewModel : Notifier, IAnnouncementViewModel
    {
        readonly IAnnouncementModel _model;
        public AnnouncementViewModel(IAnnouncementModel announcementModel)
        {
            _model = announcementModel;
        }
        public bool IsAnnouncementsEmpty { get => Announcements.Count == 0 ? true : false; }

        public async Task<bool> SetAnnouncements(string? title, string? url)
        {
            Title = title!;
            if(int.TryParse(title, out int id))
                IsEmployeeAnnouncement = false;
            else
                IsEmployeeAnnouncement = true;
            return await _model.LoadAnnouncementsAsync(url);
        }

        private ICommand? loadScheduleCommand;
        public ICommand LoadScheduleCommand
        {
            get
            {
                return loadScheduleCommand ??
                    (loadScheduleCommand = new RelayCommand(obj =>
                    {
                        if (obj is not string url) return;
                        CommandUrl = url;
                        OnRequestClose(this, new EventArgs());
                    }));
            }
        }
        public event EventHandler OnRequestClose;
        public ObservableCollection<Announcement> Announcements =>_model.Announcements;
        public bool IsEmployeeAnnouncement { get; private set; }
        public string Title { get; private set; } = string.Empty;
        public string CommandUrl { get; private set; } = string.Empty;
    }
}
