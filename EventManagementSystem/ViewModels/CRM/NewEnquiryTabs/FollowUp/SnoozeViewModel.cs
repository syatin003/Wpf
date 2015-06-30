using System;
using EventManagementSystem.Core.Commands;
using EventManagementSystem.Core.ViewModels;
using EventManagementSystem.Models;

namespace EventManagementSystem.ViewModels.CRM.NewEnquiryTabs.FollowUp
{
    public class SnoozeViewModel : ViewModelBase
    {
        #region Fields

        private string _snoozeTime;
        private FollowUpModel _followUp;

        #endregion

        #region Properties

        public string SnoozeTime
        {
            get { return _snoozeTime; }
            set
            {
                if (_snoozeTime == value) return;
                _snoozeTime = value;
                RaisePropertyChanged(() => SnoozeTime);
                OKCommand.RaiseCanExecuteChanged();
            }
        }

        public FollowUpModel FollowUp
        {
            get { return _followUp; }
            set
            {
                if (_followUp == value) return;
                _followUp = value;
                RaisePropertyChanged(() => FollowUp);
            }
        }

        public RelayCommand OKCommand { get; private set; }
        public RelayCommand CancelCommand { get; private set; }

        #endregion

        #region Constructor

        public SnoozeViewModel(FollowUpModel followUpModel)
        {
            FollowUp = followUpModel;

            OKCommand = new RelayCommand(OKCommandExecute, OKCommandCanExecute);
            CancelCommand = new RelayCommand(CancelCommandExecute);
        }

        #endregion

        #region Methods

        #endregion

        #region Commands

        private void OKCommandExecute()
        {
                      
        }

        private bool OKCommandCanExecute()
        {
            return !String.IsNullOrEmpty(SnoozeTime);
        }

        private void CancelCommandExecute()
        {

        }

        #endregion
    }
}
