using BSUIRScheduleDESK.classes;
using BSUIRScheduleDESK.services;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System;
using BSUIRScheduleDESK.views;
using System.Diagnostics;

namespace BSUIRScheduleDESK.models
{
    public class MainWindowModel
    {
        public ObservableCollection<DateTime>? Dates { get; set; }
        public ObservableCollection<Announcement>? Announcements { get; set; }
        public ObservableCollection<Note>? Notes { get; set; }
        private GroupSchedule? _schedule;
        public GroupSchedule? Schedule
        {
            get => _schedule!;
            set
            {
                _schedule = value;
            }
        }
        public async Task SaveRecentSchedule(GroupSchedule schedule)
        {
            await ScheduleService.SaveRecentSchedule(schedule);
        }
        public async Task<bool> LoadSchedule(string? url, LoadingType loadingType)
        {
            GroupSchedule schedule = await ScheduleService.LoadSchedule(url, loadingType);
            if(schedule != null)
            {
                Schedule = schedule;
                return true;
            }
            return false;
        }
        public async Task<bool> LoadNotes(string? url)
        {
            ObservableCollection<Note> notes;
            notes = await NoteService.LoadNotes(url);
            if(notes != null)
            {
                Notes = notes;
                return true;
            }
            return false;
        }
        public async Task<bool> LoadAnnouncements(string? url)
        {
            ObservableCollection<Announcement> announcements;
            if (int.TryParse(url, out var id))
            {
                announcements = await NetworkService.GetAsync<ObservableCollection<Announcement>>($"https://iis.bsuir.by/api/v1/announcements/student-groups?name={url}");
            }
            else
            {
                announcements = await NetworkService.GetAsync<ObservableCollection<Announcement>>($"https://iis.bsuir.by/api/v1/announcements/employees?url-id={url}");
            }
            if(announcements != null)
            {
                Announcements = announcements;
                return true;
            }
            return false;
            
        }
        public async Task<bool> CheckScheduleUpdate()
        {
            if (Schedule != null)
            {
                if (await Internet.CheckServerAccess($"https://iis.bsuir.by/api/v1/schedule/current-week") == Internet.ConnectionStatus.Connected)
                {
                    string? url = Schedule.GetUrl();
                    Debug.WriteLine(url);
                    var schedule = await ScheduleService.LoadSchedule(url, LoadingType.Server);
                    if(!Schedule.Compare(schedule))
                    {
                        ModalWindowResult result = ModalWindow.Show($"Расписание [{Schedule.GetName()}] было обновлено. Загрузить?", "Расписание БГУИР", "", ModalWindowButtons.YesNo);
                        if (result == ModalWindowResult.Yes)
                        {
                            schedule.favorited = Schedule.favorited;
                            Schedule = schedule;
                            await ScheduleService.SaveSchedule(Schedule, url);
                            return true;
                        }
                    }
                }
            }
            return false;
        }
        public MainWindowModel()
        {
            Dates = new ObservableCollection<DateTime>(DateService.GetCurrentWeekDates());
        }
    }
}
