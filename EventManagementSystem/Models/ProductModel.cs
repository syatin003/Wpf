using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using EventManagementSystem.Core.ViewModels;
using EventManagementSystem.Data.Model;
using System.Runtime.Serialization;

namespace EventManagementSystem.Models
{
    public class ProductModel : ModelBase, IDataErrorInfo
    {
        #region Fields

        private readonly Product _product;
        private ObservableCollection<EventTypeModel> _eventTypes;
        private string _eventType;
        private bool _isMembership;

        #endregion

        #region Properties

        public Product Product
        {
            get { return _product; }
        }

        public ObservableCollection<EventTypeModel> EventTypes
        {
            get { return _eventTypes; }
            set
            {
                if (_eventTypes == value) return;
                _eventTypes = value;
                RaisePropertyChanged(() => EventTypes);
            }
        }

        public string EventType
        {
            get { return _eventType; }
            private set
            {
                _eventType = value;
                RaisePropertyChanged(() => EventType);
            }
        }

        public string Name
        {
            get { return _product.Name; }
            set
            {
                if (_product.Name == value) return;
                _product.Name = value;
                RaisePropertyChanged(() => Name);
            }
        }

        public float GrossPrice
        {
            get { return _product.GrossPrice; }
            set
            {
                if (_product.GrossPrice == value) return;
                _product.GrossPrice = value;
                RaisePropertyChanged(() => GrossPrice);
            }
        }

        public ProductType ProductType
        {
            get { return _product.ProductType; }
            set
            {
                if (_product.ProductType == value) return;
                _product.ProductType = value;
                RaisePropertyChanged(() => ProductType);
            }
        }

        public ProductVATRate ProductVATRate
        {
            get { return _product.ProductVATRate; }
            set
            {
                if (_product.ProductVATRate == value) return;
                _product.ProductVATRate = value;
                RaisePropertyChanged(() => ProductVATRate);
            }
        }

        public ProductGroup ProductGroup
        {
            get { return _product.ProductGroup; }
            set
            {
                if (_product.ProductGroup == value) return;
                _product.ProductGroup = value;
                RaisePropertyChanged(() => ProductGroup);
            }
        }

        public ProductDepartment ProductDepartment
        {
            get { return _product.ProductDepartment; }
            set
            {
                if (_product.ProductDepartment == value) return;
                _product.ProductDepartment = value;
                RaisePropertyChanged(() => ProductDepartment);
            }
        }

        public ProductOption ProductOption
        {
            get { return _product.ProductOption; }
            set
            {
                if (_product.ProductOption == value) return;
                _product.ProductOption = value;
                RaisePropertyChanged(() => ProductOption);
            }
        }

        public double? EarlyPaymentDiscount
        {
            get { return _product.EarlyPaymentDiscount; }
            set
            {
                if (_product.EarlyPaymentDiscount == value) return;
                _product.EarlyPaymentDiscount = value;
                RaisePropertyChanged(() => EarlyPaymentDiscount);
            }
        }

        public bool IsMembership
        {
            get { return _isMembership; }
            set
            {
                if (_isMembership == value) return;
                _isMembership = value;
                RaisePropertyChanged(() => IsMembership);
            }
        }
        #endregion

        #region Constructors

        public ProductModel(Product product)
        {
            _product = product;

            LoadEventTypes();
            RefreshEventTypesAbbreviations();
        }

        #endregion

        #region Methods

        private void LoadEventTypes()
        {
            var types = new List<EventTypeModel>();

            if (_product.ProductEventTypes.Any())
            {
                types.AddRange(_product.ProductEventTypes.Select(type => new EventTypeModel(type.EventType)));
            }
            EventTypes = new ObservableCollection<EventTypeModel>(types);
        }

        public void RefreshEventTypesAbbreviations()
        {
            var abbreviations = (from type in EventTypes where !string.IsNullOrWhiteSpace(type.EventType.Abbreviation) select type.EventType.Abbreviation).ToList();

            EventType = string.Join(",", abbreviations);
        }

        public bool HasErrors
        {
            get { return typeof(ProductModel).GetProperties().Any(prop => !string.IsNullOrEmpty(this[prop.Name])); }
        }

        #endregion

        #region IDataErrorInfo

        public string this[string columnName]
        {
            get
            {
                Error = string.Empty;

                if (columnName == "Name")
                {
                    if (string.IsNullOrEmpty(Name))
                        Error = "Name can't be empty.";
                }

                if (columnName == "ProductType")
                {
                    if (ProductType == null)
                        Error = "Type can't be empty.";
                }

                if (columnName == "GrossPrice")
                {
                    if (GrossPrice < 0)
                        Error = "Gross Price can't be empty or less than.";
                }

                if (columnName == "ProductVATRate")
                {
                    if (ProductVATRate == null)
                        Error = "VAT Rate can't be empty.";
                }

                if (columnName == "ProductGroup")
                {
                    if (ProductGroup == null)
                        Error = "Product group can't be empty.";
                }

                if (columnName == "ProductDepartment")
                {
                    if (ProductDepartment == null)
                        Error = "Product department can't be empty.";
                }
                if (!IsMembership)
                {
                    if (columnName == "ProductOption")
                    {
                        if (ProductOption == null)
                            Error = "Product option can't be empty.";
                    }
                }
                return Error;
            }
        }

        public string Error { get; private set; }

        #endregion
    }
}