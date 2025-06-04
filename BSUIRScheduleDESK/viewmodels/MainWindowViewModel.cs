using BSUIRScheduleDESK.Classes;
using BSUIRScheduleDESK.Models;
using BSUIRScheduleDESK.Services;
using BSUIRScheduleDESK.Views;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Threading.Tasks;

namespace BSUIRScheduleDESK.ViewModels;

public class MainWindowViewModel : Notifier, IMainWindowViewModel
{
    readonly IMainWindowModel _model;
    private readonly IScheduleSearchViewModel _scheduleSearchViewModel;
    private readonly IAnnouncementViewModel _announcementViewModel;
    private readonly INoteViewModel _noteViewModel;
    private readonly IScheduleHistoryViewModel _historyViewModel;

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
    public Schedule? Schedule => _model.Schedule;

    public ObservableCollection<FavoriteSchedule> FavoriteSchedules => _model.FavoriteSchedules;
    
    public bool IsNotesExist
    {
        get => !_noteViewModel.IsNotesEmpty;
    }
    public bool IsAnnouncementsExist
    {
        get => !_announcementViewModel.IsAnnouncementsEmpty;
    }
    public bool IsHistoryVisible
    {
        get => Schedule != null && Schedule.favorited && !_historyViewModel.IsHistoryEmpty;
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
                        string? url = string.Empty;
                        if (response == null)
                        {
                            string input = _scheduleSearchViewModel.Input;
                            if(input.Length == 6 && int.TryParse(input, out int res))
                            {
                                url = input;
                            }
                        }
                        else
                        {
                            url = response.GetUrl();
                        }
                        await LoadScheduleAsync(url, LoadingType.Server);
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
                    if (Schedule == null) return;

                    AnnouncementWindow announcementWindow = new AnnouncementWindow();
                    announcementWindow.DataContext = _announcementViewModel;
                    EventHandler windowCloseHandler = null;
                    windowCloseHandler = async (s, e) =>
                    {
                        announcementWindow.Close();
                        await LoadScheduleAsync(_announcementViewModel.CommandUrl, LoadingType.Server);
                        _announcementViewModel.OnRequestClose -= windowCloseHandler;
                    };
                    _announcementViewModel.OnRequestClose += windowCloseHandler;
                    announcementWindow.ShowDialog();
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
                    if (Schedule == null) return;

                    NotesWindow notesWindow = new NotesWindow();
                    notesWindow.DataContext = _noteViewModel;
                    notesWindow.ShowDialog();
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
                    if (Schedule == null) return;

                    if (Schedule.favorited)
                        await _model.DeleteFavoriteScheduleAsync(Schedule);
                    else
                        await _model.AddFavoriteScheduleAsync(Schedule);

                    Schedule.favorited = !Schedule.favorited;
                    OnPropertyChanged(nameof(Schedule));
                    OnPropertyChanged(nameof(IsHistoryVisible));
                    await _model.SaveRecentScheduleAsync(Schedule);
                    await _noteViewModel.SetNotes(Schedule.GetName(), Schedule.GetUrl());
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
                        string? url = string.Empty;
                        if (response == null)
                        {
                            string input = _scheduleSearchViewModel.Input;
                            if (input.Length == 6 && int.TryParse(input, out int res))
                            {
                                url = input;
                            }
                        }
                        else
                        {
                            url = response.GetUrl();
                        }

                        await LoadScheduleAsync(url, LoadingType.ServerWP);

                        if (Schedule == null) return;
                        if (Schedule.GetUrl() != url) return;

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

    private ICommand? loadFavoriteScheduleByKey;
    public ICommand LoadFavoriteScheduleByKey
    {
        get
        {
            return loadFavoriteScheduleByKey ??
                (loadFavoriteScheduleByKey = new RelayCommand(async obj =>
                {
                    if (!int.TryParse(obj.ToString(), out int num)) return;
                    if (num > FavoriteSchedules.Count) return;

                    await LoadScheduleAsync(FavoriteSchedules[num - 1].UrlId, LoadingType.Local);
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

    private ICommand? openScheduleHistoryWindow;
    public ICommand OpenScheduleHistoryWindow
    {
        get
        {
            return openScheduleHistoryWindow ??
                (openScheduleHistoryWindow = new RelayCommand(obj =>
                {
                    if (Schedule == null) return;

                    ScheduleHistoryWindow scheduleHistoryWindow = new ScheduleHistoryWindow();
                    scheduleHistoryWindow.DataContext = _historyViewModel;
                    scheduleHistoryWindow.ShowDialog();
                }));
        }
    }

    #endregion

    #region Methods

    private async Task LoadScheduleAsync(string? url, LoadingType loadingType)
    {
        if (await _model.LoadScheduleAsync(url, loadingType))
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
            Schedule.favorited = false;
            OnPropertyChanged(nameof(Schedule));
            await _model.SaveRecentScheduleAsync(Schedule);
        }
    }

    private async Task OnScheduleLoaded()
    {
        if (Schedule == null) return;
        OnPropertyChanged(nameof(Schedule));
        string? url = Schedule.GetUrl();
        string? name = Schedule.GetName();

        if (DateTime.TryParse(Schedule.startExamsDate, out DateTime startExamsDate) 
            && DateTime.TryParse(Schedule.endExamsDate, out DateTime endExamsDate))
        {
            if (startExamsDate <= DateTime.Today && endExamsDate >= DateTime.Today)
                SelectedTab = 1;
            else
                SelectedTab = 0;
        }

        if (_model.IsScheduleFavorited(url!))
        {
            Schedule.favorited = true;
            OnPropertyChanged(nameof(Schedule));
            if (await _noteViewModel.SetNotes(name, url))
                OnPropertyChanged(nameof(IsNotesExist));
            if (await _historyViewModel.SetScheduleHistory(name, url))
                OnPropertyChanged(nameof(IsHistoryVisible));
            var historyNote = await _model.UpdateScheduleAsync();
            if (historyNote != null)
            {
                _historyViewModel.AddHistoryNote(historyNote);
                OnPropertyChanged(nameof(Schedule));
            }
        }
        OnPropertyChanged(nameof(IsHistoryVisible));
        if (await _announcementViewModel.SetAnnouncements(name, url))
            OnPropertyChanged(nameof(IsAnnouncementsExist));
    }

    #endregion
    
    public MainWindowViewModel(IMainWindowModel mainWindowModel,
                               IScheduleSearchViewModel scheduleSearchViewModel,
                               IAnnouncementViewModel announcementViewModel,
                               INoteViewModel noteViewModel,
                               IScheduleHistoryViewModel historyViewModel)
    {

        _model = mainWindowModel;
        _scheduleSearchViewModel = scheduleSearchViewModel;
        _announcementViewModel = announcementViewModel;
        _noteViewModel = noteViewModel;
        _historyViewModel = historyViewModel;

        _noteViewModel.NotesChanged += () => OnPropertyChanged(nameof(IsNotesExist));

        var _ = LoadRecentSchedule();
    }
}