using EventManagementSystem.Core.Commands;
using EventManagementSystem.Core.ViewModels;

namespace EventManagementSystem.ViewModels.Admin.CRM
{
    public class NumberOfDaysViewModel : ViewModelBase
    {
        #region Fields

        private int _numberOfDays;
        private bool _isDataLoadedOnce;
        private bool _isDirty;

        #endregion

        #region Properties

        public int NumberOfDays
        {
            get { return _numberOfDays; }
            set
            {
                if (_numberOfDays == value) return;
                _numberOfDays = value;
                RaisePropertyChanged(() => NumberOfDays);
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

        public NumberOfDaysViewModel()
        {
            SaveChangesCommand = new RelayCommand(SaveChangesCommandExecuted, SaveChangesCommandCanExecute);

            LoadSettings();

            this.PropertyChanged += NumberOfDaysViewModel_PropertyChanged;
        }

        private void NumberOfDaysViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "NumberOfDays")
                if (_isDataLoadedOnce)
                    IsDirty = true;
        }

        #endregion

        #region Methods

        private void LoadSettings()
        {
            NumberOfDays = Properties.Settings.Default.NumberOfDaysToForceReminder;

            _isDataLoadedOnce = true;
        }

        #endregion

        #region Commands

        private void SaveChangesCommandExecuted()
        {
            IsDirty = false;
            Properties.Settings.Default.NumberOfDaysToForceReminder = NumberOfDays;
            Properties.Settings.Default.Save();

        }

        private bool SaveChangesCommandCanExecute()
        {
            return IsDirty;
        }

        #endregion
    }
}
