using EventManagementSystem.Core.Commands;
using EventManagementSystem.Core.ViewModels;
using EventManagementSystem.Data.UnitOfWork.Interfaces;
using EventManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;
using EventManagementSystem.Views.Membership.MembershipTabs;
using EventManagementSystem.Core.Unity;
using Telerik.Windows.Controls;
using System.Windows;
using EventManagementSystem.Services;
using EventManagementSystem.Properties;
using System.Collections.ObjectModel;
using EventManagementSystem.Data.Model;
using EventManagementSystem.Core.Serialization;

namespace EventManagementSystem.ViewModels.Membership.MembershipTabs
{
    public class MemberNotesViewModel : EventManagementSystem.Core.ViewModels.ViewModelBase
    {

        #region Fields

        private readonly IMembershipDataUnit _membershipDataUnit;
        private MemberModel _member;
        private MemberNoteModel _memberNote;
        private MemberModel _originalMember;

        #endregion

        #region Properties

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

        public MemberNoteModel SelectedNote
        {
            get { return _memberNote; }
            set
            {
                if (_memberNote == value) return;
                _memberNote = value;
                RaisePropertyChanged(() => SelectedNote);
            }
        }

        public RelayCommand AddNoteCommand { get; private set; }
        public RelayCommand<MemberNoteModel> DeleteNoteCommand { get; private set; }
        public RelayCommand<MemberNoteModel> EditNoteCommand { get; private set; }

        #endregion

        #region Constructor

        public MemberNotesViewModel(MemberModel member)
        {
            var dataUnitLocator = ContainerAccessor.Instance.GetContainer().Resolve<IDataUnitLocator>();
            _membershipDataUnit = dataUnitLocator.ResolveDataUnit<IMembershipDataUnit>();

            AddNoteCommand = new RelayCommand(AddNoteCommandExecuted);
            DeleteNoteCommand = new RelayCommand<MemberNoteModel>(DeleteNoteCommandExecuted);

            EditNoteCommand = new RelayCommand<MemberNoteModel>(EditNoteCommandExecuted);

            ProcessMember(member);
        }

        #endregion

        #region Methods

        private void ProcessMember(MemberModel member)
        {
            Member = member;
        }

        private async void ProcessUpdates()
        {
            if (!Member.MembershipUpdates.Any())
            {
                var updates = await _membershipDataUnit.MembershipUpdatesRepository.GetAllAsync(x => x.MemberID == Member.Member.ID);
                Member.MembershipUpdates = new ObservableCollection<MembershipUpdate>(updates.OrderByDescending(x => x.Date));
            }
            var membershipUpdates = LoggingService.FindDifference(_originalMember, Member, "MemberNotes");
            membershipUpdates.ForEach(update =>
            {
                Member.MembershipUpdates.Insert(0, update);
                _membershipDataUnit.MembershipUpdatesRepository.Add(update);
            });

            Member.MembershipUpdates = new ObservableCollection<MembershipUpdate>(Member.MembershipUpdates.OrderByDescending(x => x.Date));

            await _membershipDataUnit.SaveChanges();
        }

        #endregion

        #region Commands

        private void AddNoteCommandExecuted()
        {
            RaisePropertyChanged("DisableParentWindow");

            _originalMember = Member.Clone();
            var window = new AddMemberNoteView(Member);
            window.ShowDialog();

            RaisePropertyChanged("EnableParentWindow");

            if (window.DialogResult != null && window.DialogResult.Value)
            {
                Member.MemberNotes.Add(window.ViewModel.MemberNote);
                ProcessUpdates();
                SelectedNote = window.ViewModel.MemberNote;
            }
        }

        private void EditNoteCommandExecuted(MemberNoteModel memberNote)
        {
            RaisePropertyChanged("DisableParentWindow");
            SelectedNote = memberNote;
            _originalMember = Member.Clone();
            var window = new AddMemberNoteView(Member, memberNote);
            window.ShowDialog();

            if (window.DialogResult != null && window.DialogResult.Value)
            {
                ProcessUpdates();
                memberNote.Refresh();
            }
            RaisePropertyChanged("EnableParentWindow");
        }

        private void DeleteNoteCommandExecuted(MemberNoteModel memberNote)
        {
            bool? dialogResult = null;
            string confirmText = Properties.Resources.MESSAGE_ASK_BEFORE_DELETING_ITEM;

            RaisePropertyChanged("DisableParentWindow");

            SelectedNote = memberNote;
            RadWindow.Confirm(new DialogParameters()
            {
                Owner = Application.Current.MainWindow,
                Content = confirmText,
                Closed = (sender, args) => { dialogResult = args.DialogResult; }
            });

            RaisePropertyChanged("EnableParentWindow");

            if (dialogResult != true) return;

            Member.MemberNotes.Remove(memberNote);
            _membershipDataUnit.MemberNotesRepository.Delete(memberNote.MemberNote);
            _membershipDataUnit.SaveChanges();
        }

        #endregion
    }
}
