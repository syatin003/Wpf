using System.Collections.ObjectModel;
using EventManagementSystem.Core.ViewModels;
using EventManagementSystem.Services;

namespace EventManagementSystem.ViewModels.Admin.Settings
{
    public class MailFieldsViewModel : ViewModelBase
    {
        #region Fields

        private ObservableCollection<string> _mailFields;
        private string _selectedField;

        #endregion

        #region Properties

        public ObservableCollection<string> MailFields
        {
            get { return _mailFields; }
            set
            {
                if (_mailFields == value) return;
                _mailFields = value;
                RaisePropertyChanged(() => MailFields);
            }
        }

        public string SelectedField
        {
            get { return _selectedField; }
            set
            {
                if (_selectedField == value) return;
                _selectedField = value;
                RaisePropertyChanged(() => SelectedField);
            }
        }

        #endregion

        #region Methods

        public void LoadData()
        {
            MailFields = new ObservableCollection<string>(EmailService.GetMailFieldsNames());
        }

        #endregion
    }
}
