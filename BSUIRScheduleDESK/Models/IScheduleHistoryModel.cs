using BSUIRScheduleDESK.Classes;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace BSUIRScheduleDESK.Models;

public interface IScheduleHistoryModel
{
    ObservableCollection<HistoryNote> HistoryNotes { get; }
    void AddHistoryNote(HistoryNote note);
    Task<bool> LoadHistory(string? url);
}
