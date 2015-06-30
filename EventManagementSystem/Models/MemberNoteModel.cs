using EventManagementSystem.Core.ViewModels;
using EventManagementSystem.Data.Model;
using EventManagementSystem.Properties;
using EventManagementSystem.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace EventManagementSystem.Models
{
    [Serializable]
    public class MemberNoteModel : ModelBase, IDataErrorInfo
    {
        #region Fields

        private readonly MemberNote _memberNote;

        #endregion

        #region Properties

        [DataMember]
        public MemberNote MemberNote
        {
            get { return _memberNote; }
        }

        [DataMember]
        public string Note
        {
            get { return _memberNote.Note; }
            set
            {
                if (_memberNote.Note == value) return;
                _memberNote.Note = value;
                RaisePropertyChanged(() => Note);
            }
        }
        [DataMember]
        public User AddedByUser
        {
            get { return _memberNote.User; }
            set
            {
                if (_memberNote.User == value) return;
                _memberNote.User = value;
                RaisePropertyChanged(() => AddedByUser);
            }
        }

        [DataMember]
        public User EditedByUser
        {
            get { return _memberNote.User1; }
            set
            {
                if (_memberNote.User1 == value) return;
                _memberNote.User1 = value;
                RaisePropertyChanged(() => EditedByUser);
            }
        }

        public bool CanEditNote { get; private set; }

        public bool CanDeleteNote { get; private set; }

        #endregion

        #region Constructor

        public MemberNoteModel(MemberNote memberNote)
        {
            _memberNote = memberNote;

            CanEditNote = AccessService.Current.User.ID == memberNote.AddedByID || AccessService.Current.UserHasPermissions(Resources.PERMISSION_MEMBERSHIP_NOTE_OVERRIDE);

            CanDeleteNote = AccessService.Current.User.ID == memberNote.AddedByID;
        }

        #endregion

        #region Methods

        public void Refresh()
        {
            RaisePropertyChanged(() => Note);
            RaisePropertyChanged(() => AddedByUser);
            RaisePropertyChanged(() => EditedByUser);
        }

        #endregion

        #region IDataErrorInfo

        public bool HasErrors
        {
            get { return typeof(MemberNoteModel).GetProperties().Any(prop => !string.IsNullOrEmpty(this[prop.Name])); }
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
