using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using EventManagementSystem.Core.Commands;
using EventManagementSystem.Core.Unity;
using EventManagementSystem.Core.ViewModels;
using EventManagementSystem.Data.Model;
using EventManagementSystem.Data.UnitOfWork.Interfaces;
using Microsoft.Practices.Unity;

namespace EventManagementSystem.ViewModels.Admin.CRM
{
    public class DefaultWebEnquiryStatusViewModel : ViewModelBase
    {
        #region Fields

        private readonly IAdminDataUnit _adminDataUnit;
        private ObservableCollection<EnquiryStatus> _enquiryStatuses;
        private ObservableCollection<User> _users;
        private bool _isBusy;
        private List<DefaultSettingsForEnquiry> _defaultSettings;

        private DefaultSettingsForEnquiry _defaultSetting;

        private bool _isDirty;
        private EnquiryStatus _enquiryStatus;
        private User _user;
        private bool _isDataLoadedOnce;

        #endregion

        #region Properties

        public bool IsBusy
        {
            get { return _isBusy; }
            set
            {
                _isBusy = value;
                RaisePropertyChanged(() => IsBusy);
            }
        }

        public ObservableCollection<EnquiryStatus> EnquiryStatuses
        {
            get { return _enquiryStatuses; }
            set
            {
                if (_enquiryStatuses == value) return;
                _enquiryStatuses = value;
                RaisePropertyChanged(() => EnquiryStatuses);

            }
        }

        public ObservableCollection<User> Users
        {
            get { return _users; }
            set
            {
                if (_users == value) return;
                _users = value;
                RaisePropertyChanged(() => Users);

            }
        }

        public DefaultSettingsForEnquiry DefaultSetting
        {
            get { return _defaultSetting; }
            set
            {
                if (_defaultSetting == value) return;
                _defaultSetting = value;
                RaisePropertyChanged(() => DefaultSetting);
            }
        }

        public User User
        {
            get { return _user; }
            set
            {
                if (_user == value) return;
                _user = value;
                RaisePropertyChanged(() => User);
                if (_isDataLoadedOnce)
                    IsDirty = true;
            }
        }

        public EnquiryStatus EnquiryStatus
        {
            get { return _enquiryStatus; }
            set
            {
                if (_enquiryStatus == value) return;
                _enquiryStatus = value;
                RaisePropertyChanged(() => EnquiryStatus);
                if (_isDataLoadedOnce)
                    IsDirty = true;
            }
        }
        public List<DefaultSettingsForEnquiry> DefaultSettings
        {
            get
            {
                return _defaultSettings;
            }
            set
            {
                if (_defaultSettings == value) return;
                _defaultSettings = value;
                RaisePropertyChanged(() => DefaultSettings);
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

        public DefaultWebEnquiryStatusViewModel()
        {
            var dataUnitLocator = ContainerAccessor.Instance.GetContainer().Resolve<IDataUnitLocator>();
            _adminDataUnit = dataUnitLocator.ResolveDataUnit<IAdminDataUnit>();

            SaveChangesCommand = new RelayCommand(SaveChangesCommandExecuted, SaveChangesCommandCanExecute);
        }

        #endregion

        #region Methods

        public async void LoadData()
        {
            IsBusy = true;

            var enquiryStatuses = await _adminDataUnit.EnquiryStatusesRepository.GetAllAsync();
            EnquiryStatuses = new ObservableCollection<EnquiryStatus>(enquiryStatuses.OrderBy(x => x.Status));

            var users = await _adminDataUnit.UsersRepository.GetUsersAsync();
            Users = new ObservableCollection<User>(users.OrderBy(x => x.FirstName));

            // Load default settigns
            _adminDataUnit.DefaultSettingsForEnquiriesRepository.Refresh();
            var settings = await _adminDataUnit.DefaultSettingsForEnquiriesRepository.GetAllAsync();

            DefaultSettings = settings.ToList();
            DefaultSetting = DefaultSettings.FirstOrDefault() ?? new DefaultSettingsForEnquiry() { ID = Guid.NewGuid() };

            User = DefaultSetting.User;
            EnquiryStatus = DefaultSetting.EnquiryStatus;
            DefaultSetting.PropertyChanged += OnPropertyChanged;

            _isDataLoadedOnce = true;

            IsBusy = false;
        }


        private void OnPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "FromAddress")
                IsDirty = true;
        }

        #endregion

        #region Commands

        private async void SaveChangesCommandExecuted()
        {
            DefaultSetting.User = User;
            DefaultSetting.EnquiryStatus = EnquiryStatus;

            if (!DefaultSettings.Any())
                _adminDataUnit.DefaultSettingsForEnquiriesRepository.Add(DefaultSetting);

            IsDirty = false;
            await _adminDataUnit.SaveChanges();
        }

        private bool SaveChangesCommandCanExecute()
        {
            return IsDirty;
        }

        #endregion
    }
}
