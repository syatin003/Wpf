using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using EventManagementSystem.Models;
using EventManagementSystem.Core.Commands;
using EventManagementSystem.Core.Unity;
using EventManagementSystem.Core.ViewModels;
using EventManagementSystem.Data.Model;
using EventManagementSystem.Data.UnitOfWork.Interfaces;
using Microsoft.Practices.Unity;
using EventManagementSystem.Core.Serialization;
using EventManagementSystem.Views.Core.Contacts;

namespace EventManagementSystem.ViewModels.Core.Contacts
{
    public class AddContactViewModel : ViewModelBase
    {
        #region Fields

        private readonly IContactsDataUnit _contactsDataUnit;
        private readonly IMembershipDataUnit _membershipDataUnit;
        private bool _isBusy;
        private ObservableCollection<ContactTitle> _contactTitles;
        private ContactModel _contactModel;
        private ContactModel _originalContactModel;
        private bool _isEditMode;
        private bool _isIgnored;
        private bool _isExistingContact;
        private bool _isOkButtonClick;
        private bool _isFromMembership;

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

        public ObservableCollection<ContactTitle> ContactTitles
        {
            get { return _contactTitles; }
            set
            {
                if (_contactTitles == value) return;
                _contactTitles = value;
                RaisePropertyChanged(() => ContactTitles);
            }
        }

        public ContactModel ContactModel
        {
            get { return _contactModel; }
            set
            {
                if (_contactModel == value) return;
                _contactModel = value;
                RaisePropertyChanged(() => ContactModel);
            }
        }
        public bool IsIgnored
        {
            get { return _isIgnored; }
            set
            {
                if (_isIgnored == value) return;
                _isIgnored = value;
                RaisePropertyChanged(() => IsIgnored);
            }
        }
        public bool IsExistingContact
        {
            get { return _isExistingContact; }
            set
            {
                if (_isExistingContact == value) return;
                _isExistingContact = value;
                RaisePropertyChanged(() => IsExistingContact);
            }
        }

        public bool IsOkButtonClick
        {
            get { return _isOkButtonClick; }
            set
            {
                if (_isOkButtonClick == value) return;
                _isOkButtonClick = value;
                RaisePropertyChanged(() => IsOkButtonClick);
            }
        }

        public bool IsFromMembership
        {
            get { return _isFromMembership; }
            set
            {
                if (_isFromMembership == value) return;
                _isFromMembership = value;
                RaisePropertyChanged(() => IsFromMembership);
            }
        }
        public RelayCommand OkCommand { get; private set; }
        public RelayCommand CancelContactCommand { get; private set; }

        #endregion

        #region Constructors

        public AddContactViewModel(ContactModel model, bool isFromMembership)
        {
            IsFromMembership = isFromMembership;
            var dataUnitLocator = ContainerAccessor.Instance.GetContainer().Resolve<IDataUnitLocator>();
            if (IsFromMembership)
                _membershipDataUnit = dataUnitLocator.ResolveDataUnit<IMembershipDataUnit>();
            else
                _contactsDataUnit = dataUnitLocator.ResolveDataUnit<IContactsDataUnit>();

            OkCommand = new RelayCommand(OkCommandExecuted, OkCommandCanExecute);
            CancelContactCommand = new RelayCommand(CancelContactCommandExecuted);

            ProcessContact(model);
        }

        #endregion

        #region Methods

        private void ProcessContact(ContactModel model)
        {
            _isEditMode = (model != null);
            ContactModel = model ?? GetContact();
            if (_isEditMode)
                _originalContactModel = ContactModel.Clone();
            ContactModel.PropertyChanged += ContactModelOnPropertyChanged;
        }

        private ContactModel GetContact()
        {
            return new ContactModel(new Contact()
            {
                ID = Guid.NewGuid()
            });
        }

        public async void LoadData()
        {
            IsBusy = true;
            if (IsFromMembership)
            {
                var titles = await _membershipDataUnit.ContactTitlesRepository.GetAllAsync();
                ContactTitles = new ObservableCollection<ContactTitle>(titles.OrderBy(x => x.Title));
            }
            else
            {
                var titles = await _contactsDataUnit.ContactTitlesRepository.GetAllAsync();
                ContactTitles = new ObservableCollection<ContactTitle>(titles.OrderBy(x => x.Title));
            }

            IsBusy = false;
        }

        private void ContactModelOnPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "LastName")
            {
                IsIgnored = false;
            }
            if (args.PropertyName == "Email")
            {
                IsIgnored = false;
            }
            OkCommand.RaiseCanExecuteChanged();
        }

        #endregion

        #region Commands

        private async void OkCommandExecuted()
        {
            IsOkButtonClick = true;
            if (!IsIgnored)
            {
                if (!string.IsNullOrEmpty(ContactModel.Email))
                    await ShowReleventContact();
                else
                {
                    await SaveContact();
                }
            }
            else
            {
                await SaveContact();
            }
        }

        private async Task SaveContact()
        {
            if (IsFromMembership)
            {
                if (!_isEditMode)
                    _membershipDataUnit.ContactsRepository.Add(ContactModel.Contact);
                await _membershipDataUnit.SaveChanges();
            }
            else
            {
                if (!_isEditMode)
                    _contactsDataUnit.ContactsRepository.Add(ContactModel.Contact);
                await _contactsDataUnit.SaveChanges();
            }
            RaisePropertyChanged("CloseDialog");
        }

        public async Task ShowReleventContact()
        {
            List<Contact> allcontacts;
            if (IsFromMembership)
                allcontacts = await _membershipDataUnit.ContactsRepository.GetAllAsyncWithoutRefresh(contact => contact.LastName == ContactModel.LastName && contact.Email == ContactModel.Email);
            else
                allcontacts = await _contactsDataUnit.ContactsRepository.GetAllAsyncWithoutRefresh(contact => contact.LastName == ContactModel.LastName && contact.Email == ContactModel.Email);

            if (allcontacts.Count > 0)
            {
                var contacts = new ObservableCollection<ContactModel>(allcontacts.OrderBy(contact => contact.LastName).Select(contact => new ContactModel(contact)));
                RaisePropertyChanged("DisableParentWindow");

                var view = new ReleventContactsView(contacts);
                view.ShowDialog();

                RaisePropertyChanged("EnableParentWindow");

                if (view.DialogResult != null && view.DialogResult.Value)
                {
                    ContactModel = view.ViewModel.Contact;
                    IsExistingContact = true;
                    RaisePropertyChanged("CloseDialog");
                }
                if (view.DialogResult == null || !view.DialogResult.Value)
                    IsIgnored = true;
            }
            else
            {
                if (IsOkButtonClick)
                {
                    await SaveContact();
                }
            }
        }

        private bool OkCommandCanExecute()
        {
            return !_contactModel.HasErrors;
        }

        public void CancelContactCommandExecuted()
        {
            if (_isEditMode)
            {
                //ContactModel = _originalContactModel.Clone();
            }
            ////_contactsDataUnit.RevertChanges();
            //_contactsDataUnit.SaveChanges();
        }
        #endregion
    }
}