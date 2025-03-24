using BSUIRScheduleDESK.Classes;
using System.Collections.ObjectModel;

namespace BSUIRScheduleDESK.ViewModels
{
    public interface IScheduleSearchViewModel
    {
        SearchResponse? SearchResponse { get; set; }
        string Input { get; set; }
        void ClearInput();
    }
}
