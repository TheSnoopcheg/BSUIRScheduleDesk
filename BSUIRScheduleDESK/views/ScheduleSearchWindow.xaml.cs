using BSUIRScheduleDESK.classes;
using BSUIRScheduleDESK.services;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace BSUIRScheduleDESK.views
{
    /// <summary>
    /// Логика взаимодействия для ScheduleSearchWindow.xaml
    /// </summary>
    public partial class ScheduleSearchWindow : Window, INotifyPropertyChanged
    {
        private ObservableCollection<SearchResponse> _results = new ObservableCollection<SearchResponse>();
        public ObservableCollection<SearchResponse> Results
        {
            get => _results;
            set
            {
                _results = value;
                OnPropertyChanged(nameof(Results));
            }
        }
        private SearchResponse? _searchResponse;
        public SearchResponse? FSearchResponce
        {
            get => _searchResponse;
            set
            {
                if (value == null)
                    return;
                _searchResponse = value;
            }
        }
        public ScheduleSearchWindow()
        {
            InitializeComponent();
            this.DataContext = this;
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

        private string? _input;
        public string? Input
        {
            get => _input;
            set
            {
                _input = value;
                OnInputChanged();
            }
        }
        private async void OnInputChanged()
        {
            if (_input != null)
            {
                if (_input.Length > 1)
                {
                    if(!searchBox.IsDropDownOpen)
                        searchBox.IsDropDownOpen = true;
                    if (char.IsDigit(_input[0]))
                    {
                        Results = await NetworkService.GetAsync<ObservableCollection<SearchResponse>>($"https://iis.bsuir.by/api/v1/student-groups/filters?name={_input}");
                    }
                    else
                    {
                        Results = await NetworkService.GetAsync<ObservableCollection<SearchResponse>>($"https://iis.bsuir.by/api/v1/employees/fio?employee-fio={_input}");
                    }
                    searchBox.SelectedIndex = 0;
                }
                else
                    searchBox.IsDropDownOpen = false;
            }
        }
        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
