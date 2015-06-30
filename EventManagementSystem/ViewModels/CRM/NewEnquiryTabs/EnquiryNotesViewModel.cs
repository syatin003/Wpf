using EventManagementSystem.Core.Commands;
using EventManagementSystem.Core.Unity;
using EventManagementSystem.Core.ViewModels;
using EventManagementSystem.Data.UnitOfWork.Interfaces;
using EventManagementSystem.Models;
using EventManagementSystem.Views.CRM.NewEnquiryTabs.Notes;
using Microsoft.Practices.Unity;
using EventManagementSystem.Core.Serialization;
using System.Linq;
using EventManagementSystem.Services;
using EventManagementSystem.Properties;

namespace EventManagementSystem.ViewModels.CRM.NewEnquiryTabs
{
    public class EnquiryNotesViewModel : ViewModelBase
    {
        #region Fields

        private EnquiryModel _enquiry;
        private readonly ICrmDataUnit _crmDataUnit;
        private EnquiryNoteModel _originalEventNote;
        private EnquiryModel _originalEnquiryAfterNotes;
        #endregion

        #region Properties

        public EnquiryModel Enquiry
        {
            get { return _enquiry; }
            set
            {
                if (_enquiry == value) return;
                _enquiry = value;
                RaisePropertyChanged(() => Enquiry);
            }
        }

        public bool CanDeleteNotes { get; private set; }
        public bool CanEditEveryoneNotes { get; private set; }
        public bool CanEditOwnNotes { get; private set; }

        public RelayCommand AddNoteCommand { get; private set; }
        public RelayCommand<EnquiryNoteModel> DeleteNoteCommand { get; private set; }
        public RelayCommand<EnquiryNoteModel> EditNoteCommand { get; private set; }

        #endregion

        #region Constructor

        public EnquiryNotesViewModel()
        {
            var dataUnitLocator = ContainerAccessor.Instance.GetContainer().Resolve<IDataUnitLocator>();
            _crmDataUnit = dataUnitLocator.ResolveDataUnit<ICrmDataUnit>();

            CanDeleteNotes = AccessService.Current.UserHasPermissions(Resources.PERMISSION_DELETE_NOTES_ALLOWED);
            CanEditEveryoneNotes = AccessService.Current.UserHasPermissions(Resources.PERMISSION_EDIT_EVERYONE_NOTE_ALLOWED);
            CanEditOwnNotes = AccessService.Current.UserHasPermissions(Resources.PERMISSION_EDIT_OWN_NOTE_ALLOWED);

            AddNoteCommand = new RelayCommand(AddNoteCommandExecuted);
            DeleteNoteCommand = new RelayCommand<EnquiryNoteModel>(DeleteNoteCommandExecuted);
            EditNoteCommand = new RelayCommand<EnquiryNoteModel>(EditNoteCommandExecuted);
        }

        #endregion

        #region Commands

        private void AddNoteCommandExecuted()
        {
            RaisePropertyChanged("DisableParentWindow");
            _originalEnquiryAfterNotes = Enquiry.Clone();
            var window = new AddNoteView(Enquiry);
            window.ShowDialog();

            RaisePropertyChanged("EnableParentWindow");
        }

        private void EditNoteCommandExecuted(EnquiryNoteModel item)
        {
            RaisePropertyChanged("DisableParentWindow");
            _originalEventNote = item.Clone();
            var window = new AddNoteView(Enquiry, item);
            window.ShowDialog();
            if (window.DialogResult != null && !window.DialogResult.Value)
            {
                item.EnquiryNote.Note = _originalEventNote.EnquiryNote.Note;
                item.Refresh();
            }

            RaisePropertyChanged("EnableParentWindow");
        }

        private void DeleteNoteCommandExecuted(EnquiryNoteModel item)
        {
            Enquiry.EnquiryNotes.Remove(item);
            _crmDataUnit.EnquiryNotesRepository.Delete(item.EnquiryNote);
        }

        #endregion
    }
}
