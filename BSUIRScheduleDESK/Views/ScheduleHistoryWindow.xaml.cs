using System.Windows;
using System.Windows.Input;

namespace BSUIRScheduleDESK.Views
{
    /// <summary>
    /// Interaction logic for ScheduleHistoryWindow.xaml
    /// </summary>
    public partial class ScheduleHistoryWindow : Window
    {
        public ScheduleHistoryWindow()
        {
            InitializeComponent();
            this.Owner = App.Current.MainWindow;
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if(e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Escape)
            {
                this.DialogResult = true;
                this.Close();
            }
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            this.Close();
        }
    }
}
