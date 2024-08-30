using BSUIRScheduleDESK.Classes;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace BSUIRScheduleDESK.Models
{
    public interface INoteModel
    {
        event Action NotesChanged;
        ObservableCollection<Note> Notes { get; }
        void AddNote(Note note);
        void RemoveNote(Note note);
        void EditNote(Note oldValue, Note newValue);
        Task<bool> LoadNotesAsync(string? url);
    }
}
