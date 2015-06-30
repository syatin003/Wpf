using EventManagementSystem.Core.Commands;
using EventManagementSystem.Core.Unity;
using EventManagementSystem.Core.ViewModels;
using EventManagementSystem.Data.UnitOfWork.Interfaces;
using EventManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;
using EventManagementSystem.Data.Model;
using EventManagementSystem.Services;
using EventManagementSystem.Core.Serialization;
using System.Collections.ObjectModel;

namespace EventManagementSystem.ViewModels.Membership.MembershipTabs
{
    public class AddMemberNoteViewModel : ViewModelBase
    {

        #region Fields

        private readonly IMembershipDataUnit _membershipDataUnit;
        private MemberModel _member;
        private bool _isBusy;
        private MemberNoteModel _memberNote;
        private MemberNoteModel _originalMemberNote;
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

        public MemberModel Member
        {
            get { return _member; }
            set
            {
                if (_member == value) return;
                _member = value;
                RaisePropertyChanged(() => Member);
            }
        }

        public MemberNoteModel MemberNote
        {
            get { return _memberNote; }
            set
            {
                if (_memberNote == value) return;
                _memberNote = value;
                RaisePropertyChanged(() => MemberNote);
            }
        }

        public RelayCommand SubmitCommand { get; private set; }
        public RelayCommand CancelCommand { get; private set; }

        #endregion

        #region Constructor

        public AddMemberNoteViewModel(MemberModel memberModel, MemberNoteModel noteModel)
        {
            Member = memberModel;
            var dataUnitLocator = ContainerAccessor.Instance.GetContainer().Resolve<IDataUnitLocator>();
            _membershipDataUnit = dataUnitLocator.ResolveDataUnit<IMembershipDataUnit>();

            SubmitCommand = new RelayCommand(SubmitCommandExecuted, SubmitCommandCanExecute);
            CancelCommand = new RelayCommand(CancelCommandExecuted);

            ProcessNote(noteModel);
        }

        #endregion

        #region Methods

        private void ProcessNote(MemberNoteModel noteModel)
        {
            _isEditMode = (noteModel != null);

            MemberNote = (_isEditMode) ? noteModel : GetNote();

            if (_isEditMode)
                _originalMemberNote = MemberNote.Clone();

            MemberNote.PropertyChanged += MemberNoteOnPropertyChanged;
        }

        private MemberNoteModel GetNote()
        {
            var noteModel = new MemberNoteModel(new MemberNote()
            {
                ID = Guid.NewGuid(),
                MemberID = _member.Member.ID,
                CreationDate = DateTime.Now,
                AddedByID = AccessService.Current.User.ID
            });

            return noteModel;
        }

        private void MemberNoteOnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            SubmitCommand.RaiseCanExecuteChanged();
        }

        #endregion

        #region Commands

        private async void SubmitCommandExecuted()
        {
            IsBusy = true;

            if (!_isEditMode)
                _membershipDataUnit.MemberNotesRepository.Add(MemberNote.MemberNote);
            else
            {
                MemberNote.MemberNote.EditedDate = DateTime.Now;
                MemberNote.MemberNote.EditedByID = AccessService.Current.User.ID;
            }

            await _membershipDataUnit.SaveChanges();

            IsBusy = false;
        }

        private bool SubmitCommandCanExecute()
        {
            if (IsBusy)
                return false;
            return !MemberNote.HasErrors;
        }

        private void CancelCommandExecuted()
        {
            if (_originalMemberNote != null)
                MemberNote.Note = _originalMemberNote.Note;
        }

        #endregion
    }
}
