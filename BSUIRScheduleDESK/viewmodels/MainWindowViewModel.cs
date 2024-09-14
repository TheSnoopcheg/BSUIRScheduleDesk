using BSUIRScheduleDESK.Classes;
using BSUIRScheduleDESK.Models;
using BSUIRScheduleDESK.Services;
using BSUIRScheduleDESK.Views;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Threading.Tasks;
using System.Windows;

namespace BSUIRScheduleDESK.ViewModels
{
    public class MainWindowViewModel : Notifier, IMainWindowViewModel
    {
        readonly IMainWindowModel _model;
        private readonly IScheduleSearchViewModel _scheduleSearchViewModel;
        private readonly IAnnouncementViewModel _announcementViewModel;
        private readonly INoteViewModel _noteViewModel;

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
        private Schedule? _groupSchedule;
        public Schedule? Schedule
        {
            get { return _groupSchedule!; }
            set
            {
                _groupSchedule = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<FavoriteSchedule> FavoriteSchedules => _model.FavoriteSchedules;
        
        public Visibility NotesExistenceVisibility
        {
            get => _noteViewModel.IsNotesEmpty ? Visibility.Collapsed : Visibility.Visible;
        }
        public Visibility AnnouncementsExistenceVisibility
        {
            get => _announcementViewModel.IsAnnouncementsEmpty ? Visibility.Collapsed : Visibility.Visible;
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
                        _scheduleSearchViewModel.ClearInput();
                        scheduleSearchWindow.DataContext = _scheduleSearchViewModel;
                        if (scheduleSearchWindow.ShowDialog() == true)
                        {
                            SearchResponse? response = _scheduleSearchViewModel.SearchResponse;
                            if (response == null) return;

                            await LoadScheduleAsync(response.GetUrl(), LoadingType.Server);
                        }
                    }));
            }
        }
        
        // command to load schedule from schedule's plate
        private ICommand? loadScheduleFromPlate;
        public ICommand LoadScheduleFromPlate
        {
            get
            {
                return loadScheduleFromPlate ??
                    (loadScheduleFromPlate = new RelayCommand(async obj =>
                    {
                        if (obj is not string url) return;
                        await LoadScheduleAsync(url, LoadingType.Server);
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
                        if (Schedule != null)
                        {
                            AnnouncementWindow announcementWindow = new AnnouncementWindow();
                            announcementWindow.DataContext = _announcementViewModel;
                            _announcementViewModel.OnRequestClose += async (s, e) => { announcementWindow.Close(); await LoadScheduleAsync(_announcementViewModel.CommandUrl, LoadingType.Server); };
                            announcementWindow.ShowDialog();
                        }
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
                            notesWindow.DataContext = _noteViewModel;
                            notesWindow.ShowDialog();
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
                        if (Schedule != null)
                        {
                            if (Schedule.favorited)
                                await _model.DeleteFavoriteScheduleAsync(Schedule);
                            else
                                await _model.AddFavoriteScheduleAsync(Schedule);

                            Schedule.favorited = !Schedule.favorited;
                            OnPropertyChanged(nameof(Schedule));
                            await _model.SaveRecentScheduleAsync(Schedule);
                            await _noteViewModel.SetNotes(Schedule.GetName(), Schedule.GetUrl());
                        }
                    }));
            }
        }

        // command to add favorite schedule without presentation
        private ICommand? addFavoriteScheduleWP;
        public ICommand AddFavoriteScheduleWP
        {
            get
            {
                return addFavoriteScheduleWP ??
                    (addFavoriteScheduleWP = new RelayCommand(async obj =>
                    {
                        ScheduleSearchWindow scheduleSearchWindow = new ScheduleSearchWindow();
                        _scheduleSearchViewModel.ClearInput();
                        scheduleSearchWindow.DataContext = _scheduleSearchViewModel;
                        if (scheduleSearchWindow.ShowDialog() == true)
                        {
                            SearchResponse? response = _scheduleSearchViewModel.SearchResponse;
                            if (response == null) return;

                            await LoadScheduleAsync(response.GetUrl(), LoadingType.ServerWP);

                            if (Schedule == null) return;
                            if (Schedule.GetUrl() != response.GetUrl()) return;

                            _model.Schedule!.favorited = true;
                            Schedule.favorited = true;
                            await _model.SaveRecentScheduleAsync(Schedule);
                            OnPropertyChanged(nameof(Schedule));
                        }
                    }));
            }
        }

        private ICommand? loadFavoriteSchedule;
        public ICommand LoadFavoriteSchedule
        {
            get
            {
                return loadFavoriteSchedule ??
                    (loadFavoriteSchedule = new RelayCommand(async obj =>
                    {
                        if (obj is FavoriteSchedule v)
                        {
                            await LoadScheduleAsync(v.UrlId, LoadingType.Local);
                        }
                    }));
            }
        }

        private ICommand? deleteFavoriteSchedule;
        public ICommand DeleteFavoriteSchedule
        {
            get
            {
                return deleteFavoriteSchedule ??
                    (deleteFavoriteSchedule = new RelayCommand(async obj =>
                    {
                        if (obj is FavoriteSchedule v)
                        {
                            await _model.DeleteFavoriteScheduleAsync(v);
                            OnScheduleUnFavorited(v);
                        }
                    }));
            }
        }

        #endregion

        #region Methods

        private async Task LoadScheduleAsync(string? url, LoadingType loadingType)
        {
            if (await _model.LoadScheduleAsync(url, loadingType))
                Schedule = _model.Schedule;
            await OnScheduleLoaded();
        }

        private async Task LoadRecentSchedule()
        {
            await LoadScheduleAsync("recent", LoadingType.Local);
        }
        private async void OnScheduleUnFavorited(FavoriteSchedule schedule)
        {
            if (Schedule == null) return;
            if (Schedule.GetUrl() == schedule.UrlId)
            {
                _model.Schedule!.favorited = false;
                Schedule!.favorited = false;
                OnPropertyChanged(nameof(Schedule));
                await _model.SaveRecentScheduleAsync(Schedule);
            }
        }

        private async Task OnScheduleLoaded()
        {
            if (Schedule == null) return;
            string? url = Schedule.GetUrl();
            string? name = Schedule.GetName();

            if (await _announcementViewModel.SetAnnouncements(name, url))
                OnPropertyChanged(nameof(AnnouncementsExistenceVisibility));

            if (_model.IsScheduleFavorited(url!))
            {
                Schedule!.favorited = true;
                _model.Schedule!.favorited = true;
                OnPropertyChanged(nameof(Schedule));
                await _noteViewModel.SetNotes(name, url);
                if (await _model.UpdateScheduleAsync())
                    Schedule = _model.Schedule;
            }

            if (DateTime.TryParse(Schedule!.startExamsDate, out DateTime startExamsDate) && DateTime.TryParse(Schedule!.endExamsDate, out DateTime endExamsDate))
            {
                if (startExamsDate <= DateTime.Today && endExamsDate >= DateTime.Today)
                    SelectedTab = 1;
                else
                    SelectedTab = 0;
            }
        }

        #endregion
        
        public MainWindowViewModel(IMainWindowModel mainWindowModel,
                                   IScheduleSearchViewModel scheduleSearchViewModel,
                                   IAnnouncementViewModel announcementViewModel,
                                   INoteViewModel noteViewModel)
        {
            
            _model = mainWindowModel;
            _scheduleSearchViewModel = scheduleSearchViewModel;
            _announcementViewModel = announcementViewModel;
            _noteViewModel = noteViewModel;

            _noteViewModel.NotesChanged += () => OnPropertyChanged(nameof(NotesExistenceVisibility));

            var _ = LoadRecentSchedule();
        }

    }
}