using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using EventManagementSystem.Core.Commands;
using EventManagementSystem.Core.Unity;
using EventManagementSystem.Core.ViewModels;
using EventManagementSystem.Data.UnitOfWork.Interfaces;
using EventManagementSystem.Models;
using EventManagementSystem.Services;
using Microsoft.Practices.Unity;

namespace EventManagementSystem.ViewModels.ContactManager.ContactManagerTabs
{
    public class AccountsViewModel : ViewModelBase
    {
        #region Fields

        private readonly IContactsDataUnit _contactsDataUnit;
        private bool _isBusy;
        private ContactModel _contactModel;
        private DateTime _startDate = DateTime.Today.AddDays(-1);
        private DateTime _endDate = DateTime.Today.AddDays(1);

        private ObservableCollection<EventPaymentInvoiceModel> _paymentInvoices;
        private List<EventPaymentInvoiceModel> _allPaymentInvoices;

        #endregion

        #region Properties

        public DateTime StartDate
        {
            get
            {
                return _startDate;
            }
            set
            {
                if (_startDate == value) return;
                _startDate = value;
                RaisePropertyChanged(() => StartDate);
                UpdatePaymentsCollection();
            }
        }

        public DateTime EndDate
        {
            get
            {
                return _endDate;
            }
            set
            {
                if (_endDate == value) return;
                _endDate = value;
                RaisePropertyChanged(() => EndDate);
                UpdatePaymentsCollection();
            }
        }

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

        public ObservableCollection<EventPaymentInvoiceModel> PaymentInvoices
        {
            get { return _paymentInvoices; }
            set
            {
                if (_paymentInvoices == value) return;
                _paymentInvoices = value;
                RaisePropertyChanged(() => PaymentInvoices);
            }
        }

        public RelayCommand<EventPaymentInvoiceModel> OpenInvoiceCommand { get; private set; }

        #endregion

        #region Constructors

        public AccountsViewModel(ContactModel model)
        {
            var dataUnitLocator = ContainerAccessor.Instance.GetContainer().Resolve<IDataUnitLocator>();
            _contactsDataUnit = dataUnitLocator.ResolveDataUnit<IContactsDataUnit>();

            _contactModel = model;

            OpenInvoiceCommand = new RelayCommand<EventPaymentInvoiceModel>(OpenInvoiceCommandExecute);
        }

        #endregion

        #region Methods

        public async void LoadData()
        {
            IsBusy = true;

            _allPaymentInvoices = new List<EventPaymentInvoiceModel>();

            var invoices = await _contactsDataUnit.InvoicesRepository.GetAllAsync(x => !x.Event.IsDeleted
                && (x.Event.Contact.ID == _contactModel.Contact.ID || x.Event.EventContacts.Select(y => y.ContactID).Contains(_contactModel.Contact.ID)));

            if (invoices.Any())
                _allPaymentInvoices.AddRange(invoices.Select(x => new EventPaymentInvoiceModel(x, x.Event)));

            var payments = await _contactsDataUnit.EventPaymentsRepository.GetAllAsync(x => !x.Event.IsDeleted
                && (x.Event.Contact.ID == _contactModel.Contact.ID || x.Event.EventContacts.Select(y => y.ContactID).Contains(_contactModel.Contact.ID)));

            if (payments.Any())
                _allPaymentInvoices.AddRange(payments.Select(x => new EventPaymentInvoiceModel(x, x.Event)));

            UpdatePaymentsCollection();

            IsBusy = false;
        }

        private void UpdatePaymentsCollection()
        {
            PaymentInvoices = new ObservableCollection<EventPaymentInvoiceModel>(_allPaymentInvoices.Where(x => x.Date.Date >= StartDate.Date && x.Date.Date <= EndDate.Date));
        }

        private void OpenInvoiceCommandExecute(EventPaymentInvoiceModel model)
        {
            var appPath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            var fileName = string.Format("{0}_{1}_{2}", model.EventName, "Invoice", DateTime.Now.ToString("g").Replace(":", "").Replace(@"/", ""));
            var reportPath = string.Concat(@"\documents\reports\", fileName, ".pdf");
            var exportPath = string.Concat(appPath, reportPath);

            // create a pdf file
            ReportingService.CreateInvoiceReport(new InvoiceModel(model.InnerInvoice)
            {
                EventModel = model.Event
            }, exportPath);
        }

        #endregion
    }
}
