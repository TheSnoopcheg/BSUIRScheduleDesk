using BSUIRScheduleDESK.Classes;
using BSUIRScheduleDESK.Langs;
using BSUIRScheduleDESK.Services;
using BSUIRScheduleDESK.Themes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text.Json;
using System.Windows.Input;

namespace BSUIRScheduleDESK.ViewModels;

public class SettingsViewModel : Notifier, ISettingsViewModel
{
    private readonly IScheduleService _scheduleService;
    public List<string> Themes { get; }
    public List<string> Languages { get; }
    private ObservableCollection<int> _selectedIndexes = new ObservableCollection<int>();
    public ObservableCollection<int> SelectedIndexes
    {
        get { return _selectedIndexes; }
        set
        {
            _selectedIndexes = value;
            OnPropertyChanged();
        }
    }
    private int _themeIndex;
    public int ThemeIndex
    {
        get { return _themeIndex; }
        set
        {
            _themeIndex = value;
            UpdateTheme();
            OnPropertyChanged();
        }
    }
    private int _languageIndex;
    public int LanguageIndex
    {
        get { return _languageIndex; }
        set
        {
            _languageIndex = value;
            UpdateLanguage();
            OnPropertyChanged();
        }
    }
    public SettingsViewModel(IScheduleService scheduleService)
    {
        _scheduleService = scheduleService;

        Themes = new List<string>
        {
            Langs.Language.IISTheme,
            Langs.Language.DarkTheme
        };
        Languages = new List<string>
        {
            Langs.Language.Russian,
            Langs.Language.Belarusian,
            Langs.Language.English
        };
        LoadInitialSettings();
    }

    private ICommand _currentWeekUpdate;
    public ICommand CurrentWeekUpdate
    {
        get
        {
            return _currentWeekUpdate ??
                (_currentWeekUpdate = new RelayCommand(async obj =>
                {
                    await _scheduleService.UpdateCurrentWeekAsync();
                }));
        }
    }

    private void LoadInitialSettings()
    {
        SelectedIndexes = JsonSerializer.Deserialize<ObservableCollection<int>>(Config.Instance.Indexes)!;
        ThemeIndex = GetThemeIndex();
        LanguageIndex = GetLanguageIndex();
    }
    public void UpdateColors()
    {
        EventService.ColorsUpdated_Invoke();
        Config.Instance.Indexes = JsonSerializer.Serialize(SelectedIndexes);
        Config.Instance.Save();
    }
    private int GetThemeIndex()
    {
        return Config.Instance.CurrentTheme switch
        {
            "IISTheme" => 0,
            "DarkTheme" => 1,
            _ => 0

        };
    }
    private int GetLanguageIndex()
    {
        return Config.Instance.CurrentLanguage switch
        {
            "ru" => 0,
            "be" => 1,
            "en" => 2,
            _ => 0
        };
    }
    private void UpdateTheme()
    {
        switch (ThemeIndex)
        {
            case 0: ThemeManager.SetTheme(ThemeType.IIS); break;
            case 1: ThemeManager.SetTheme(ThemeType.Dark); break;
            default: throw new ArgumentOutOfRangeException();
        }
    }
    private void UpdateLanguage()
    {
        switch (LanguageIndex)
        {
            case 0: LanguageManager.SetLanguage(LanguageType.RUS); break;
            case 1: LanguageManager.SetLanguage(LanguageType.BEL); break;
            case 2: LanguageManager.SetLanguage(LanguageType.ENG); break;
            default: throw new ArgumentOutOfRangeException();
        }
    }

    public Version Version { get; } = new Version(1, 0, 5, 4);
}
