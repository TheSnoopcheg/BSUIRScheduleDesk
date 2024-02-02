using BSUIRScheduleDESK.classes;
using BSUIRScheduleDESK.services;
using System.Collections.ObjectModel;

namespace BSUIRScheduleDESK.models
{
    public class NoteModel
    {
        private readonly ObservableCollection<Note> _notes = new ObservableCollection<Note>();
        public readonly ReadOnlyObservableCollection<Note> Notes;
        private string? _url;
        public NoteModel(ObservableCollection<Note> notes, string? url)
        {
            _notes = notes;
            _url = url;
            Notes = new ReadOnlyObservableCollection<Note>(_notes);
        }
        public void AddNote(Note note)
        {
            _notes.Add(note);
            var saveTask = NoteService.SaveNotes(_notes, _url);
        }
        public void RemoveNote(Note note)
        {
            _notes.Remove(note);
            var saveTask = NoteService.SaveNotes(_notes, _url);
        }
        public void EditNote(Note oldvalue, Note newvalue)
        {
            int noteIndex = _notes.IndexOf(oldvalue);
            _notes[noteIndex] = newvalue;
            var saveTask = NoteService.SaveNotes(_notes, _url);
        }
    }
}
