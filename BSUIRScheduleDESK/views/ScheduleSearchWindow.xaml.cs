using System.Windows;
using System.Windows.Input;

namespace BSUIRScheduleDESK.Views
{
    /// <summary>
    /// Логика взаимодействия для ScheduleSearchWindow.xaml
    /// </summary>
    public partial class ScheduleSearchWindow : Window
    {
        public ScheduleSearchWindow()
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

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Escape)
            {
                this.DialogResult = false;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void searchBox_ItemSelected()
        {
            this.DialogResult = true;
        }

        private void searchBox_TextChanged()
        {
            if (searchBox.SText.Length > 1)
                searchBox.IsDropDownOpen = true;
            else
                searchBox.IsDropDownOpen = false;
        }
    }
}
