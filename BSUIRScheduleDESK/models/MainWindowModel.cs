using BSUIRScheduleDESK.classes;
using BSUIRScheduleDESK.services;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Windows;
using System;

namespace BSUIRScheduleDESK.models
{
    public class MainWindowModel
    {
        public ObservableCollection<DateTime>? Dates { get; set; }
        public ObservableCollection<Announcement>? Announcements { get; set; }
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
        public async Task LoadSchedule(string? url, ScheduleService.LoadingType loadingType)
        {
            GroupSchedule schedule = await ScheduleService.LoadSchedule(url, loadingType);
            if(schedule != null)
            {
                if(await Internet.CheckServerAccess($"https://iis.bsuir.by/api/v1/schedule/current-week") == Internet.ConnectionStatus.Connected)
                {
                    if (schedule.studentGroup != null)
                    {
                        Announcements = await NetworkService.GetAsync<ObservableCollection<Announcement>>($"https://iis.bsuir.by/api/v1/announcements/student-groups?name={schedule.studentGroup!.name}");
                    }
                    else
                    {
                        Announcements = await NetworkService.GetAsync<ObservableCollection<Announcement>>($"https://iis.bsuir.by/api/v1/announcements/employees?url-id={schedule.employee!.urlId}");
                    }
                }
                if (schedule.favorited)
                {
                    schedule = await CheckLastUpdate(schedule);
                }
                Schedule = schedule;
            }
        }
        public async Task<GroupSchedule> CheckLastUpdate(GroupSchedule? schedule)
        {
            UpdateDate updateDate;
            if (schedule != null)
            {
                if(await Internet.CheckServerAccess($"https://iis.bsuir.by/api/v1/schedule/current-week") == Internet.ConnectionStatus.Connected)
                {
                    string? url = schedule.GetUrl();
                    updateDate = await ScheduleService.GetLastUpdate(url);
                    if(updateDate != schedule.updateDate)
                    {
                        if(MessageBox.Show($"Расписание [{schedule.GetName()}] было обновлено. Загрузить?", "BSUIR Schedule", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                        {
                            schedule = await ScheduleService.LoadSchedule(url, ScheduleService.LoadingType.Server);
                            await ScheduleService.SaveSchedule(schedule, url);
                        }
                    }
                }
            }
            return schedule!;
        }
        public MainWindowModel()
        {
            Dates = new ObservableCollection<DateTime>(DateService.GetCurrentWeekDates());
        }
    }
}
