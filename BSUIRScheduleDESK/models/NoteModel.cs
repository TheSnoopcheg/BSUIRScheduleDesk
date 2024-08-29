using BSUIRScheduleDESK.classes;
using BSUIRScheduleDESK.services;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace BSUIRScheduleDESK.models
{
    public class NoteModel : INoteModel
    {
        public event Action NotesChanged;
        private  ObservableCollection<Note> _notes = new ObservableCollection<Note>();
        public ObservableCollection<Note> Notes
        {
            get { return _notes; }
            set { _notes = value; }
        }
        private string? _url;

        private readonly INoteService _noteService;
        public NoteModel(INoteService noteService)
        {
            _noteService = noteService;
        }

        public async Task<bool> LoadNotesAsync(string? url)
        {
            _url = url;
            var notes = await _noteService.LoadNotesAsync(url);
            if (notes == null) return false;
            Notes = new ObservableCollection<Note>(notes);
            return true;
        }
        public void AddNote(Note note)
        {
            _notes.Add(note);
            _notes.OrderBy(s => s.Date);
            NotesChanged.Invoke();
            var saveTask = _noteService.SaveNotesAsync(_notes, _url);
        }
        public void RemoveNote(Note note)
        {
            _notes.Remove(note);
            NotesChanged.Invoke();
            var saveTask = _noteService.SaveNotesAsync(_notes, _url);
        }
        public void EditNote(Note oldValue, Note newValue)
        {
            int noteIndex = _notes.IndexOf(oldValue);
            _notes[noteIndex] = newValue;
            _notes.OrderBy(s => s.Date);
            NotesChanged.Invoke();
            var saveTask = _noteService.SaveNotesAsync(_notes, _url);
        }
    }
}
