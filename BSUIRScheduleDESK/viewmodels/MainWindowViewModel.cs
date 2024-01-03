using BSUIRScheduleDESK.classes;
using BSUIRScheduleDESK.models;
using BSUIRScheduleDESK.services;
using BSUIRScheduleDESK.views;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
#if DEBUG
    using System.Diagnostics;
#endif

namespace BSUIRScheduleDESK.viewmodels
{
    public class MainWindowViewModel : Notifier
    {
        private bool isCalendarOpen = false;
        public bool IsCalendarOpen
        {
            get { return isCalendarOpen; }
            set
            {
                isCalendarOpen = value;
                OnPropertyChanged();
            }
        }
        private FavoriteSchedulesViewModel favoriteSchedulesViewModel;
        public FavoriteSchedulesViewModel FavoriteSchedulesViewModel
        {
            get { return favoriteSchedulesViewModel;}
            set { favoriteSchedulesViewModel = value; }
        }
        private ObservableCollection<DateTime> _dates;
        public ObservableCollection<DateTime> Dates
        {
            get { return _dates; }
            set
            {
                _dates = value;
                OnPropertyChanged();
            }
        }
        readonly MainWindowModel _model;
        private GroupSchedule? _groupSchedule;
        private int _currentWeek;
        public int CurrentWeek
        {
            get => _currentWeek;
            set
            {
                if (value >= 5)
                    value -= 4;
                else if (value <= 0)
                    value += 4;
                _currentWeek = value;
                OnPropertyChanged();
            }
        }
        private bool _firstsubgroup;
        public bool FirstSubGroup
        {
            get => _firstsubgroup;
            set
            {
                _firstsubgroup = value;
                Properties.Settings.Default.firstsubgroup = value;
                Properties.Settings.Default.Save();
                EventService.SubgroupUpdated_Invoke();
            }
        }
        private bool _secondsubgroup;
        public bool SecondSubGroup
        {
            get => _secondsubgroup;
            set
            {
                _secondsubgroup = value;
                Properties.Settings.Default.secondsubgroup = value;
                Properties.Settings.Default.Save();
                EventService.SubgroupUpdated_Invoke();
            }
        }
        public string Build { get; set; } = "build:000022603beta 02.01.2024";
        private ObservableCollection<Announcement> _announcements;
        public ObservableCollection<Announcement> Announcements
        {
            get { return _announcements;}
            set
            {
                _announcements = value;
                OnPropertyChanged();
            }
        }
        public GroupSchedule Schedule
        {
            get { return _groupSchedule!; }
            set
            {
                string? url = value?.employee == null ? value?.studentGroup?.name : value.employee.urlId;
                if (FavoriteSchedulesViewModel.IsScheduleFavorite(url))
                {
                    value!.favorited = true;
                }
                _groupSchedule = value;
                OnPropertyChanged();
            }
        }
        private ICommand? searchSchedule;
        public ICommand SearchSchedule
        {
            get
            {
                return searchSchedule ??
                    (searchSchedule = new RelayCommand(async obj =>
                    {
                        ScheduleSearchWindow scheduleSearchWindow = new ScheduleSearchWindow();
                        if (scheduleSearchWindow.ShowDialog() == true)
                        {
                            SearchResponse? response = scheduleSearchWindow.FSearchResponce;
                            if(response != null)
                            {
                                await _model.LoadSchedule(response.GetUrl(), ScheduleService.LoadingType.Server);
                                Schedule = _model.Schedule;
                                Announcements = _model.Announcements;
                            }
                        }
                        else { }
                    }));
            }
        }
        // command to add favorite schedule without presentation
        private ICommand? addFavoriteScheduleWL;
        public ICommand AddFavoriteScheduleWL
        {
            get
            {
                return addFavoriteScheduleWL ??
                    (addFavoriteScheduleWL = new RelayCommand(async obj =>
                    {
                        ScheduleSearchWindow scheduleSearchWindow = new ScheduleSearchWindow();
                        if (scheduleSearchWindow.ShowDialog() == true)
                        {
                            SearchResponse? response = scheduleSearchWindow.FSearchResponce;
                            if (response != null)
                            {
                                GroupSchedule tSchedule = await ScheduleService.LoadSchedule(response.GetUrl(), ScheduleService.LoadingType.ServerWL);
                                if(Schedule != null)
                                {

                                    if (Schedule.Compare(tSchedule))
                                    {
                                        _model.Schedule!.favorited = true;
                                        Schedule.favorited = true;
                                        await _model.SaveRecentSchedule(Schedule);
                                    }
                                }
                                
                                EventService.ScheduleFavorited_Invoke(tSchedule, false);
                            }
                        }
                    }));
            }
        }
        private ICommand? openAnnouncements;
        public ICommand OpenAnnouncements
        {
            get
            {
                return openAnnouncements ??
                    (openAnnouncements = new RelayCommand(obj =>
                    {
                        if(Announcements != null)
                        {
                            AnnouncementWindow announcementWindow = new AnnouncementWindow();
                            announcementWindow.DataContext = new AnnouncementViewModel(Announcements, Schedule.GetName());
                            announcementWindow.ShowDialog();
                        }
                    }));
            }
        }

        private ICommand? addFavoriteSchedule;
        public ICommand AddFavoriteSchedule
        {
            get
            {
                return addFavoriteSchedule ??
                    (addFavoriteSchedule = new RelayCommand(async obj =>
                    {
                        if(Schedule != null)
                        {
                            Schedule.favorited = !Schedule.favorited;
                            OnPropertyChanged(nameof(Schedule));
                            await _model.SaveRecentSchedule(Schedule);
                            EventService.ScheduleFavorited_Invoke(Schedule, true);
                        }
                    }));
            }
        }
        private ICommand? backToCurrentWeek;
        public ICommand BackToCurrentWeek
        {
            get
            {
                return backToCurrentWeek ??
                    (backToCurrentWeek = new RelayCommand(obj =>
                    {
                        BackCurrentWeek();
                    }));
            }
        }
        private int weekDiff = 0;
        public int WeekDiff
        {
            get { return weekDiff;}
            set
            {
                weekDiff = value;
                OnPropertyChanged();
            }
        }
        private void ChangeDates(int diff)
        {
            for(int i = 0; i < 6; i++)
            {
                Dates[i] = Dates[i].AddDays(diff * 7);
            }
        }
        private ICommand? changeWeekNum;
        public ICommand ChangeWeekNum
        {
            get
            {
                return changeWeekNum ??
                    (changeWeekNum = new RelayCommand(obj =>
                    {
                        if(int.TryParse(obj.ToString(), out int res))
                        {
                            WeekDiff += res;
                            CurrentWeek += res;
                            ChangeDates(res);
                            EventService.WeekUpdated_Invoke(res);
                        }
                    }));
            }
        }
        private void BackCurrentWeek()
        {
            int wd = Properties.Settings.Default.currentweek - CurrentWeek;
            EventService.WeekUpdated_Invoke(wd);
            ChangeDates(-WeekDiff);
            CurrentWeek = Properties.Settings.Default.currentweek;
            WeekDiff = 0;
        }
        // command to load schedule from schedule' plate
        private ICommand? loadScheduleBS;
        public ICommand LoadScheduleBS
        {
            get
            {
                return loadScheduleBS ??
                    (loadScheduleBS = new RelayCommand(async obj =>
                    {
                        var ve = obj as Employee;
                        var vs = obj as StudentGroup;
                        await _model.LoadSchedule(ve == null ? vs?.name : ve.urlId, ScheduleService.LoadingType.Server);
                        BackCurrentWeek();
                        Schedule = _model.Schedule;
                        Announcements = _model.Announcements;
                    }));
            }
        }
        private ICommand? openSettingsWindow;
        public ICommand OpenSettingsWindow
        {
            get
            {
                return openSettingsWindow ??
                    (openSettingsWindow = new RelayCommand(obj =>
                    {
                        SettingsWindow settingsWindow = new SettingsWindow();
                        settingsWindow.ShowDialog();
                    }));
            }
        }
        private ICommand? calendarStatusChange;
        public ICommand CalendarStatusChange
        {
            get
            {
                return calendarStatusChange ??
                    (calendarStatusChange = new RelayCommand(obj =>
                    {
                        IsCalendarOpen = !IsCalendarOpen;
                    }));
            }
        }
        private ICommand? openSelectedWeek;
        public ICommand OpenSelectedWeek
        {
            get
            {
                return openSelectedWeek ??
                    (openSelectedWeek = new RelayCommand(obj =>
                    {
                        if(obj is DateTime date)
                        {
                            int weeks = -(DateService.GetWeekDiff(date, Dates[0]));
                            WeekDiff += weeks;
                            ChangeDates(weeks);
                            int chavo = weeks % 4;
                            CurrentWeek += chavo;
                            EventService.WeekUpdated_Invoke(chavo);
                            IsCalendarOpen = false;
                        }
                    }));
            }
        }
        
        private async void LoadFavoriteSchedule(FavoriteSchedule schedule)
        {
            await _model.LoadSchedule(schedule.UrlId, ScheduleService.LoadingType.Local);
            Schedule = _model.Schedule;
            Announcements = _model.Announcements;
        }

        private async void LoadRecentSchedule()
        {
            await _model.LoadSchedule("recent", ScheduleService.LoadingType.Local);
            Schedule = _model.Schedule;
            Announcements = _model.Announcements;
        }
        private async void OnScheduleUnFavorited(FavoriteSchedule schedule)
        {
            
            if ((Schedule?.employee == null ? Schedule?.studentGroup?.name : Schedule.employee.urlId) == schedule.UrlId)
            {
                _model.Schedule!.favorited = false;
                Schedule!.favorited = false;
                OnPropertyChanged(nameof(Schedule));
                await _model.SaveRecentSchedule(Schedule);
            }
        }
        public MainWindowViewModel()
        {
            favoriteSchedulesViewModel = new FavoriteSchedulesViewModel();
            _model = new MainWindowModel();
            EventService.FavoriteScheduleSelected += LoadFavoriteSchedule;
            EventService.ScheduleUnFavorited += OnScheduleUnFavorited;
            _firstsubgroup = Properties.Settings.Default.firstsubgroup;
            _secondsubgroup = Properties.Settings.Default.secondsubgroup;
            Task.Run(async () => 
            {
                CurrentWeek = await _model.GetCurrentWeekAsync();
                if (DateTime.Today.DayOfWeek == DayOfWeek.Sunday)
                    CurrentWeek += 1;
            });
            Dates = _model.DateTimes;
            LoadRecentSchedule();
        }
    }
}