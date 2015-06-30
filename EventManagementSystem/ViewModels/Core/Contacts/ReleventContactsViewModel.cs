using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using EventManagementSystem.Models;
using EventManagementSystem.Core.Commands;
using EventManagementSystem.Core.Unity;
using EventManagementSystem.Data.UnitOfWork.Interfaces;
using EventManagementSystem.Views.Core.Contacts;
using Microsoft.Practices.Unity;
using ViewModelBase = EventManagementSystem.Core.ViewModels.ViewModelBase;

namespace EventManagementSystem.ViewModels.Core.Contacts
{
    public class ReleventContactsViewModel : ViewModelBase
    {
        #region Fields

        private readonly IContactsDataUnit _contactsDataUnit;
        private bool _isBusy;
        private ObservableCollection<ContactModel> _contacts;
        private List<ContactModel> _allContacts;
        private ContactModel _selectedContact;
        private ContactModel _contact;


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

                OKCommand.RaiseCanExecuteChanged();
            }
        }

        public ContactModel Contact
        {
            get { return _contact; }
            set
            {
                if (_contact == value) return;
                _contact = value;
                RaisePropertyChanged(() => Contact);
            }
        }
        public RelayCommand OKCommand { get; private set; }

        #endregion

        #region Constructor

        public ReleventContactsViewModel(ObservableCollection<ContactModel> contacts)
        {
            var dataUnitLocator = ContainerAccessor.Instance.GetContainer().Resolve<IDataUnitLocator>();
            _contactsDataUnit = dataUnitLocator.ResolveDataUnit<IContactsDataUnit>();

            Contacts = contacts;

            OKCommand = new RelayCommand(OKCommandExecuted, OKCommandCanExecute);
        }

        #endregion

        #region Commands

        private bool OKCommandCanExecute()
        {
            return SelectedContact != null;
        }

        private void OKCommandExecuted()
        {
            IsBusy = true;

            Contact = SelectedContact;

            IsBusy = false;
        }

        #endregion
    }
}
