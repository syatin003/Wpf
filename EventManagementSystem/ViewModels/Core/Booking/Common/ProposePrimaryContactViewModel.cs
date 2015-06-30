using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using EventManagementSystem.Core.Commands;
using EventManagementSystem.Core.ViewModels;
using EventManagementSystem.Data.Model;
using EventManagementSystem.Models;

namespace EventManagementSystem.ViewModels.Core.Booking.Common
{
    public class ProposePrimaryContactViewModel : ViewModelBase
    {
        #region Fields

        private ObservableCollection<ContactModel> _contacts;
        private ContactModel _selectedContact;

        #endregion

        #region Properties

        public ObservableCollection<ContactModel> Contacts
        {
            get { return _contacts; }
            set
            {
                if (_contacts == value) return;
                _contacts = value;
                RaisePropertyChanged(() => Contacts);
            }
        }

        public ContactModel SelectedContact
        {
            get { return _selectedContact; }
            set
            {
                if (_selectedContact == value) return;
                _selectedContact = value;
                RaisePropertyChanged(() => SelectedContact);

                OkCommand.RaiseCanExecuteChanged();
            }
        }

        public RelayCommand OkCommand { get; private set; }

        #endregion

        #region Constructor

        public ProposePrimaryContactViewModel(IEnumerable<Contact> contacts)
        {
            Contacts = new ObservableCollection<ContactModel>(contacts.Select(x => new ContactModel(x)));

            OkCommand = new RelayCommand(OkCommandExecuted, OkCommandCanExecute);
        }

        #endregion

        #region Commands

        private bool OkCommandCanExecute()
        {
            return SelectedContact != null;
        }

        private void OkCommandExecuted()
        {

        }

        #endregion
    }
}
