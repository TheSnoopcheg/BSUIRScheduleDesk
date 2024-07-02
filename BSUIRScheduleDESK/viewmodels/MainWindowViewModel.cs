using BSUIRScheduleDESK.classes;
using BSUIRScheduleDESK.models;
using BSUIRScheduleDESK.services;
using BSUIRScheduleDESK.views;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using System.Threading;


#if DEBUG
using System.Diagnostics;
#endif

namespace BSUIRScheduleDESK.viewmodels
{
    public class MainWindowViewModel : Notifier
    {
        readonly MainWindowModel _model;

        #region Properties

        private int selectedTab = 0;
        public int SelectedTab
        {
            get { return selectedTab; }
            set
            {
                selectedTab = value;
                OnPropertyChanged();
            }
        }

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
        private ObservableCollection<DateTime>? _dates = new ObservableCollection<DateTime>();
        public ObservableCollection<DateTime>? Dates
        {
            get { return _dates; }
            set
            {
                _dates = value;
                OnPropertyChanged();
            }
        }
        private GroupSchedule? _groupSchedule;
        public GroupSchedule? Schedule
        {
            get { return _groupSchedule!; }
            set
            {
                _groupSchedule = value;
                OnPropertyChanged();
            }
        }
        private int _currentWeek;
        public int CurrentWeek
        {
            get { return _currentWeek; }
            set
            {
                if (value >= 5)
                    value -= 4;
                else if (value <= 0)
                    value += 4;
                _currentWeek = value;
                Properties.Settings.Default.openedweek = value;
                OnPropertyChanged();
            }
        }
        private ObservableCollection<Note>? _notes = new ObservableCollection<Note>();
        public ObservableCollection<Note>? Notes
        {
            get { return _notes; }
            set
            {
                _notes = value;
                OnPropertyChanged();
            }
        }
        private ObservableCollection<Announcement>? _announcements = new ObservableCollection<Announcement>();
        public ObservableCollection<Announcement>? Announcements
        {
            get { return _announcements;}
            set
            {
                _announcements = value;
                OnPropertyChanged();
            }
        }
        private int weekDiff = 0;
        public int WeekDiff
        {
            get { return weekDiff; }
            set
            {
                weekDiff = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Commands

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
                                await LoadSchedule(response.GetUrl(), LoadingType.Server);
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
                                GroupSchedule tSchedule = await ScheduleService.LoadSchedule(response.GetUrl(), LoadingType.ServerWP);
                                if(Schedule != null)
                                {
                                    if (Schedule.Compare(tSchedule))
                                    {
                                        _model.Schedule!.favorited = true;
                                        Schedule.favorited = true;
                                        await _model.SaveRecentSchedule(Schedule);
                                        OnPropertyChanged(nameof(Schedule));
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
                        if(Schedule != null)
                        {
                            AnnouncementWindow announcementWindow = new AnnouncementWindow();
                            AnnouncementViewModel announcementVW = new AnnouncementViewModel(Announcements!, Schedule!.GetName());
                            announcementWindow.DataContext = announcementVW;
                            announcementVW.OnRequestClose += async (s, e) => { announcementWindow.Close(); await LoadSchedule(announcementVW.Url, LoadingType.Server);  };
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
                            EventService.ScheduleFavorited_Invoke(Schedule, true);
                            await _model.SaveRecentSchedule(Schedule);
                            Notes = await NoteService.LoadNotes(Schedule.GetUrl());
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
        private ICommand? changeWeekNum;
        public ICommand ChangeWeekNum
        {
            get
            {
                return changeWeekNum ??
                    (changeWeekNum = new RelayCommand(obj =>
                    {
                        if (int.TryParse(obj.ToString(), out int res))
                        {
                            WeekDiff += res;
                            CurrentWeek += res;
                            ChangeDates(res);
                        }
                    }));
            }
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
                        if (obj is not string url) return;
                        await LoadSchedule(url, LoadingType.Server);
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
                        if (obj is DateTime date)
                        {
                            int weeks = -(DateService.GetWeekDiff(date, Dates![0]));
                            GoToWeekByOff(weeks, true);
                            IsCalendarOpen = false;
                        }
                    }));
            }
        }
        private ICommand? openNotesWindow;
        public ICommand OpenNotesWindow
        {
            get
            {
                return openNotesWindow ??
                    (openNotesWindow = new RelayCommand(obj =>
                    {
                        if(Schedule != null)
                        {
                            NotesWindow notesWindow = new NotesWindow();
                            notesWindow.DataContext = new NoteViewModel(Notes!, Schedule!.GetName(), Schedule!.GetUrl());
                            notesWindow.ShowDialog();
                        }
                    }));
            }
        }

        #endregion

        #region Methods
        private void ChangeDates(int diff)
        {
            for (int i = 0; i < 6; i++)
            {
                Dates![i] = Dates[i].AddDays(diff * 7);
            }
        }
        private void GoToWeekByOff(int offset, bool useEvent)
        {
            WeekDiff += offset;
            ChangeDates(offset);
            int weekDiff = offset % 4;
            CurrentWeek += weekDiff;
        }
        private void BackCurrentWeek()
        {
            GoToWeekByOff(-WeekDiff, true);
        }        
        private async Task LoadSchedule(string? url, LoadingType loadingType)
        {
            if (await _model.LoadSchedule(url, loadingType))
                Schedule = _model.Schedule;
            EventService.ScheduleLoaded_Invoke();
        }
        private async void LoadFavoriteSchedule(FavoriteSchedule schedule)
        {
            await LoadSchedule(schedule.UrlId, LoadingType.Local);
        }

        private async void LoadRecentSchedule()
        {
            await LoadSchedule("recent", LoadingType.Local);
        }
        private async void OnScheduleUnFavorited(FavoriteSchedule schedule)
        {
            if (Schedule!.GetUrl() == schedule.UrlId)
            {
                _model.Schedule!.favorited = false;
                Schedule!.favorited = false;
                OnPropertyChanged(nameof(Schedule));
                await _model.SaveRecentSchedule(Schedule);
            }
        }
        private void OnCurrentWeekUpdate()
        {
            CurrentWeek = Properties.Settings.Default.currentweek;
            EventService.CurrentWeekUpdated -= OnCurrentWeekUpdate;
        }

        private async void OnScheduleLoaded()
        {
            if (Schedule == null) return;
            string? url = Schedule.GetUrl();

            if (await _model.LoadAnnouncements(url))
                Announcements = _model.Announcements;

            if (FavoriteSchedulesViewModel.IsScheduleFavorite(url))
            {
                Schedule!.favorited = true;
                _model.Schedule!.favorited = true;
                OnPropertyChanged(nameof(Schedule));
                await _model.SaveRecentSchedule(Schedule);
            }
            
            if (DateTime.TryParse(Schedule!.startExamsDate, out DateTime startExamsDate) && DateTime.TryParse(Schedule!.endExamsDate, out DateTime endExamsDate))
            {
                if (startExamsDate <= DateTime.Today && endExamsDate >= DateTime.Today)
                    SelectedTab = 1;
                else
                    SelectedTab = 0;
            }

            if (DateTime.Today.DayOfWeek == DayOfWeek.Sunday)
                GoToWeekByOff(-WeekDiff + 1, false);
            else
                GoToWeekByOff(-WeekDiff, false);

            if (Schedule.favorited)
            {
                if (await _model.LoadNotes(url))
                    Notes = _model.Notes;
                if (await _model.CheckScheduleUpdate())
                    Schedule = _model.Schedule;
            }
        }

        #endregion
        public MainWindowViewModel()
        {
            favoriteSchedulesViewModel = new FavoriteSchedulesViewModel();
            _model = new MainWindowModel();
            EventService.FavoriteScheduleSelected += LoadFavoriteSchedule;
            EventService.ScheduleUnFavorited += OnScheduleUnFavorited;
            EventService.ScheduleLoaded += OnScheduleLoaded;
            if(Properties.Settings.Default.currentweek == 0 || Properties.Settings.Default.laststartup == DateTime.MinValue)
            {
                EventService.CurrentWeekUpdated += OnCurrentWeekUpdate;
            }
            CurrentWeek = Properties.Settings.Default.currentweek;
            Dates = _model.Dates;
            LoadRecentSchedule();
        }
    }
}