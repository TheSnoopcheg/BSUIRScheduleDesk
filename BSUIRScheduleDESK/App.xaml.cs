using BSUIRScheduleDESK.services;
using BSUIRScheduleDESK.viewmodels;
using System;
using System.Diagnostics;
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
            if (n.Default.currentweek == 0)
            {
                var up = ScheduleService.UpdateCurrentWeekAsync();
            }
            int wd = DateService.GetWeekDiff(n.Default.laststartup, DateTime.Today);
            if (wd != 0)
            {
                n.Default.currentweek += wd % 4;
            }
            n.Default.laststartup = DateTime.Today;
            n.Default.Save();
            MainWindowViewModel mwvm = new MainWindowViewModel();
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
    }
}
