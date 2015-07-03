using System;
using EventManagementSystem.Core.Commands;
using EventManagementSystem.Core.ViewModels;
using EventManagementSystem.Services;
using System.ComponentModel;
using System.Linq;
using EventManagementSystem.Data.UnitOfWork.Interfaces;
using EventManagementSystem.Core.Unity;
using Microsoft.Practices.Unity;
using EventManagementSystem.Data.Model;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace EventManagementSystem.ViewModels.Admin.Settings
{
    public class SystemSettingsViewModel : ViewModelBase, IDataErrorInfo
    {
        #region Fields

        private readonly IAdminDataUnit _adminDataUnit;
        private string _documentsPath;
        private DateTime _weeklyStartTime;
        private DateTime _dailyStartTime;
        private int _logoutTime;
        private SystemSetting _startingPLUTillProductsSetting;
        private String _startingPLUTillProducts;
        private bool _isStartingPLUIDExist;
        private SystemSetting _clubCodeSetting;
        private String _clubCode;
        private bool _isClubCodeExist;
        private SystemSetting _currentMemberNumberSetting;
        private String _currentMemberNumber;
        private bool _isCurrentMemberNumberExist;
        private bool _isDataLoadedOnce;
        private bool _isDirty;
        private List<SystemSetting> SystemSettings;

        #endregion

        #region Properties

        public string DocumentsPath
        {
            get { return _documentsPath; }
            set
            {
                if (_documentsPath == value) return;
                _documentsPath = value;
                RaisePropertyChanged(() => DocumentsPath);
            }
        }

        public DateTime WeeklyStartTime
        {
            get { return _weeklyStartTime; }
            set
            {
                if (_weeklyStartTime == value) return;
                _weeklyStartTime = value;
                RaisePropertyChanged(() => WeeklyStartTime);
            }
        }

        public DateTime DailyStartTime
        {
            get { return _dailyStartTime; }
            set
            {
                if (_dailyStartTime == value) return;
                _dailyStartTime = value;
                RaisePropertyChanged(() => DailyStartTime);
            }
        }

        public int LogoutTime
        {
            get { return _logoutTime; }
            set
            {
                if (_logoutTime == value) return;
                _logoutTime = value;
                RaisePropertyChanged(() => LogoutTime);
            }
        }

        public SystemSetting StartingPLUTillProductsSetting
        {
            get { return _startingPLUTillProductsSetting; }
            set
            {
                if (_startingPLUTillProductsSetting == value) return;
                _startingPLUTillProductsSetting = value;
                RaisePropertyChanged(() => StartingPLUTillProductsSetting);
            }
        }

        public String StartingPLUTillProducts
        {
            get { return _startingPLUTillProducts; }
            set
            {
                if (_startingPLUTillProducts == value) return;
                _startingPLUTillProducts = value;
                StartingPLUTillProductsSetting.Value = _startingPLUTillProducts;
                RaisePropertyChanged(() => StartingPLUTillProducts);
            }
        }

        public bool IsStartingPLUIDExist
        {
            get { return _isStartingPLUIDExist; }
            set
            {
                if (_isStartingPLUIDExist == value) return;
                _isStartingPLUIDExist = value;
                RaisePropertyChanged(() => IsStartingPLUIDExist);
            }
        }

        public SystemSetting ClubCodeSetting
        {
            get { return _clubCodeSetting; }
            set
            {
                if (_clubCodeSetting == value) return;
                _clubCodeSetting = value;
                RaisePropertyChanged(() => ClubCodeSetting);
            }
        }

        public String ClubCode
        {
            get { return _clubCode; }
            set
            {
                if (_clubCode == value) return;
                _clubCode = value;
                ClubCodeSetting.Value = _clubCode;
                RaisePropertyChanged(() => ClubCode);
            }
        }

        public bool IsClubCodeExist
        {
            get { return _isClubCodeExist; }
            set
            {
                if (_isClubCodeExist == value) return;
                _isClubCodeExist = value;
                RaisePropertyChanged(() => IsClubCodeExist);
            }
        }

        public SystemSetting CurrentMemberNumberSetting
        {
            get { return _currentMemberNumberSetting; }
            set
            {
                if (_currentMemberNumberSetting == value) return;
                _currentMemberNumberSetting = value;
                RaisePropertyChanged(() => CurrentMemberNumberSetting);
            }
        }

        public String CurrentMemberNumber
        {
            get { return _currentMemberNumber; }
            set
            {
                if (_currentMemberNumber == value) return;
                _currentMemberNumber = value;
                CurrentMemberNumberSetting.Value = _currentMemberNumber;
                RaisePropertyChanged(() => CurrentMemberNumber);
            }
        }

        public bool IsCurrentMemberNumberExist
        {
            get { return _isCurrentMemberNumberExist; }
            set
            {
                if (_isCurrentMemberNumberExist == value) return;
                _isCurrentMemberNumberExist = value;
                RaisePropertyChanged(() => IsCurrentMemberNumberExist);
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

        public SystemSettingsViewModel()
        {
            var dataUnitLocator = ContainerAccessor.Instance.GetContainer().Resolve<IDataUnitLocator>();
            _adminDataUnit = dataUnitLocator.ResolveDataUnit<IAdminDataUnit>();

            SaveChangesCommand = new RelayCommand(SaveChangesCommandExecuted, SaveChangesCommandCanExecute);
            LoadSettings();

            this.PropertyChanged += OnSystemSettingPropertyChanged;
        }

        #endregion

        #region Methods

        private void LoadSettings()
        {
            DocumentsPath = (string)ApplicationSettings.Read("DocumentsPath");

            var time = (int?)ApplicationSettings.Read("LogoutTime");
            LogoutTime = (time.HasValue && time.Value > 0) ? time.Value : 30; // 30 minutes - default logout time

            DailyStartTime = Properties.Settings.Default.DailyStartTime;
            WeeklyStartTime = Properties.Settings.Default.WeeklyStartTime;
        }

        public async void LoadData()
        {
            _adminDataUnit.SystemSettingsRepository.Refresh();
            var systemSettings = await _adminDataUnit.SystemSettingsRepository.GetAllAsync(setting =>
                setting.Name == "StartingPLUID" || setting.Name == "ClubCode" || setting.Name == "CurrentMemberNumber");
            SystemSettings = new List<SystemSetting>(systemSettings);

            var startingPLUIDSettings = SystemSettings.FirstOrDefault(x => x.Name == "StartingPLUID");

            if (startingPLUIDSettings == null)
                StartingPLUTillProductsSetting = GetNewSystemSetting("StartingPLUID", "0");
            else
            {
                StartingPLUTillProductsSetting = startingPLUIDSettings;
                IsStartingPLUIDExist = true;
            }

            var clubCodeSettings = SystemSettings.FirstOrDefault(x => x.Name == "ClubCode");
            if (clubCodeSettings == null)
                ClubCodeSetting = GetNewSystemSetting("ClubCode", "W");
            else
            {
                ClubCodeSetting = clubCodeSettings;
                IsClubCodeExist = true;
            }

            var currentMemberNumberSettings = SystemSettings.FirstOrDefault(x => x.Name == "CurrentMemberNumber");
            if (currentMemberNumberSettings == null)
                CurrentMemberNumberSetting = GetNewSystemSetting("CurrentMemberNumber", "1");
            else
            {
                CurrentMemberNumberSetting = currentMemberNumberSettings;
                IsCurrentMemberNumberExist = true;
            }

            StartingPLUTillProducts = StartingPLUTillProductsSetting.Value;
            ClubCode = ClubCodeSetting.Value;
            CurrentMemberNumber = CurrentMemberNumberSetting.Value;

            _isDataLoadedOnce = true;
        }

        private SystemSetting GetNewSystemSetting(String name, string value)
        {
            return new SystemSetting()
            {
                ID = Guid.NewGuid(),
                Name = name,
                IsGlobal = true,
                Value = value
            };
        }

        private void OnSystemSettingPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName != "IsDirty")
                if (!this.HasErrors && (_isDataLoadedOnce || !IsStartingPLUIDExist || !IsClubCodeExist || !IsCurrentMemberNumberExist))
                    IsDirty = true;
                else
                    IsDirty = false;

        }
        #endregion

        #region Commands

        private async void SaveChangesCommandExecuted()
        {
            IsDirty = false;
            ApplicationSettings.Write("DocumentsPath", DocumentsPath);
            ApplicationSettings.Write("LogoutTime", LogoutTime);

            Properties.Settings.Default.DailyStartTime = DailyStartTime;
            Properties.Settings.Default.WeeklyStartTime = WeeklyStartTime;

            Properties.Settings.Default.Save();
            if (!IsStartingPLUIDExist)
                _adminDataUnit.SystemSettingsRepository.Add(StartingPLUTillProductsSetting);
            if (!IsClubCodeExist)
                _adminDataUnit.SystemSettingsRepository.Add(ClubCodeSetting);
            if (!IsCurrentMemberNumberExist)
                _adminDataUnit.SystemSettingsRepository.Add(CurrentMemberNumberSetting);
            await _adminDataUnit.SaveChanges();

        }
        private bool SaveChangesCommandCanExecute()
        {
            return !this.HasErrors && IsDirty;
        }

        #endregion

        public bool HasErrors
        {
            get { return typeof(SystemSettingsViewModel).GetProperties().Any(prop => !string.IsNullOrEmpty(this[prop.Name])); }
        }

        public string this[string columnName]
        {
            get
            {
                Error = string.Empty;

                if (columnName == "StartingPLUTillProducts")
                {
                    Int32 startingID;
                    if (StartingPLUTillProducts != null)
                    {
                        if (Int32.TryParse(StartingPLUTillProducts, out startingID))
                        {
                            if (startingID < 1 || startingID > 20000)
                                Error = "Starting PLU should be between 1 and 20000.";
                        }
                        else
                        {
                            Error = "Starting PLU ID must be a number";
                        }
                    }
                }
                if (columnName == "ClubCode")
                {
                    if (ClubCode != null)
                    {
                        if (ClubCode == string.Empty)
                            Error = "Club code can't be empty";
                        else if (ClubCode.Length > 3)
                            Error = "Club code can be upto 3 letters";
                        else if (!Regex.IsMatch(ClubCode, @"^[a-zA-Z]+$"))
                            Error = "Club code should contain only alphabets";
                    }
                }
                if (columnName == "CurrentMemberNumber")
                {
                    if (CurrentMemberNumber != null)
                    {
                        if (CurrentMemberNumber == string.Empty)
                            Error = "CurrentMemberNumber can't be empty";
                        else
                        {
                            Int32 memberNumber;
                            if (Int32.TryParse(CurrentMemberNumber, out memberNumber))
                            {
                                if (CurrentMemberNumber.Length > 5)
                                    Error = "Current member number can be upto 5 numbers";
                                else if (CurrentMemberNumber.Length < 4)
                                    Error = "Current member number must be 4 digit long";
                            }
                            else
                            {
                                Error = "Current member number must be a number";
                            }
                        }
                    }
                }
                return Error;
            }
        }

        public string Error { get; private set; }

    }
}
