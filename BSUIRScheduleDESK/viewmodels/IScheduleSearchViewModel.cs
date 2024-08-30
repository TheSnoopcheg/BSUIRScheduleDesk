using BSUIRScheduleDESK.Classes;
using System.Collections.ObjectModel;

namespace BSUIRScheduleDESK.ViewModels
{
    public interface IScheduleSearchViewModel
    {
        SearchResponse? SearchResponse { get; set; }
        ObservableCollection<SearchResponse> Responses { get; }
        string Input { get; set; }
        void ClearInput();
    }
}
