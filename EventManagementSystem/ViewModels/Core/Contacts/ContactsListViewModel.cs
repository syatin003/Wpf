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
    public class ContactsListViewModel : ViewModelBase
    {
        #region Fields

        private readonly IContactsDataUnit _contactsDataUnit;
        private readonly IMembershipDataUnit _membershipDataUnit;
        private ObservableCollection<ContactModel> _contacts;
        private List<ContactModel> _allContacts;
        private bool _isBusy;
        private string _searchInputContent;
        private ContactModel _selectedContact;
        private bool _isFromMembership;


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

        public bool IsBusy
        {
            get { return _isBusy; }
            set
            {
                _isBusy = value;
                RaisePropertyChanged(() => IsBusy);
            }
        }

        public string SearchInputContent
        {
            get { return _searchInputContent; }
            set
            {
                if (_searchInputContent == value) return;
                _searchInputContent = value;
                RaisePropertyChanged(() => SearchInputContent);

                FilterGrid();
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
            }
        }

        public bool IsFromMembership
        {
            get { return _isFromMembership; }
            set
            {
                _isFromMembership = value;
                RaisePropertyChanged(() => _isFromMembership);
            }
        }

        public RelayCommand AddContactCommand { get; private set; }
        public RelayCommand<ContactModel> EditContactCommand { get; private set; }

        #endregion

        #region Constructor

        public ContactsListViewModel(bool isFromMembership)
        {
            IsFromMembership = isFromMembership;
            var dataUnitLocator = ContainerAccessor.Instance.GetContainer().Resolve<IDataUnitLocator>();
            _contactsDataUnit = dataUnitLocator.ResolveDataUnit<IContactsDataUnit>();
            _membershipDataUnit = dataUnitLocator.ResolveDataUnit<IMembershipDataUnit>();

            AddContactCommand = new RelayCommand(AddContactCommandExecuted);
            EditContactCommand = new RelayCommand<ContactModel>(EditContactCommandExecuted);
        }

        #endregion

        #region Methods

        public async void LoadData()
        {
            IsBusy = true;
            if (IsFromMembership)
            {
                var contacts = await _membershipDataUnit.ContactsRepository.GetAllAsync(contact => contact.Member == null);
                _allContacts = new List<ContactModel>(contacts.OrderBy(x => x.LastName).Select(x => new ContactModel(x)));
            }
            else
            {
                var contacts = await _contactsDataUnit.ContactsRepository.GetAllAsync();
                _allContacts = new List<ContactModel>(contacts.OrderBy(x => x.LastName).Select(x => new ContactModel(x)));
            }
            Contacts = new ObservableCollection<ContactModel>(_allContacts);

            IsBusy = false;
        }

        private void FilterGrid()
        {
            if (!string.IsNullOrWhiteSpace(SearchInputContent))
            {
                var searchText = SearchInputContent.ToLower();

                var resultSet = _allContacts.Where(x => x.ContactName.ToLower().Contains(searchText)).ToList();

                if (resultSet.Any())
                    Contacts = new ObservableCollection<ContactModel>(resultSet.OrderBy(x => x.LastName));
            }
            else
            {
                Contacts = new ObservableCollection<ContactModel>(_allContacts);
            }
        }

        #endregion

        #region Commands

        private void AddContactCommandExecuted()
        {
            RaisePropertyChanged("DisableParentWindow");

            var window = new AddContactView(null, IsFromMembership);
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

        private void EditContactCommandExecuted(ContactModel model)
        {
            RaisePropertyChanged("DisableParentWindow");

            var window = new AddContactView(model, IsFromMembership);
            window.ShowDialog();

            RaisePropertyChanged("EnableParentWindow");


            if (window.DialogResult != null && window.DialogResult == true)
            {
                if (IsFromMembership)
                    _membershipDataUnit.ContactsRepository.RefreshContact();
                else
                    _contactsDataUnit.ContactsRepository.RefreshContact();
            }
        }

        #endregion
    }
}
