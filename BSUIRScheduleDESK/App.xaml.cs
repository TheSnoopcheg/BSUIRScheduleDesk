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
using n = BSUIRScheduleDESK.Properties.Settings;

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
            App.Current.Resources.MergedDictionaries[0] = new ResourceDictionary() { Source = new Uri($"Themes/ColourDictionaries/{n.Default.currentTheme}.xaml", UriKind.Relative)};
            if (n.Default.currentweek == 0 || n.Default.laststartup == DateTime.MinValue)
            {
                var up = ScheduleService.UpdateCurrentWeekAsync();
            }
            int wd = DateService.GetWeekDiff(n.Default.laststartup, DateTime.Today);
            if (wd != 0)
            {
                n.Default.currentweek += wd % 4;
                if(n.Default.currentweek >= 5)
                {
                    n.Default.currentweek -= 4;
                    n.Default.Save();
                }
            }
            MainWindowViewModel mwvm = new MainWindowViewModel();
            n.Default.laststartup = DateTime.Today;
            n.Default.Save();
            this.MainWindow = new MainWindow();
            this.MainWindow.DataContext = mwvm;
            MainWindow.Show();
            base.OnStartup(e);
            ShowUpdateInfo();
            string? path = Directory.GetCurrentDirectory() + @"\data";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }
        private void IsAnotherProcessExist()
        {
            string? processName = Process.GetCurrentProcess().ProcessName;
            Process[] processes = Process.GetProcesses().Where(u => u.ProcessName == processName).ToArray();
            if(processes.Length > 1)
            {
                MessageBox.Show("Данная программа уже запущена.", "Расписание БГУИР", MessageBoxButton.OK, MessageBoxImage.Error);
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
                        ModalWindow modalWindow = new ModalWindow($"Обновление от {updateInfo.UpdateDate}", Directory.GetCurrentDirectory() + @"\updates\update.png", updateInfo.Content, ModalWindowButtons.OK);
                        modalWindow.ShowDialog();
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
            MessageBox.Show("Упс.. Отправьте, пожалуйста, файл crash.log в телеграм @snoopcheg\nДля запуска попробуйте удалить recent.json в папке data", "Critical error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
