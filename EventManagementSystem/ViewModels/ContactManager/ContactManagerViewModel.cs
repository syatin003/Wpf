using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Controls;
using EventManagementSystem.Core.Commands;
using EventManagementSystem.Core.Unity;
using EventManagementSystem.Core.ViewModels;
using EventManagementSystem.Data.UnitOfWork.Interfaces;
using EventManagementSystem.Models;
using EventManagementSystem.Properties;
using EventManagementSystem.Services;
using EventManagementSystem.Views.ContactManager.ContactManagerTabs;
using EventManagementSystem.Views.Core.Contacts;
using Microsoft.Practices.Unity;

namespace EventManagementSystem.ViewModels.ContactManager
{
    public class ContactManagerViewModel : ViewModelBase
    {
        #region Fields

        private readonly IContactsDataUnit _contactsDataUnit;
        private ObservableCollection<ContactModel> _contacts;
        private List<ContactModel> _allContacts;
        private bool _isBusy;
        private ContactModel _selectedContact;

        private ContentControl _contactDetailsContent;
        private ContentControl _correspondenceContent;
        private ContentControl _activityContent;
        private ContentControl _accountsContent;

        #endregion

        #region Properties

        public ContentControl ContactDetailsContent
        {
            get { return _contactDetailsContent; }
            set
            {
                if (_contactDetailsContent == value) return;
                _contactDetailsContent = value;
                RaisePropertyChanged(() => ContactDetailsContent);
            }
        }

        public ContentControl CorrespondenceContent
        {
            get { return _correspondenceContent; }
            set
            {
                if (_correspondenceContent == value) return;
                _correspondenceContent = value;
                RaisePropertyChanged(() => CorrespondenceContent);
            }
        }

        public ContentControl ActivityContent
        {
            get { return _activityContent; }
            set
            {
                if (_activityContent == value) return;
                _activityContent = value;
                RaisePropertyChanged(() => ActivityContent);
            }
        }

        public ContentControl AccountsContent
        {
            get { return _accountsContent; }
            set
            {
                if (_accountsContent == value) return;
                _accountsContent = value;
                RaisePropertyChanged(() => AccountsContent);
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

        public bool IsBusy
        {
            get { return _isBusy; }
            set
            {
                _isBusy = value;
                RaisePropertyChanged(() => IsBusy);
            }
        }

        public ContactModel SelectedContact
        {
            get
            {
                return _selectedContact;
            }
            set
            {
                if (_selectedContact == value) return;
                _selectedContact = value;
                RaisePropertyChanged(() => SelectedContact);

                ShowSelectedContactTabs(SelectedContact);
            }
        }

        private void ShowSelectedContactTabs(ContactModel model)
        {
            if (model != null)
            {
                if (CanViewContactDetails)
                    ContactDetailsContent = new ContactDetailsView(model);
                if (CanViewCorrespondence)
                    CorrespondenceContent = new CorrespondenceView(model);
                if (CanViewActivity)
                    ActivityContent = new ActivityView(model);
                if (CanViewAccounts)
                    AccountsContent = new AccountsView(model);
            }
        }

        public bool CanViewContactDetails { get; private set; }

        public bool CanViewCorrespondence { get; private set; }

        public bool CanViewActivity { get; private set; }

        public bool CanViewAccounts { get; private set; }

        public RelayCommand AddContactCommand { get; private set; }

        #endregion

        #region Constructor

        public ContactManagerViewModel()
        {
            var dataUnitLocator = ContainerAccessor.Instance.GetContainer().Resolve<IDataUnitLocator>();
            _contactsDataUnit = dataUnitLocator.ResolveDataUnit<IContactsDataUnit>();

            AddContactCommand = new RelayCommand(AddContactCommandExecuted, AddContactCommandCanExecute);

            CanViewAccounts = AccessService.Current.UserHasPermissions(Resources.PERMISSION_ACCOUNTS_TAB_ALLOWED);
            CanViewActivity = AccessService.Current.UserHasPermissions(Resources.PERMISSION_ACTIVITY_TAB_ALLOWED);
            CanViewContactDetails = AccessService.Current.UserHasPermissions(Resources.PERMISSION_CONTACT_DETAILS_TAB_ALLOWED);
            CanViewCorrespondence = AccessService.Current.UserHasPermissions(Resources.PERMISSION_CORRESPONDENCE_TAB_ALLOWED);

        }

        #endregion

        #region Methods

        public async void LoadData()
        {
            IsBusy = true;

            var contacts = await _contactsDataUnit.ContactsRepository.GetAllAsync();
            _allContacts = new List<ContactModel>(contacts.OrderBy(x => x.LastName).Select(x => new ContactModel(x)));
            Contacts = new ObservableCollection<ContactModel>(_allContacts);

            IsBusy = false;
        }
        public void ChangeSelectedContact()
        {
            ContactDetailsContent = null;
            CorrespondenceContent = null;
            ActivityContent = null;
            AccountsContent = null;
        }
        #endregion

        #region Commands

        private void AddContactCommandExecuted()
        {
            RaisePropertyChanged("DisableParentWindow");

            var window = new AddContactView();
            window.ShowDialog();

            RaisePropertyChanged("EnableParentWindow");

            if (window.DialogResult != null && window.DialogResult == true)
            {
                if (!window.ViewModel.IsExistingContact)
                    _allContacts.Add(window.ViewModel.ContactModel);
                Contacts = new ObservableCollection<ContactModel>(_allContacts.OrderBy(x => x.LastName));
                SelectedContact = window.ViewModel.IsExistingContact ? Contacts.FirstOrDefault(contact => contact.Contact == window.ViewModel.ContactModel.Contact) : window.ViewModel.ContactModel;
                RaisePropertyChanged("ScrollToSelectedItem");
            }
        }

        private bool AddContactCommandCanExecute()
        {
            return AccessService.Current.UserHasPermissions(Resources.PERMISSION_ADD_CONTACT_ALLOWED);
        }

        #endregion
    }
}
