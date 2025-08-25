using BSUIRScheduleDESK.Classes;

namespace BSUIRScheduleDESK.ViewModels;

public interface IScheduleSearchViewModel
{
    SearchResponse? SearchResponse { get; set; }
    string Input { get; set; }
    void ClearInput();
}
