using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using EventManagementSystem.Core.ViewModels;
using EventManagementSystem.Data.Model;

namespace EventManagementSystem.Models
{
    [Serializable]
    public class EnquiryNoteModel : ModelBase, IDataErrorInfo
    {
        #region Fields

        private readonly EnquiryNote _enquiryNote;

        #endregion

        #region Properties

        [DataMember]
        public EnquiryNote EnquiryNote
        {
            get { return _enquiryNote; }
        }     

        [DataMember]
        public string Note
        {
            get { return _enquiryNote.Note; }
            set
            {
                if (_enquiryNote.Note == value) return;
                _enquiryNote.Note = value;
                RaisePropertyChanged(() => Note);
            }
        }

        #endregion

        #region Constructor

        public EnquiryNoteModel(EnquiryNote enquiryNote)
        {
            _enquiryNote = enquiryNote;
        }

        #endregion

        #region Methods

        public void Refresh()
        {
            RaisePropertyChanged(() => Note);
        }

        #endregion

        #region IDataErrorInfo

        public bool HasErrors
        {
            get { return typeof(EnquiryNoteModel).GetProperties().Any(prop => !string.IsNullOrEmpty(this[prop.Name])); }
        }

        public string this[string columnName]
        {
            get
            {
                Error = string.Empty;
               
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
