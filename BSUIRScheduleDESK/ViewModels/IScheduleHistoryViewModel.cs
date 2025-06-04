using BSUIRScheduleDESK.Classes;
using System.Threading.Tasks;

namespace BSUIRScheduleDESK.ViewModels
{
    public interface IScheduleHistoryViewModel
    {
        bool IsHistoryEmpty { get; }
        void AddHistoryNote (HistoryNote note);
        Task<bool> SetScheduleHistory(string? title, string? url);
    }
}
