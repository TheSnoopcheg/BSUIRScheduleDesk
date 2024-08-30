using BSUIRScheduleDESK.Classes;
using BSUIRScheduleDESK.Models;
using System.Collections.ObjectModel;

namespace BSUIRScheduleDESK.ViewModels
{
    public class ScheduleSearchViewModel : Notifier, IScheduleSearchViewModel
    {
        private readonly IScheduleSearchModel _model;
        public SearchResponse? SearchResponse
        {
            get { return _model.SearchResponse; }
            set {  _model.SearchResponse = value; }
        }
        public ObservableCollection<SearchResponse> Responses => _model.Responses;
        public string Input
        {
            get { return _model.Input; }
            set {  _model.Input = value; }
        }
        public ScheduleSearchViewModel(IScheduleSearchModel scheduleSearchModel)
        {
            _model = scheduleSearchModel;
            scheduleSearchModel.ResponsesChanged += () => { OnPropertyChanged(nameof(Responses)); };
        }
        public void ClearInput()
        {
            Input = string.Empty;
            SearchResponse = null;
        }
    }
}
