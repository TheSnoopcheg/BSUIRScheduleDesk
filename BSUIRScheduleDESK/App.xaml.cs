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

namespace BSUIRScheduleDESK
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private IMainWindowViewModel _mainWindowViewModel;
        private IScheduleService _scheduleService;
        private IDateService _dateService;
        public IServiceProvider ServiceProvider { get; private set; }
        protected override void OnStartup(StartupEventArgs e)
        {
            IsAnotherProcessExist();
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            App.Current.Resources.MergedDictionaries[0] = new ResourceDictionary() { Source = new Uri($"Themes/ColourDictionaries/{Config.Instance.CurrentTheme}.xaml", UriKind.Relative)};

            ConfigureServices();

            _mainWindowViewModel = ServiceProvider.GetService<IMainWindowViewModel>()!;
            _scheduleService = ServiceProvider.GetService<IScheduleService>()!;
            _dateService = ServiceProvider.GetService<IDateService>()!;

            string? dataPath = Directory.GetCurrentDirectory() + @"\data";
            if (!Directory.Exists(dataPath))
            {
                Directory.CreateDirectory(dataPath);
            }

            if(Config.Instance.CurrentWeek == 0 || Config.Instance.LastStartup == DateTime.MinValue)
            {
                var up = _scheduleService.UpdateCurrentWeekAsync();
            }

            int wd = _dateService.GetWeekDiff(Config.Instance.LastStartup, DateTime.Today);
            Config.Instance.CurrentWeek = (Config.Instance.CurrentWeek + wd + 3) % 4 + 1;
            Config.Instance.Save();

            Config.Instance.LastStartup = DateTime.Today;
            Config.Instance.Save();
            this.MainWindow = new MainWindow();
            this.MainWindow.DataContext = _mainWindowViewModel;
            MainWindow.Show();
            ShowUpdateInfo();
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

            services.AddSingleton<IMainWindowViewModel, MainWindowViewModel>();
            services.AddSingleton<IScheduleSearchViewModel, ScheduleSearchViewModel>();
            services.AddSingleton<INoteViewModel, NoteViewModel>();
            services.AddSingleton<IAnnouncementViewModel, AnnouncementViewModel>();

            services.AddSingleton<IDateService, DateService>();
            services.AddSingleton<INetworkService, NetworkService>();
            services.AddSingleton<IInternetService, InternetService>();

            services.AddTransient<INoteService, NoteService>();
            services.AddTransient<IFavoriteSchedulesService, FavoriteSchedulesService>();
            services.AddTransient<IAnnouncementService, AnnouncementService>();
            services.AddTransient<IScheduleService, ScheduleService>();
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
                ModalWindow.Show("Данная программа уже запущена.", "Расписание БГУИР", "", ModalWindowButtons.OK);
                this.Shutdown();
            }
        }
        private void ShowUpdateInfo()
        {
            string? path = Directory.GetCurrentDirectory() + @"\updates\update.json";
            if (File.Exists(path))
            {
                string? updateInfoJson = File.ReadAllText(path);
                UpdateInfo updateInfo = JsonSerializer.Deserialize<UpdateInfo>(updateInfoJson)!;
                if (updateInfo != null)
                {
                    if (!updateInfo.IsShowed)
                    {
                        ModalWindow.Show(updateInfo.Content!, $"Обновление от {updateInfo.UpdateDate}", Directory.GetCurrentDirectory() + @"\updates\update.png", ModalWindowButtons.OK);
                        updateInfo.IsShowed = true;
                        updateInfoJson = JsonSerializer.Serialize(updateInfo);
                        File.WriteAllText(path, updateInfoJson);
                    }
                }
            }
        }
        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            File.WriteAllText($"{Directory.GetCurrentDirectory()}\\crash.log", $"[{DateTime.Now}] [{sender}] \n{e.ExceptionObject}");
            ModalWindow.Show("Упс.. Отправьте, пожалуйста, файл crash.log в телеграм @snoopcheg\nДля запуска попробуйте удалить recent.json в папке data", "Critical error", null, ModalWindowButtons.OK);
        }
    }
}
