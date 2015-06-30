using EventManagementSystem.Core.Commands;
using EventManagementSystem.Core.ViewModels;

namespace EventManagementSystem.ViewModels.Admin.CRM
{
    public class EmailNotificationsViewModel : ViewModelBase
    {
        #region Fields

        private bool _sendEmailWhenEnquiryAssigned;
        private bool _sendEmailWhenFollowUpBreached;
        private bool _sendDailyEmailWithTasks;
        private bool _sendEmailToAdminAtTheEndOfEachDay;
        private bool _sendEmailToAdminAtTheEndOfEachWeek;
        private bool _isDataLoadedOnce;
        private bool _isDirty;

        #endregion

        #region Properties

        public bool SendEmailToAdminAtTheEndOfEachWeek
        {
            get { return _sendEmailToAdminAtTheEndOfEachWeek; }
            set
            {
                if (_sendEmailToAdminAtTheEndOfEachWeek == value) return;
                _sendEmailToAdminAtTheEndOfEachWeek = value;
                RaisePropertyChanged(() => SendEmailToAdminAtTheEndOfEachWeek);
            }
        }

        public bool SendEmailToAdminAtTheEndOfEachDay
        {
            get { return _sendEmailToAdminAtTheEndOfEachDay; }
            set
            {
                if (_sendEmailToAdminAtTheEndOfEachDay == value) return;
                _sendEmailToAdminAtTheEndOfEachDay = value;
                RaisePropertyChanged(() => SendEmailToAdminAtTheEndOfEachDay);
            }
        }

        public bool SendDailyEmailWithTasks
        {
            get { return _sendDailyEmailWithTasks; }
            set
            {
                if (_sendDailyEmailWithTasks == value) return;
                _sendDailyEmailWithTasks = value;
                RaisePropertyChanged(() => SendDailyEmailWithTasks);
            }
        }

        public bool SendEmailWhenFollowUpBreached
        {
            get { return _sendEmailWhenFollowUpBreached; }
            set
            {
                if (_sendEmailWhenFollowUpBreached == value) return;
                _sendEmailWhenFollowUpBreached = value;
                RaisePropertyChanged(() => SendEmailWhenFollowUpBreached);
            }
        }

        public bool SendEmailWhenEnquiryAssigned
        {
            get { return _sendEmailWhenEnquiryAssigned; }
            set
            {
                if (_sendEmailWhenEnquiryAssigned == value) return;
                _sendEmailWhenEnquiryAssigned = value;
                RaisePropertyChanged(() => SendEmailWhenEnquiryAssigned);
            }
        }
        public bool IsDirty
        {
            get { return _isDirty; }
            set
            {
                if (_isDirty == value) return;
                _isDirty = value;
                RaisePropertyChanged(() => IsDirty);
                SaveChangesCommand.RaiseCanExecuteChanged();
            }
        }
        public RelayCommand SaveChangesCommand { get; set; }

        #endregion

        #region Constructor

        public EmailNotificationsViewModel()
        {
            SaveChangesCommand = new RelayCommand(SaveChangesCommandExecuted, SaveChangesCommandCanExecute);

            LoadSettings();

            this.PropertyChanged += EmailNotificationsViewModel_PropertyChanged;
        }
        #endregion

        #region Methods

        private void LoadSettings()
        {
            SendEmailToAdminAtTheEndOfEachWeek = Properties.Settings.Default.SendEmailToAdminAtTheEndOfEachWeek;
            SendEmailToAdminAtTheEndOfEachDay = Properties.Settings.Default.SendEmailToAdminAtTheEndOfEachDay;
            SendDailyEmailWithTasks = Properties.Settings.Default.SendDailyEmailWithTasks;
            SendEmailWhenFollowUpBreached = Properties.Settings.Default.SendEmailWhenFollowUpBreached;
            SendEmailWhenEnquiryAssigned = Properties.Settings.Default.SendEmailWhenEnquiryAssigned;
            _isDataLoadedOnce = true;
        }

        private void EmailNotificationsViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName != "IsDirty")
                if (_isDataLoadedOnce)
                    IsDirty = true;
        }

        #endregion

        #region Commands

        private void SaveChangesCommandExecuted()
        {
            IsDirty = false;
            Properties.Settings.Default.SendEmailToAdminAtTheEndOfEachWeek = SendEmailToAdminAtTheEndOfEachWeek;
            Properties.Settings.Default.SendEmailToAdminAtTheEndOfEachDay = SendEmailToAdminAtTheEndOfEachDay;
            Properties.Settings.Default.SendDailyEmailWithTasks = SendDailyEmailWithTasks;
            Properties.Settings.Default.SendEmailWhenFollowUpBreached = SendEmailWhenFollowUpBreached;
            Properties.Settings.Default.SendEmailWhenEnquiryAssigned = SendEmailWhenEnquiryAssigned;
            Properties.Settings.Default.Save();
        }

        private bool SaveChangesCommandCanExecute()
        {
            return IsDirty;
        }

        #endregion
    }
}
