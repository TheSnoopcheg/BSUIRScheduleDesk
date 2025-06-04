using BSUIRScheduleDESK.Classes;
using BSUIRScheduleDESK.Models;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace BSUIRScheduleDESK.ViewModels
{
    public class ScheduleHistoryViewModel : Notifier, IScheduleHistoryViewModel
    {
        readonly IScheduleHistoryModel _model;

        public ScheduleHistoryViewModel(IScheduleHistoryModel scheduleHistoryModel)
        {
            _model = scheduleHistoryModel;
        }

        public async Task<bool> SetScheduleHistory(string? title, string? url)
        {
            Title = title;
            if (int.TryParse(title, out int id))
                IsEmployeeHistory = false;
            else
                IsEmployeeHistory = true;
            return await _model.LoadHistory(url);
        }

        public void AddHistoryNote(HistoryNote note)
        {
            _model.AddHistoryNote(note);
        }
        public ObservableCollection<HistoryNote> HistoryNotes => _model.HistoryNotes;
        public bool IsEmployeeHistory { get; private set; }
        public string? Title {  get; private set; } = string.Empty;
        public bool IsHistoryEmpty { get => HistoryNotes.Count == 0; }
    }
}
