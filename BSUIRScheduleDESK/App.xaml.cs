using BSUIRScheduleDESK.classes;
using BSUIRScheduleDESK.services;
using BSUIRScheduleDESK.viewmodels;
using BSUIRScheduleDESK.views;
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
        protected override void OnStartup(StartupEventArgs e)
        {
            IsAnotherProcessExist();
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            App.Current.Resources.MergedDictionaries[0] = new ResourceDictionary() { Source = new Uri($"Themes/ColourDictionaries/{Config.Instance.CurrentTheme}.xaml", UriKind.Relative)};
            
            string? dataPath = Directory.GetCurrentDirectory() + @"\data";
            if (!Directory.Exists(dataPath))
            {
                Directory.CreateDirectory(dataPath);
            }

            if(Config.Instance.CurrentWeek == 0 || Config.Instance.LastStartup == DateTime.MinValue)
            {
                var up = ScheduleService.UpdateCurrentWeekAsync();
            }

            int wd = DateService.GetWeekDiff(Config.Instance.LastStartup, DateTime.Today);
            Config.Instance.CurrentWeek = (Config.Instance.CurrentWeek + wd + 3) % 4 + 1;
            Config.Instance.Save();

            MainWindowViewModel mwvm = new MainWindowViewModel();
            Config.Instance.LastStartup = DateTime.Today;
            Config.Instance.Save();
            this.MainWindow = new MainWindow();
            this.MainWindow.DataContext = mwvm;
            MainWindow.Show();
            ShowUpdateInfo();
            base.OnStartup(e);
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
