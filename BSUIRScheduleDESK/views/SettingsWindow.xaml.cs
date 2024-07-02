using System.Windows;
using System.Windows.Input;
using System.Text.Json;
using System.ComponentModel;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Collections.ObjectModel;
using System;
using BSUIRScheduleDESK.Themes;

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
            this.Owner = App.Current.MainWindow;
            SelectedIndexes = JsonSerializer.Deserialize<ObservableCollection<int>>(Properties.Settings.Default.indexes)!;
            ThemeIndex = GetThemeIndex();
        }
        private int GetThemeIndex()
        {
            switch(Properties.Settings.Default.currentTheme)
            {
                case "IISTheme": return 0;
                case "DarkTheme": return 1;
                default: throw new ArgumentOutOfRangeException();
            }
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
        private int themeIndex = -1;
        public int ThemeIndex
        {
            get { return themeIndex; }
            set
            {
                themeIndex = value;
                OnPropertyChanged();
            }
        }
        private List<string> themes = new List<string>()
        {
            "ИИС",
            "Тёмная"
        };
        public List<string> Themes { get => themes; }
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

        private void ComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            switch (ThemeIndex)
            {
                case 0: ThemeManager.SetTheme(ThemeType.IIS); break;
                case 1: ThemeManager.SetTheme(ThemeType.Dark); break;
                default: throw new ArgumentOutOfRangeException();
            }
        }

        public Version Version { get; set; } = new Version(1,0,3);
    }
}
