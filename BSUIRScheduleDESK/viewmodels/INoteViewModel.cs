using System;
using System.Threading.Tasks;

namespace BSUIRScheduleDESK.ViewModels
{
    public interface INoteViewModel
    {
        event Action NotesChanged;
        Task<bool> SetNotes(string? title, string? url);
        bool IsNotesEmpty { get; }
    }
}
