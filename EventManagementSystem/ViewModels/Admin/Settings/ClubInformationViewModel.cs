using EventManagementSystem.Core.Commands;
using EventManagementSystem.Core.ViewModels;

namespace EventManagementSystem.ViewModels.Admin.Settings
{
    public class ClubInformationViewModel : ViewModelBase
    {
        #region Fields

        private string _imageUrl;
        private string _address;
        private string _footer;
        private string _bankAccount;
        private bool _isDirty;

        #endregion

        #region Properties

        public string ImageUrl
        {
            get { return _imageUrl; }
            set
            {
                if (_imageUrl == value) return;
                _imageUrl = value;
                RaisePropertyChanged(() => ImageUrl);
            }
        }

        public string Address
        {
            get { return _address; }
            set
            {
                if (_address == value) return;
                _address = value;
                RaisePropertyChanged(() => Address);
            }
        }

        public string Footer
        {
            get { return _footer; }
            set
            {
                if (_footer == value) return;
                _footer = value;
                RaisePropertyChanged(() => Footer);
            }
        }

        public string BankAccount
        {
            get { return _bankAccount; }
            set
            {
                if (_bankAccount == value) return;
                _bankAccount = value;
                RaisePropertyChanged(() => BankAccount);
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
        public RelayCommand SaveChangesCommand { get; private set; }

        #endregion

        #region Constructor

        public ClubInformationViewModel()
        {
            SaveChangesCommand = new RelayCommand(SaveChangesCommandExecuted, SaveChangesCommandCanExecute);

            LoadSettings();

            this.PropertyChanged += ClubInformationViewModel_PropertyChanged;
        }

        #endregion

        #region Methods

        private void LoadSettings()
        {
            ImageUrl = Properties.Settings.Default.ClubInfoImageUrl;
            Address = Properties.Settings.Default.ClubInfoAddress;
            Footer = Properties.Settings.Default.ClubInfoFooter;
            BankAccount = Properties.Settings.Default.ClubInfoBankAccount;
        }

        private void ClubInformationViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName != "IsDirty")
                IsDirty = true;
        }
        #endregion

        #region Commands

        private void SaveChangesCommandExecuted()
        {
            IsDirty = false;
            Properties.Settings.Default.ClubInfoImageUrl = ImageUrl;
            Properties.Settings.Default.ClubInfoAddress = Address;
            Properties.Settings.Default.ClubInfoFooter = Footer;
            Properties.Settings.Default.ClubInfoBankAccount = BankAccount;
            Properties.Settings.Default.Save();
        }

        private bool SaveChangesCommandCanExecute()
        {
            return IsDirty;
        }

        #endregion
    }
}
