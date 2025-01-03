using BSUIRScheduleDESK.Themes;
using BSUIRScheduleDESK.Classes;
using BSUIRScheduleDESK.Services;
using BSUIRScheduleDESK.Langs;
using System.Windows;
using System.Text.Json;
using System.Windows.Input;
using System.ComponentModel;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Collections.ObjectModel;
using System;

namespace BSUIRScheduleDESK.Views
{
    /// <summary>
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window, INotifyPropertyChanged
    {
        public SettingsWindow()
        {
            themes = new List<string>
            {
                Langs.Language.IISTheme,
                Langs.Language.DarkTheme
            };
            languages = new List<string>
            {
                Langs.Language.Russian,
                Langs.Language.Belarusian,
                Langs.Language.English
            };
            InitializeComponent();
            this.Owner = App.Current.MainWindow;
            SelectedIndexes = JsonSerializer.Deserialize<ObservableCollection<int>>(Config.Instance.Indexes)!;
            ThemeIndex = GetThemeIndex();
            LanguageIndex = GetLanguageIndex();
        }
        private int GetThemeIndex()
        {
            switch (Config.Instance.CurrentTheme)
            {
                case "IISTheme": return 0;
                case "DarkTheme": return 1;
                default: throw new ArgumentOutOfRangeException();
            }
        }
        private int GetLanguageIndex()
        {
            switch (Config.Instance.CurrentLanguage)
            {
                case "ru": return 0;
                case "be": return 1;
                case "en": return 2;
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
        private int languageIndex = -1;
        public int LanguageIndex
        {
            get { return languageIndex; }
            set
            {
                languageIndex = value;
                OnPropertyChanged();
            }
        }
        private List<string> themes;
        public List<string> Themes { get => themes; }
        private List<string> languages;
        public List<string> Languages { get => languages; }
        private void ColorPicker_ColorChanged()
        {
            EventService.ColorsUpdated_Invoke();
            Config.Instance.Indexes = JsonSerializer.Serialize(selectedIndexes);
            Config.Instance.Save(); 
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void ThemeComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            switch (ThemeIndex)
            {
                case 0: ThemeManager.SetTheme(ThemeType.IIS); break;
                case 1: ThemeManager.SetTheme(ThemeType.Dark); break;
                default: throw new ArgumentOutOfRangeException();
            }
        }
        private void LanguageComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            switch (LanguageIndex)
            {
                case 0: LanguageManager.SetLanguage(LanguageType.RUS); break;
                case 1: LanguageManager.SetLanguage(LanguageType.BEL); break;
                case 2: LanguageManager.SetLanguage(LanguageType.ENG); break;
                default: throw new ArgumentOutOfRangeException();
            }
        }

        public Version Version { get; set; } = new Version(1,0,5,2);

    }
}
