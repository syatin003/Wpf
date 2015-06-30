using EventManagementSystem.Controls;
using EventManagementSystem.Core.Commands;
using EventManagementSystem.Core.ViewModels;
using EventManagementSystem.Services;

namespace EventManagementSystem.ViewModels.Reports.ContentViewModels
{
    public class JoinersLeaversOptionsViewModel : ViewModelBase
    {

        #region Fields

        private bool _isBusy;

        private bool _incOpening;
        private bool _incJoiners;
        private bool _incLeavers;
        private bool _incTransfersIn;
        private bool _incTransfersOut;
        private bool _incClosing;

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

        public bool IncOpening
        {
            get { return _incOpening; }
            set
            {
                if (_incOpening == value) return;
                _incOpening = value;
                RaisePropertyChanged(() => IncOpening);
            }
        }

        public bool IncJoiners
        {
            get { return _incJoiners; }
            set
            {
                if (_incJoiners == value) return;
                _incJoiners = value;
                RaisePropertyChanged(() => IncJoiners);
            }
        }

        public bool IncLeavers
        {
            get { return _incLeavers; }
            set
            {
                if (_incLeavers == value) return;
                _incLeavers = value;
                RaisePropertyChanged(() => IncLeavers);
            }
        }

        public bool IncTransfersIn
        {
            get { return _incTransfersIn; }
            set
            {
                if (_incTransfersIn == value) return;
                _incTransfersIn = value;
                RaisePropertyChanged(() => IncTransfersIn);
            }
        }

        public bool IncTransfersOut
        {
            get { return _incTransfersOut; }
            set
            {
                if (_incTransfersOut == value) return;
                _incTransfersOut = value;
                RaisePropertyChanged(() => IncTransfersOut);
            }
        }

        public bool IncClosing
        {
            get { return _incClosing; }
            set
            {
                if (_incClosing == value) return;
                _incClosing = value;
                RaisePropertyChanged(() => IncClosing);
            }
        }

        public RelayCommand SaveCommand { get; private set; }
        public RelayCommand CancelCommand { get; private set; }

        #endregion Properties

        #region Constructor

        public JoinersLeaversOptionsViewModel()
        {
            SaveCommand = new RelayCommand(SaveCommandExecuted);
            CancelCommand = new RelayCommand(CancelCommandExecuted);
        }

        #endregion

        #region Commands

        private void SaveCommandExecuted()
        {
            IsBusy = true;

            Properties.Settings.Default.IncOpeningOption = IncOpening;
            Properties.Settings.Default.IncJoinersOption = IncJoiners;
            Properties.Settings.Default.IncLeaversOption = IncLeavers;
            Properties.Settings.Default.IncTransfersInOption = IncTransfersIn;
            Properties.Settings.Default.IncTransfersOutOption = IncTransfersOut;
            Properties.Settings.Default.IncClosingOption = IncClosing;

            Properties.Settings.Default.Save();

            IsBusy = false;
            PopupService.ShowMessage(Properties.Resources.MESSAGE_SETTINGS_SAVED, MessageType.Successful);
        }

        private void CancelCommandExecuted()
        {
            LoadOptions();
        }

        #endregion

        #region Methods

        public void ResetOptions()
        {
            LoadOptions();
        }

        public void LoadOptions()
        {
            IsBusy = true;

            IncOpening = Properties.Settings.Default.IncOpeningOption;
            IncJoiners = Properties.Settings.Default.IncJoinersOption;
            IncLeavers = Properties.Settings.Default.IncLeaversOption;
            IncTransfersIn = Properties.Settings.Default.IncTransfersInOption;
            IncTransfersOut = Properties.Settings.Default.IncTransfersOutOption;
            IncClosing = Properties.Settings.Default.IncClosingOption;

            IsBusy = false;
        }

        #endregion
    }
}
