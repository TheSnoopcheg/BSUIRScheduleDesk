using BSUIRScheduleDESK.services;
using BSUIRScheduleDESK.viewmodels;
using System;
using System.IO;
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
            string? path = Directory.GetCurrentDirectory() + @"\data";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            File.WriteAllText($"{Directory.GetCurrentDirectory()}\\crash.log", $"[{DateTime.Now}] {e.ExceptionObject}");
            MessageBox.Show("Упс.. Отправьте, пожалуйста, файл crash.log в телеграм @snoopcheg\nДля запуска попробуйте удалить recent.json в папке data", "Critical error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
