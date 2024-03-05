using System.Windows;
using System.Windows.Input;

namespace BSUIRScheduleDESK.views
{
    /// <summary>
    /// Логика взаимодействия для AnnouncementWindow.xaml
    /// </summary>
    public partial class AnnouncementWindow : Window
    {
        public AnnouncementWindow()
        {
            InitializeComponent();
            this.Owner = App.Current.MainWindow;
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if(e.ChangedButton == MouseButton.Left)
            {
                DragMove();
            }
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Escape)
            {
                this.DialogResult = true;
            }
        }
    }
}
