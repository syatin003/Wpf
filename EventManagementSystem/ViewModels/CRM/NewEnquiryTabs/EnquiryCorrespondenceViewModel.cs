using EventManagementSystem.Core.Commands;
using EventManagementSystem.Core.ViewModels;
using EventManagementSystem.Models;
using EventManagementSystem.Views.CRM.NewEnquiryTabs.Correspondence;

namespace EventManagementSystem.ViewModels.CRM.NewEnquiryTabs
{
    public class EnquiryCorrespondenceViewModel : ViewModelBase
    {
        #region Fields

        private bool _isBusy;
        private EnquiryModel _enquiry;

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

        public RelayCommand SendEmailCommand { get; private set; }
        public RelayCommand SendLetterCommand { get; private set; }
        public RelayCommand AttachEmailCommand { get; private set; }

        public RelayCommand<CorrespondenceModel> ShowCorrespondenceCommand { get; private set; }

        #endregion

        #region Constructors

        public EnquiryCorrespondenceViewModel()
        {
            SendEmailCommand = new RelayCommand(SendEmailCommandExecuted);
            SendLetterCommand = new RelayCommand(SendLetterCommandExecuted, () => false);
            AttachEmailCommand = new RelayCommand(AttachEmailCommandExecuted, () => false);
            ShowCorrespondenceCommand = new RelayCommand<CorrespondenceModel>(ShowCorrespondenceCommandExecuted);
        }

        #endregion

        #region Commands

        private void SendEmailCommandExecuted()
        {
            RaisePropertyChanged("DisableParentWindow");

            var window = new SendEnquiryMailView(_enquiry);
            window.ShowDialog();

            RaisePropertyChanged("EnableParentWindow");
        }

        private void ShowCorrespondenceCommandExecuted(CorrespondenceModel obj)
        {
            RaisePropertyChanged("DisableParentWindow");

            var window = new SendEnquiryMailView(_enquiry, obj);
            window.ShowDialog();

            RaisePropertyChanged("EnableParentWindow");
        }

        private void SendLetterCommandExecuted()
        {
        }

        private void AttachEmailCommandExecuted()
        {
        }

        #endregion
    }
}
