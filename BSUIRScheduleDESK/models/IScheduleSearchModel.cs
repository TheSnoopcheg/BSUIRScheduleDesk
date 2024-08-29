using BSUIRScheduleDESK.classes;
using System;
using System.Collections.ObjectModel;

namespace BSUIRScheduleDESK.models
{
    public interface IScheduleSearchModel
    {
        event Action ResponsesChanged;
        SearchResponse? SearchResponse { get; set; }
        ObservableCollection<SearchResponse> Responses { get; }
        string Input { get; set; }
    }
}
