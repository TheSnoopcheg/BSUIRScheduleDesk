using BSUIRScheduleDESK.classes;
using BSUIRScheduleDESK.services;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System;
using BSUIRScheduleDESK.views;
using System.Diagnostics;
using System.Windows;

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
        public async Task<bool> LoadSchedule(string? url, ScheduleService.LoadingType loadingType)
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
            UpdateDate updateDate;
            if (Schedule != null)
            {
                if (await Internet.CheckServerAccess($"https://iis.bsuir.by/api/v1/schedule/current-week") == Internet.ConnectionStatus.Connected)
                {
                    string? url = Schedule.GetUrl();
                    updateDate = await ScheduleService.GetLastUpdate(url);
                    if(updateDate != Schedule.updateDate)
                    {
                        ModalWindow mw = new ModalWindow("Расписание БГУИР", "", $"Расписание [{Schedule.GetName()}] было обновлено. Загрузить?", ModalWindowButtons.YesNo);
                        if (mw.ShowDialog() == true)
                        {
                            Schedule = await ScheduleService.LoadSchedule(url, ScheduleService.LoadingType.Server);
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
