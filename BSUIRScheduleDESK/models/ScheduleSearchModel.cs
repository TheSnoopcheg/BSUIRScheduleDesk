using BSUIRScheduleDESK.Classes;
using BSUIRScheduleDESK.Services;
using System;
using System.Collections.ObjectModel;

namespace BSUIRScheduleDESK.Models
{
    public class ScheduleSearchModel : IScheduleSearchModel
    {
        public event Action ResponsesChanged;
        private readonly INetworkService _networkService;
        public SearchResponse? SearchResponse { get; set; }
        private ObservableCollection<SearchResponse> _responses = new ObservableCollection<SearchResponse>();
        public ObservableCollection<SearchResponse> Responses
        {
            get { return _responses; }
            set
            {
                if (value == null) return;
                _responses = value;
                ResponsesChanged.Invoke();
            }
        }

        private string _input = string.Empty;
        public string Input
        {
            get { return _input; }
            set
            {
                _input = value;
                OnInputChanged();
            }
        }
        private async void OnInputChanged()
        {
            if (_input.Length < 2) return;
            if (char.IsDigit(_input[0]))
            {
                Responses = await _networkService.GetAsync<ObservableCollection<SearchResponse>>($"https://iis.bsuir.by/api/v1/student-groups/filters?name={_input}");
            }
            else
            {
                Responses = await _networkService.GetAsync<ObservableCollection<SearchResponse>>($"https://iis.bsuir.by/api/v1/employees/fio?employee-fio={_input}");
            }
            if (Responses.Count > 0)
                SearchResponse = Responses[0];
        }

        public ScheduleSearchModel(INetworkService networkService)
        {
            _networkService = networkService;
        }
    }
}
