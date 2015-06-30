using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using EventManagementSystem.Core.ViewModels;
using EventManagementSystem.Data.Model;

namespace EventManagementSystem.Models
{
    [Serializable]
    public class EventNoteModel : ModelBase, IDataErrorInfo
    {
        #region Fields

        private readonly EventNote _eventNote;

        #endregion

        #region Properties

        [DataMember]
        public EventNote EventNote
        {
            get { return _eventNote; }
        }

        [DataMember]
        public EventNoteType NoteType
        {
            get { return _eventNote.EventNoteType; }
            set
            {
                if (_eventNote.EventNoteType == value) return;
                _eventNote.EventNoteType = value;
                RaisePropertyChanged(() => NoteType);
            }
        }

        [DataMember]
        public string Note
        {
            get { return _eventNote.Note; }
            set
            {
                if (_eventNote.Note == value) return;
                _eventNote.Note = value;
                RaisePropertyChanged(() => Note);
            }
        }

        #endregion

        #region Constructor

        public EventNoteModel(EventNote eventNote)
        {
            _eventNote = eventNote;
        }

        #endregion

        #region Methods

        public void Refresh()
        {
            RaisePropertyChanged(() => NoteType);
            RaisePropertyChanged(() => Note);
        }

        #endregion

        #region IDataErrorInfo

        public bool HasErrors
        {
            get { return typeof(EventNoteModel).GetProperties().Any(prop => !string.IsNullOrEmpty(this[prop.Name])); }
        }

        public string this[string columnName]
        {
            get
            {
                Error = string.Empty;

                if (columnName == "NoteType")
                    if (NoteType == null)
                        Error = "Note type can't be empty.";

                if (columnName == "Note")
                    if (string.IsNullOrWhiteSpace(Note))
                        Error = "Note text can't be empty.";

                return Error;
            }
        }

        public string Error { get; private set; }

        #endregion
    }
}
