using BSUIRScheduleDESK.Classes;
using BSUIRScheduleDESK.Services;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace BSUIRScheduleDESK.Models;

public class ScheduleHistoryModel : IScheduleHistoryModel
{
    private IScheduleHistoryService _service;
    private ObservableCollection<HistoryNote> _historyNotes = new ObservableCollection<HistoryNote>();
    public ObservableCollection<HistoryNote> HistoryNotes
    {
        get { return _historyNotes; }
        set
        {
            _historyNotes = value;
        }
    }
    private string? _url;
    public ScheduleHistoryModel(IScheduleHistoryService scheduleHistoryService)
    {
        _service = scheduleHistoryService;
    }


    public async Task<bool> LoadHistory(string? url)
    {
        _url = url;
        var notes = await _service.LoadHistoryAsync(url);
        if (notes == null) return false;
        HistoryNotes = new ObservableCollection<HistoryNote>(notes);
        return true;
    }

    public void AddHistoryNote(HistoryNote note)
    {
        _historyNotes.Add(note);
        var saveTask = _service.SaveHistoryAsync(_historyNotes, _url);
    }
}
