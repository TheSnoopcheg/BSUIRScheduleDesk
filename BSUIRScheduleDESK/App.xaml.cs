using BSUIRScheduleDESK.Classes;
using BSUIRScheduleDESK.Models;
using BSUIRScheduleDESK.Services;
using BSUIRScheduleDESK.ViewModels;
using BSUIRScheduleDESK.Views;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Windows;
using System.Collections.Generic;

namespace BSUIRScheduleDESK;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    private IMainWindowViewModel _mainWindowViewModel;
    private IScheduleService _scheduleService;
    public IServiceProvider ServiceProvider { get; private set; }
    protected override void OnStartup(StartupEventArgs e)
    {
#if DEBUG

#else
        IsAnotherProcessExist();
#endif
        AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
        App.Current.Resources.MergedDictionaries[0] = new ResourceDictionary() { Source = new Uri($"Themes/ColourDictionaries/{Config.Instance.CurrentTheme}.xaml", UriKind.Relative)};

        ConfigureServices();

        System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(Config.Instance.CurrentLanguage);

        _mainWindowViewModel = ServiceProvider.GetService<IMainWindowViewModel>()!;
        _scheduleService = ServiceProvider.GetService<IScheduleService>()!;

        string? dataPath = Directory.GetCurrentDirectory() + @"\data";
        if (!Directory.Exists(dataPath))
        {
            Directory.CreateDirectory(dataPath);
        }

        int wd = DateService.GetWeekDiff(Config.Instance.LastStartup, DateTime.Today);
        Config.Instance.CurrentWeek = (Config.Instance.CurrentWeek + wd + 3) % 4 + 1;
        Config.Instance.LastStartup = DateTime.Today;

        this.MainWindow = new MainWindow();
        this.MainWindow.DataContext = _mainWindowViewModel;
        
        MainWindow.Show();
        
        ShowUpdateInfo();

        _ = _scheduleService.UpdateCurrentWeekAsync();

        base.OnStartup(e);
    }

    private void ConfigureServices()
    {
        var services = new ServiceCollection();
        services.AddSingleton<App>();

        services.AddSingleton<IMainWindowModel, MainWindowModel>();
        services.AddSingleton<IScheduleSearchModel, ScheduleSearchModel>();
        services.AddSingleton<IAnnouncementModel, AnnouncementModel>();
        services.AddSingleton<INoteModel, NoteModel>();
        services.AddSingleton<IScheduleHistoryModel, ScheduleHistoryModel>();
        services.AddSingleton<EmployeeInfoModel>();

        services.AddSingleton<IMainWindowViewModel, MainWindowViewModel>();
        services.AddSingleton<IScheduleSearchViewModel, ScheduleSearchViewModel>();
        services.AddSingleton<INoteViewModel, NoteViewModel>();
        services.AddSingleton<IAnnouncementViewModel, AnnouncementViewModel>();
        services.AddSingleton<IScheduleHistoryViewModel, ScheduleHistoryViewModel>();
        services.AddSingleton<ISettingsViewModel, SettingsViewModel>();
        services.AddSingleton<EmployeeInfoViewModel>();

        services.AddSingleton<INetworkService, NetworkService>();
        services.AddSingleton<IInternetService, InternetService>();

        services.AddTransient<INoteService, NoteService>();
        services.AddTransient<IFavoriteSchedulesService, FavoriteSchedulesService>();
        services.AddTransient<IAnnouncementService, AnnouncementService>();
        services.AddTransient<IScheduleService, ScheduleService>();
        services.AddTransient<IScheduleHistoryService, ScheduleHistoryService>();
        ServiceProvider = services.BuildServiceProvider();
    }
    protected override void OnExit(ExitEventArgs e)
    {
        Config.Instance.Save();
        base.OnExit(e);
    }
    private void IsAnotherProcessExist()
    {
        string? processName = Process.GetCurrentProcess().ProcessName;
        Process[] processes = Process.GetProcesses().Where(u => u.ProcessName == processName).ToArray();
        if(processes.Length > 1)
        {
            ModalWindow.Show(Langs.Language.ProgramRunning + ".", Langs.Language.AppName, "", ModalWindowButtons.OK);
            this.Shutdown();
        }
    }
    private void ShowUpdateInfo()
    {
        string? path = Directory.GetCurrentDirectory() + @"\updates\";
        if (!Directory.Exists(path)) return;
        List<UpdateInfo?> updates = Directory.GetFiles(path).Select(f => JsonSerializer.Deserialize<UpdateInfo>(File.ReadAllText(f))).Where(u => u.IsShowed == false).ToList();
        if (updates.Count == 0) return;
        string text = string.Empty;
        foreach(var update in updates)
        {
            text += $"{Langs.Language.UpdateBy} {update.UpdateDate}:\n{update.Content}\n\n";
            update.IsShowed = true;
            File.WriteAllText($"{path}{update.UpdateDate}.json", JsonSerializer.Serialize(update));
        }
        ModalWindow.Show(text, Langs.Language.Updates, Directory.GetCurrentDirectory() + @"\updates\update.png", ModalWindowButtons.OK);
    }
    private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
    {
        File.WriteAllText($"{Directory.GetCurrentDirectory()}\\crash.log", $"[{DateTime.Now}] [{sender}] \n{e.ExceptionObject}");
        ModalWindow.Show(Langs.Language.CriticalErrorMessage, Langs.Language.CriticalError, string.Empty, ModalWindowButtons.OK);
    }
}
