using System;
using System.ComponentModel;
using System.Linq;
using EventManagementSystem.Core.Commands;
using EventManagementSystem.Core.Unity;
using EventManagementSystem.Core.ViewModels;
using EventManagementSystem.Data.Model;
using EventManagementSystem.Data.UnitOfWork.Interfaces;
using EventManagementSystem.Models;
using EventManagementSystem.Services;
using Microsoft.Practices.Unity;
using EventManagementSystem.Core.Serialization;

namespace EventManagementSystem.ViewModels.CRM.NewEnquiryTabs.Notes
{
    public class AddEnquiryNoteViewModel : ViewModelBase
    {
        #region Fields

        private readonly ICrmDataUnit _crmDataUnit;
        private readonly EnquiryModel _enquiry;
        private bool _isBusy;
        private EnquiryNoteModel _enquiryNote;
        private EnquiryNoteModel _originalEnquiryNote;

        private bool _isEditMode;

        #endregion

        #region Properties

        public bool IsBusy
        {
            get { return _isBusy; }
            set
            {
                if (_isBusy == value) return;
                _isBusy = value;
                RaisePropertyChanged(() => IsBusy);
            }
        }

        public EnquiryNoteModel EnquiryNote
        {
            get { return _enquiryNote; }
            set
            {
                if (_enquiryNote == value) return;
                _enquiryNote = value;
                RaisePropertyChanged(() => EnquiryNote);
            }
        }

        public RelayCommand SubmitCommand { get; private set; }
        public RelayCommand CancelCommand { get; private set; }

        #endregion

        #region Constructor

        public AddEnquiryNoteViewModel(EnquiryModel enquiryModel, EnquiryNoteModel noteModel)
        {
            _enquiry = enquiryModel;
            var dataUnitLocator = ContainerAccessor.Instance.GetContainer().Resolve<IDataUnitLocator>();
            _crmDataUnit = dataUnitLocator.ResolveDataUnit<ICrmDataUnit>();

            SubmitCommand = new RelayCommand(SubmitCommandExecuted, SubmitCommandCanExecute);
            CancelCommand = new RelayCommand(CancelCommandExecuted);

            ProcessNote(noteModel);
        }

        #endregion

        #region Methods

        private void ProcessNote(EnquiryNoteModel noteModel)
        {
            _isEditMode = (noteModel != null);

            EnquiryNote = (_isEditMode) ? noteModel : GetNote();
            if (_isEditMode)
                _originalEnquiryNote = EnquiryNote.Clone();
            EnquiryNote.PropertyChanged += EnquiryNoteOnPropertyChanged;
        }

        private EnquiryNoteModel GetNote()
        {
            var noteModel = new EnquiryNoteModel(new EnquiryNote()
            {
                ID = Guid.NewGuid(),
                Enquiry = _enquiry.Enquiry,
                Date = DateTime.Now,
            });

            return noteModel;
        }

        private void EnquiryNoteOnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            SubmitCommand.RaiseCanExecuteChanged();
        }

        public async void LoadData()
        {
            IsBusy = true;

            var user = (await _crmDataUnit.UsersRepository.GetUsersAsync(x => x.ID == AccessService.Current.User.ID)).FirstOrDefault();
            EnquiryNote.EnquiryNote.User = user;

            IsBusy = false;
        }

        #endregion

        #region Commands

        private void SubmitCommandExecuted()
        {
            if (!_isEditMode)
            {
                _enquiry.EnquiryNotes.Add(EnquiryNote);
                _crmDataUnit.EnquiryNotesRepository.Add(EnquiryNote.EnquiryNote);
            }
            else
            {
                _crmDataUnit.EnquiryNotesRepository.SetEntityModified(EnquiryNote.EnquiryNote);
            }
        }

        private bool SubmitCommandCanExecute()
        {
            return !EnquiryNote.HasErrors;
        }

        private void CancelCommandExecuted()
        {
            if (!_isEditMode)
            {
                _enquiry.Enquiry.EnquiryNotes.Remove(EnquiryNote.EnquiryNote);
                _enquiry.Enquiry.User.EnquiryNotes.Remove(EnquiryNote.EnquiryNote);
            }
            //_crmDataUnit.RevertChanges();
            //EnquiryNote.Refresh();
        }

        #endregion
    }
}
