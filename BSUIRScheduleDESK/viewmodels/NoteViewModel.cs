﻿using BSUIRScheduleDESK.classes;
using BSUIRScheduleDESK.models;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace BSUIRScheduleDESK.viewmodels
{
    public class NoteViewModel : Notifier
    {
        readonly NoteModel _model;
        public NoteViewModel(ObservableCollection<Note> notes, string? title, string? url)
        {
            _model = new NoteModel(notes, url);
            Title = title!;
            if (int.TryParse(title, out int id))
            {
                IsEmployeeNote = false;
            }
            else
                IsEmployeeNote = true;
        }

        #region Commands

        private ICommand? addNote;
        public ICommand AddNote
        {
            get
            {
                return addNote ??
                    (addNote = new RelayCommand(obj =>
                    {
                        _model.AddNote(new Note(NoteDate, NoteText));
                        NoteDate = null;
                        NoteText = string.Empty;
                    }));
            }
        }
        private ICommand? editNote;
        public ICommand EditNote
        {
            get
            {
                return editNote ??
                    (editNote = new RelayCommand(obj =>
                    {
                        if (!IsEditing)
                        {
                            IsEditing = true;
                            NoteText = SelectedNote.Content;
                            NoteDate = SelectedNote.Date;
                        }
                        else
                        {
                            _model.EditNote(SelectedNote, new Note(NoteDate, NoteText));
                            IsEditing = false;
                            NoteDate = null;
                            NoteText = string.Empty;
                        }
                    }));
            }
        }
        private ICommand? deleteNote;
        public ICommand DeleteNote
        {
            get
            {
                return deleteNote ??
                    (deleteNote = new RelayCommand(obj =>
                    {
                        if(obj is Note v)
                        {
                            _model.RemoveNote(v);
                            if (IsEditing)
                            {
                                IsEditing = false;
                                NoteDate = null;
                                NoteText = string.Empty;
                            }
                        }
                    }));
            }
        }

        #endregion
        private string? _noteText;
        public string? NoteText
        {
            get { return _noteText; }
            set
            {
                _noteText = value;
                OnPropertyChanged();
            }
        }
        private DateTime? _noteDate;
        public DateTime? NoteDate
        {
            get { return _noteDate; }
            set
            {
                _noteDate = value;
                OnPropertyChanged();
            }
        }
        public ReadOnlyObservableCollection<Note> Notes => _model.Notes;
        private bool _isEditing;
        public bool IsEditing
        {
            get { return _isEditing; }
            set
            {
                _isEditing = value;
                OnPropertyChanged();
            }
        }
        private Note selectedNote;
        public Note SelectedNote
        {
            get { return selectedNote; }
            set
            {
                selectedNote = value;
                OnPropertyChanged();
            }
        }
        public bool IsEmployeeNote { get; private set; }
        public string Title { get; private set; }
    }
}