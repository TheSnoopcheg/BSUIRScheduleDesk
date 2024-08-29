using BSUIRScheduleDESK.classes;
using System.Collections.ObjectModel;

namespace BSUIRScheduleDESK.viewmodels
{
    public interface IScheduleSearchViewModel
    {
        SearchResponse? SearchResponse { get; set; }
        ObservableCollection<SearchResponse> Responses { get; }
        string Input { get; set; }
        void ClearInput();
    }
}
