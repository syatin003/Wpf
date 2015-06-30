using System.ComponentModel;
using System.Linq;
using EventManagementSystem.Core.ViewModels;
using EventManagementSystem.Data.Model;

namespace EventManagementSystem.Models
{
    public class CalendarNoteModel : ModelBase, IDataErrorInfo
    {
        #region Fields

        private readonly CalendarNote _calendarNote;

        #endregion

        public CalendarNote CalendarNote
        {
            get { return _calendarNote; }
        }

        public string Description
        {
            get { return _calendarNote.Description; }
            set
            {
                if (_calendarNote.Description == value) return;
                _calendarNote.Description = value;
                RaisePropertyChanged(() => Description);
            }
        }

        public string Color
        {
            get { return _calendarNote.Color; }
            set
            {
                if (_calendarNote.Color == value) return;
                _calendarNote.Color = value;
                RaisePropertyChanged(() => Color);
            }
        }

        #region Constructor

        public CalendarNoteModel(CalendarNote note)
        {
            _calendarNote = note;
        }

        #endregion

        #region Implementation of IDataErrorInfo

        public virtual bool HasErrors
        {
            get { return typeof(CalendarNoteModel).GetProperties().Any(prop => !string.IsNullOrEmpty(this[prop.Name])); }
        }

        public string this[string columnName]
        {
            get
            {
                Error = string.Empty;
                
                if (columnName == "Description")
                    if (string.IsNullOrWhiteSpace(Description))
                        Error = "Description can't be empty";

                if (columnName == "Color")
                    if (string.IsNullOrWhiteSpace(Color))
                        Error = "Color can't be empty";

                return Error;
            }
        }

        public string Error { get; private set; }

        #endregion

    }
}
