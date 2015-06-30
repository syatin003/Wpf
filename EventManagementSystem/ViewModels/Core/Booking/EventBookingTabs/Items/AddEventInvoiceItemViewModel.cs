using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using EventManagementSystem.Models;
using EventManagementSystem.Core.Commands;
using EventManagementSystem.Core.Unity;
using EventManagementSystem.Core.ViewModels;
using EventManagementSystem.Data.Model;
using EventManagementSystem.Data.UnitOfWork.Interfaces;
using EventManagementSystem.Views.Admin.EPOS.Products;
using Microsoft.Practices.Unity;
using EventManagementSystem.Core.Serialization;
using Microsoft.Practices.ObjectBuilder2;

namespace EventManagementSystem.ViewModels.Core.Booking.EventBookingTabs.Items
{
    public class AddEventInvoiceItemViewModel : ViewModelBase
    {
        #region Fields

        private readonly IEventDataUnit _eventDataUnit;
        private readonly EventModel _event;
        private bool _isBusy;
        private ObservableCollection<Product> _products;
        private EventInvoiceModel _eventInvoice;
        private EventInvoiceModel _eventInvoiceOriginal;
        private bool _isEditMode;

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

        public ObservableCollection<Product> Products
        {
            get { return _products; }
            set
            {
                if (_products == value) return;
                _products = value;
                RaisePropertyChanged(() => Products);
            }
        }

        public EventInvoiceModel EventInvoice
        {
            get { return _eventInvoice; }
            set
            {
                if (_eventInvoice == value) return;
                _eventInvoice = value;
                RaisePropertyChanged(() => EventInvoice);
            }
        }
        public EventInvoiceModel EventInvoiceOriginal
        {
            get { return _eventInvoiceOriginal; }
            set
            {
                if (_eventInvoiceOriginal == value) return;
                _eventInvoiceOriginal = value;
                RaisePropertyChanged(() => _eventInvoiceOriginal);
            }
        }

        public RelayCommand SubmitCommand { get; private set; }
        public RelayCommand AddItemCommand { get; private set; }
        public RelayCommand AddProductCommand { get; private set; }
        public RelayCommand CancelCommand { get; private set; }
        public RelayCommand<EventBookedProductModel> DeleteBookedProductCommand { get; private set; }

        #endregion

        #region Constructors

        public AddEventInvoiceItemViewModel(EventModel eventModel, EventInvoiceModel invoiceModel)
        {
            _event = eventModel;

            var dataUnitLocator = ContainerAccessor.Instance.GetContainer().Resolve<IDataUnitLocator>();
            _eventDataUnit = dataUnitLocator.ResolveDataUnit<IEventDataUnit>();

            SubmitCommand = new RelayCommand(SubmitCommandExecuted, SubmitCommandCanExecute);
            AddItemCommand = new RelayCommand(AddItemCommandExecuted);
            CancelCommand = new RelayCommand(CancelCommandExecuted);
            AddProductCommand = new RelayCommand(AddProductCommandExecuted);
            DeleteBookedProductCommand = new RelayCommand<EventBookedProductModel>(DeleteBookedProductCommandExecuted);

            ProcessEventInvoice(invoiceModel);
        }

        private void ProcessEventInvoice(EventInvoiceModel invoiceModel)
        {
            _isEditMode = (invoiceModel != null);

            EventInvoice = (_isEditMode) ? invoiceModel : GetEventInvoice();
            if (_isEditMode)
                EventInvoiceOriginal = EventInvoice.Clone();
            EventInvoice.PropertyChanged += OnEventBookedProductModelPropertyChanged;
        }

        #endregion

        #region Methods

        private EventInvoiceModel GetEventInvoice()
        {
            var invoiceModel = new EventInvoiceModel(new EventInvoice()
            {
                ID = Guid.NewGuid(),
                EventID = _event.Event.ID,
                ShowInInvoice = true,
                IncludeInCorrespondence = true,
                IncludeInForwardBook = true
            });

            return invoiceModel;
        }

        private void AddInvoiceProduct(EventInvoiceModel model)
        {
            var charge = new EventCharge()
            {
                ID = Guid.NewGuid(),
                EventID = _event.Event.ID,
                ShowInInvoice = model.EventInvoice.ShowInInvoice
            };

            var item = new EventBookedProductModel(new EventBookedProduct()
            {
                ID = Guid.NewGuid(),
                EventBookingItemID = model.EventInvoice.ID,
                EventID = _event.Event.ID,
                EventCharge = charge
            });

            item.Quantity = _event.Event.Places;
            item.PropertyChanged += OnEventBookedProductModelPropertyChanged;

            model.EventBookedProducts.Add(item);
        }

        private void OnEventBookedProductModelPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            SubmitCommand.RaiseCanExecuteChanged();
        }

        public async void LoadData()
        {
            IsBusy = true;

            var products = await _eventDataUnit.ProductsRepository.GetAllAsync();

            if (_event.EventType == null)
            {
                Products = new ObservableCollection<Product>(products
                    .Where(x => x.ProductOption.OptionName == "Invoice")
                    .OrderBy(x => x.Name)
                    .ToList());
            }
            else
            {
                Products = new ObservableCollection<Product>(products
                    .Where(x => x.ProductEventTypes.Any(e => e.EventType.ID == _event.EventType.ID))
                    .Where(x => x.ProductOption.OptionName == "Invoice")
                    .OrderBy(x => x.Name)
                    .ToList());
            }

            IsBusy = false;
        }

        private void MapChangedDataAfterRefresh(Data.Model.EventInvoice eventInvoiceOriginal, Data.Model.EventInvoice eventInvoiceChanged)
        {
            eventInvoiceOriginal.ShowInInvoice = eventInvoiceChanged.ShowInInvoice;
            eventInvoiceOriginal.IncludeInForwardBook = eventInvoiceChanged.IncludeInForwardBook;
            eventInvoiceOriginal.IncludeInCorrespondence = eventInvoiceChanged.IncludeInCorrespondence;
            eventInvoiceOriginal.Notes = eventInvoiceChanged.Notes;
        }

        #endregion

        #region Commands

        private void SubmitCommandExecuted()
        {
            if (IsBusy) return;
            IsBusy = true;
            SubmitCommand.RaiseCanExecuteChanged();
            if (!_isEditMode)
            {
                _event.EventInvoices.Add(EventInvoice);
                _eventDataUnit.EventInvoicesRepository.Add(EventInvoice.EventInvoice);

                foreach (var product in EventInvoice.EventBookedProducts)
                {
                    product.EventCharge.EventCharge.ShowInInvoice = EventInvoice.EventInvoice.ShowInInvoice;

                    _event.EventCharges.Add(product.EventCharge);
                    _eventDataUnit.EventChargesRepository.Add(product.EventCharge.EventCharge);

                    _event.EventBookedProducts.Add(product);
                    _eventDataUnit.EventBookedProductsRepository.Add(product.EventBookedProduct);
                }
            }
            else
            {
                EventInvoice.EventBookedProducts.ForEach(eventBookedProduct =>
                        {
                            eventBookedProduct.EventCharge.EventCharge.ShowInInvoice = EventInvoice.EventInvoice.ShowInInvoice;
                        });
                var newProdcuts = _eventInvoice.EventBookedProducts.Except(_event.EventBookedProducts).ToList();
                if (newProdcuts.Any())
                {
                    foreach (var prodcut in newProdcuts)
                    {
                        _event.EventBookedProducts.Add(prodcut);
                        _eventDataUnit.EventBookedProductsRepository.Add(prodcut.EventBookedProduct);

                        _event.EventCharges.Add(prodcut.EventCharge);
                        _eventDataUnit.EventChargesRepository.Add(prodcut.EventCharge.EventCharge);
                    }
                }
            }

            RaisePropertyChanged("CloseDialog");
        }

        private bool SubmitCommandCanExecute()
        {
            if (IsBusy)
                return false;
            else
                return EventInvoice != null && !EventInvoice.HasErrors;

        }

        private void AddItemCommandExecuted()
        {
            AddInvoiceProduct(EventInvoice);
        }

        public void CancelCommandExecuted()
        {
            _eventDataUnit.RevertChanges();

            if (_isEditMode)
            {
                MapChangedDataAfterRefresh(EventInvoice.EventInvoice, EventInvoiceOriginal.EventInvoice);
                EventInvoice.Refresh();
            }
        }

        private void AddProductCommandExecuted()
        {
            RaisePropertyChanged("DisableParentWindow");

            var view = new AddProductView();
            view.ShowDialog();

            RaisePropertyChanged("EnableParentWindow");

            if (view.DialogResult != null && view.DialogResult == true && !view.ViewModel.Product.HasErrors)
            {
                Products.Add(view.ViewModel.Product.Product);
            }
        }

        private void DeleteBookedProductCommandExecuted(EventBookedProductModel obj)
        {
            obj.PropertyChanged -= OnEventBookedProductModelPropertyChanged;

            _eventInvoice.EventBookedProducts.Remove(obj);

            _event.EventCharges.Remove(obj.EventCharge);
            _eventDataUnit.EventChargesRepository.Delete(obj.EventCharge.EventCharge);

            _event.EventBookedProducts.Remove(obj);
            _eventDataUnit.EventBookedProductsRepository.Delete(obj.EventBookedProduct);
        }

        #endregion
    }
}