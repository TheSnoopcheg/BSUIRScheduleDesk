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
    }
}
