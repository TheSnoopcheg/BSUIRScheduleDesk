using BSUIRScheduleDESK.classes;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace BSUIRScheduleDESK.services
{
    public interface INoteService
    {
        Task<ObservableCollection<Note>> LoadNotesAsync(string? path);
        Task SaveNotesAsync(ObservableCollection<Note> notes, string? path);
    }
}
