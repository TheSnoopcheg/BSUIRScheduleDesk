using System.Windows;
using System.Windows.Input;
using System.Text.Json;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.ObjectModel;

namespace BSUIRScheduleDESK.views
{
    /// <summary>
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window, INotifyPropertyChanged
    {
        public SettingsWindow()
        {
            InitializeComponent();
            SelectedIndexes = JsonSerializer.Deserialize<ObservableCollection<int>>(Properties.Settings.Default.indexes)!;
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
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
            if (e.Key == Key.Escape)
            {
                this.DialogResult = true;
            }
        }
        private ObservableCollection<int> selectedIndexes;
        public ObservableCollection<int> SelectedIndexes
        {
            get => selectedIndexes;
            set
            {
                selectedIndexes = value;
                OnPropertyChanged();
            }
        }
        private void ColorPicker_ColorChanged()
        {
            Properties.Settings.Default.indexes = JsonSerializer.Serialize(selectedIndexes);
            Properties.Settings.Default.Save(); 
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
