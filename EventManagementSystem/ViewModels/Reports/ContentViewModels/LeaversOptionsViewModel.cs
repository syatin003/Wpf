using EventManagementSystem.Controls;
using EventManagementSystem.Core.Commands;
using EventManagementSystem.Core.ViewModels;
using EventManagementSystem.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventManagementSystem.ViewModels.Reports.ContentViewModels
{
    public class LeaversOptionsViewModel : ViewModelBase
    {
        #region Fields

        private bool _isBusy;
        private bool _incResignDate;
        private bool _incLeavingDate;
        private bool _incMembershipNumberLeavers;
        private bool _incMemberNameLeavers;
        private bool _incCategoryNameLeavers;
        private bool _incReason;
        private bool _incNotes;
        private bool _incLinkedMembers;
        private bool _incMembershipStart;
        private bool _incMembershipEnd;
        private bool _incContractPeriodLeavers;
        private bool _incLastDDMonth;

        #endregion Fields

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

        public bool IncResignDate
        {
            get { return _incResignDate; }
            set
            {
                if (_incResignDate == value) return;
                _incResignDate = value;
                RaisePropertyChanged(() => IncResignDate);

            }
        }

        public bool IncLeavingDate
        {
            get { return _incLeavingDate; }
            set
            {
                if (_incLeavingDate == value) return;
                _incLeavingDate = value;
                RaisePropertyChanged(() => IncLeavingDate);

            }
        }

        public bool IncMembershipNumberLeavers
        {
            get { return _incMembershipNumberLeavers; }
            set
            {
                if (_incMembershipNumberLeavers == value) return;
                _incMembershipNumberLeavers = value;
                RaisePropertyChanged(() => IncMembershipNumberLeavers);

            }
        }

        public bool IncMemberNameLeavers
        {
            get { return _incMemberNameLeavers; }
            set
            {
                if (_incMemberNameLeavers == value) return;
                _incMemberNameLeavers = value;
                RaisePropertyChanged(() => IncMemberNameLeavers);

            }
        }

        public bool IncCategoryNameLeavers
        {
            get { return _incCategoryNameLeavers; }
            set
            {
                if (_incCategoryNameLeavers == value) return;
                _incCategoryNameLeavers = value;
                RaisePropertyChanged(() => IncCategoryNameLeavers);
            }
        }

        public bool IncReason
        {
            get { return _incReason; }
            set
            {
                if (_incReason == value) return;
                _incReason = value;
                RaisePropertyChanged(() => IncReason);
            }
        }

        public bool IncNotes
        {
            get { return _incNotes; }
            set
            {
                if (_incNotes == value) return;
                _incNotes = value;
                RaisePropertyChanged(() => IncNotes);
            }
        }

        public bool IncLinkedMembers
        {
            get { return _incLinkedMembers; }
            set
            {
                if (_incLinkedMembers == value) return;
                _incLinkedMembers = value;
                RaisePropertyChanged(() => IncLinkedMembers);
            }
        }

        public bool IncMembershipStart
        {
            get { return _incMembershipStart; }
            set
            {
                if (_incMembershipStart == value) return;
                _incMembershipStart = value;
                RaisePropertyChanged(() => IncMembershipStart);
            }
        }


        public bool IncMembershipEnd
        {
            get { return _incMembershipEnd; }
            set
            {
                if (_incMembershipEnd == value) return;
                _incMembershipEnd = value;
                RaisePropertyChanged(() => IncMembershipEnd);
            }
        }

        public bool IncContractPeriodLeavers
        {
            get { return _incContractPeriodLeavers; }
            set
            {
                if (_incContractPeriodLeavers == value) return;
                _incContractPeriodLeavers = value;
                RaisePropertyChanged(() => IncContractPeriodLeavers);
            }
        }

        public bool IncLastDDMonth
        {
            get { return _incLastDDMonth; }
            set
            {
                if (_incLastDDMonth == value) return;
                _incLastDDMonth = value;
                RaisePropertyChanged(() => IncLastDDMonth);
            }
        }

        public RelayCommand SaveCommand { get; private set; }
        public RelayCommand CancelCommand { get; private set; }

        #endregion Properties

        #region Constructor

        public LeaversOptionsViewModel()
        {
            SaveCommand = new RelayCommand(SaveCommandExecuted);
            CancelCommand = new RelayCommand(CancelCommandExecuted);
        }

        #endregion Constructor

        #region Methods

        public void ResetOptions()
        {
            LoadOptions();
        }

        public void LoadOptions()
        {
            IsBusy = true;

            IncResignDate = Properties.Settings.Default.IncResignDate;
            IncLeavingDate = Properties.Settings.Default.IncLeavingDate;
            IncMembershipNumberLeavers = Properties.Settings.Default.IncMembershipNumberLeavers;
            IncMemberNameLeavers = Properties.Settings.Default.IncMemberNameLeavers;
            IncCategoryNameLeavers = Properties.Settings.Default.IncCategoryNameLeavers;
            IncReason = Properties.Settings.Default.IncReason;
            IncNotes = Properties.Settings.Default.IncNotes;
            IncLinkedMembers = Properties.Settings.Default.IncLinkedMembers;
            IncMembershipStart = Properties.Settings.Default.IncMembershipStart;
            IncMembershipEnd = Properties.Settings.Default.IncMembershipEnd;
            IncContractPeriodLeavers = Properties.Settings.Default.IncContractPeriodLeavers;
            IncLastDDMonth = Properties.Settings.Default.IncLastDDMonth;

            IsBusy = false;
        }

        #endregion Methods

        #region Commands

        private void SaveCommandExecuted()
        {
            IsBusy = true;

            Properties.Settings.Default.IncResignDate = IncResignDate;
            Properties.Settings.Default.IncLeavingDate = IncLeavingDate;
            Properties.Settings.Default.IncMembershipNumberLeavers = IncMembershipNumberLeavers;
            Properties.Settings.Default.IncMemberNameLeavers = IncMemberNameLeavers;
            Properties.Settings.Default.IncCategoryNameLeavers = IncCategoryNameLeavers;
            Properties.Settings.Default.IncReason = IncReason;
            Properties.Settings.Default.IncNotes = IncNotes;
            Properties.Settings.Default.IncLinkedMembers = IncLinkedMembers;
            Properties.Settings.Default.IncMembershipStart = IncMembershipStart;
            Properties.Settings.Default.IncMembershipEnd = IncMembershipEnd;
            Properties.Settings.Default.IncContractPeriodLeavers = IncContractPeriodLeavers;
            Properties.Settings.Default.IncLastDDMonth = IncLastDDMonth;

            Properties.Settings.Default.Save();

            IsBusy = false;

            PopupService.ShowMessage(Properties.Resources.MESSAGE_SETTINGS_SAVED, MessageType.Successful);
        }

        private void CancelCommandExecuted()
        {
            LoadOptions();
        }

        #endregion Commands
    }
}
