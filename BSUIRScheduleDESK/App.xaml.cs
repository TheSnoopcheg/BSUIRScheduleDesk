using BSUIRScheduleDESK.viewmodels;
using System.IO;
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
