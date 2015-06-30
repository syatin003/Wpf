using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventManagementSystem.Core.ViewModels;

namespace EventManagementSystem.Models
{
    class ForwardBookModel : ViewModelBase
    {
        #region Fields

        private double _grandTotal;
        private string _productName;
        private string _eventType;

        private List<ForwardBookPropertiesModel> _properties; 

        #endregion

        #region Properties

        public double GrandTotal
        {
            get { return _grandTotal; }
            set
            {
                if (_grandTotal == value) return;
                _grandTotal = value;
                RaisePropertyChanged(() => GrandTotal);
            }
        }

        public string ProductName
        {
            get { return _productName; }
            set
            {
                if (_productName == value) return;
                _productName = value;
                RaisePropertyChanged(() => ProductName);
            }
        }

        public string EventType
        {
            get { return _eventType; }
            set
            {
                if (_eventType == value) return;
                _eventType = value;
                RaisePropertyChanged(() => EventType);
            }
        }     

        public List<ForwardBookPropertiesModel> Properties
        {
            get { return _properties; }
            set
            {
                if (_properties == value) return;
                _properties = value;
                RaisePropertyChanged(() => Properties);
            }
        }

        #endregion
    }
}
