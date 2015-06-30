using System;
using System.Runtime.Serialization;
using EventManagementSystem.Core.ViewModels;
using EventManagementSystem.Data.Model;

namespace EventManagementSystem.Models
{
    [Serializable]
    public class MembershipFullPaymentCostModel : ModelBase
    {
        #region Fields

        private readonly MembershipFullPaymentComponent _membershipFullPaymentComponent;

        #endregion Fields

        #region Properties

        [DataMember]
        public MembershipFullPaymentComponent MembershipFullPaymentComponent
        {
            get { return _membershipFullPaymentComponent; }
        }

        [DataMember]
        public Product Product
        {
            get { return _membershipFullPaymentComponent.Product; }

            set
            {
                if (_membershipFullPaymentComponent.Product == value) return;
                _membershipFullPaymentComponent.Product = value;
                RaisePropertyChanged(() => Product);
                RaisePropertyChanged(() => TotalPrice);
                RaisePropertyChanged(() => TotalPoints);
                RaisePropertyChanged(() => IsAnyFullPaymentComponent);

            }
        }

        [DataMember]
        public Product Product1
        {
            get { return _membershipFullPaymentComponent.Product1; }

            set
            {
                if (_membershipFullPaymentComponent.Product1 == value) return;
                _membershipFullPaymentComponent.Product1 = value;
                RaisePropertyChanged(() => Product1);
                RaisePropertyChanged(() => TotalPrice);
                RaisePropertyChanged(() => TotalPoints);
                RaisePropertyChanged(() => IsAnyFullPaymentComponent);

            }
        }

        [DataMember]
        public Product Product2
        {
            get { return _membershipFullPaymentComponent.Product2; }

            set
            {
                if (_membershipFullPaymentComponent.Product2 == value) return;
                _membershipFullPaymentComponent.Product2 = value;
                RaisePropertyChanged(() => Product2);
                RaisePropertyChanged(() => TotalPrice);
                RaisePropertyChanged(() => TotalPoints);
                RaisePropertyChanged(() => IsAnyFullPaymentComponent);

            }
        }

        [DataMember]
        public Product Product3
        {
            get { return _membershipFullPaymentComponent.Product3; }

            set
            {
                if (_membershipFullPaymentComponent.Product3 == value) return;
                _membershipFullPaymentComponent.Product3 = value;
                RaisePropertyChanged(() => Product3);
                RaisePropertyChanged(() => TotalPrice);
                RaisePropertyChanged(() => TotalPoints);
                RaisePropertyChanged(() => IsAnyFullPaymentComponent);

            }
        }

        [DataMember]
        public Product Product4
        {
            get { return _membershipFullPaymentComponent.Product4; }

            set
            {
                if (_membershipFullPaymentComponent.Product4 == value) return;
                _membershipFullPaymentComponent.Product4 = value;
                RaisePropertyChanged(() => Product4);
                RaisePropertyChanged(() => TotalPoints);
                RaisePropertyChanged(() => TotalPrice);
                RaisePropertyChanged(() => IsAnyFullPaymentComponent);

            }
        }
        [DataMember]
        public Product Product5
        {
            get { return _membershipFullPaymentComponent.Product5; }

            set
            {
                if (_membershipFullPaymentComponent.Product5 == value) return;
                _membershipFullPaymentComponent.Product5 = value;
                RaisePropertyChanged(() => Product5);
                RaisePropertyChanged(() => TotalPrice);
                RaisePropertyChanged(() => TotalPoints);
                RaisePropertyChanged(() => IsAnyFullPaymentComponent);

            }
        }
        [DataMember]
        public Product Product6
        {
            get { return _membershipFullPaymentComponent.Product6; }

            set
            {
                if (_membershipFullPaymentComponent.Product6 == value) return;
                _membershipFullPaymentComponent.Product6 = value;
                RaisePropertyChanged(() => Product6);
                RaisePropertyChanged(() => TotalPrice);
                RaisePropertyChanged(() => TotalPoints);
                RaisePropertyChanged(() => IsAnyFullPaymentComponent);

            }
        }
        [DataMember]
        public Product Product7
        {
            get { return _membershipFullPaymentComponent.Product7; }

            set
            {
                if (_membershipFullPaymentComponent.Product7 == value) return;
                _membershipFullPaymentComponent.Product7 = value;
                RaisePropertyChanged(() => Product7);
                RaisePropertyChanged(() => TotalPrice);
                RaisePropertyChanged(() => TotalPoints);
                RaisePropertyChanged(() => IsAnyFullPaymentComponent);

            }
        }

        [DataMember]
        public double TotalPrice
        {
            get
            {
                double price = 0.0;
                if (_membershipFullPaymentComponent.Product != null)
                    price = price + _membershipFullPaymentComponent.Product.GrossPrice;
                if (_membershipFullPaymentComponent.Product1 != null)
                    price = price + _membershipFullPaymentComponent.Product1.GrossPrice;
                if (_membershipFullPaymentComponent.Product2 != null)
                    price = price + _membershipFullPaymentComponent.Product2.GrossPrice;
                if (_membershipFullPaymentComponent.Product3 != null)
                    price = price + _membershipFullPaymentComponent.Product3.GrossPrice;
                if (_membershipFullPaymentComponent.Product4 != null)
                    price = price + _membershipFullPaymentComponent.Product4.GrossPrice;
                if (_membershipFullPaymentComponent.Product5 != null)
                    price = price + _membershipFullPaymentComponent.Product5.GrossPrice;
                if (_membershipFullPaymentComponent.Product6 != null)
                    price = price + _membershipFullPaymentComponent.Product6.GrossPrice;
                if (_membershipFullPaymentComponent.Product7 != null)
                    price = price + _membershipFullPaymentComponent.Product7.GrossPrice;
                return price;
            }
        }

        public bool IsAnyFullPaymentComponent
        {
            get
            { return TotalPrice > 0.0; }
        }
        [DataMember]
        public double? TotalPoints
        {
            get
            {
                double? points = 0.0;
                if (_membershipFullPaymentComponent.Product != null)
                    points = points + _membershipFullPaymentComponent.Product.PointsReceived;
                if (_membershipFullPaymentComponent.Product1 != null)
                    points = points + _membershipFullPaymentComponent.Product1.PointsReceived;
                if (_membershipFullPaymentComponent.Product2 != null)
                    points = points + _membershipFullPaymentComponent.Product2.PointsReceived;
                if (_membershipFullPaymentComponent.Product3 != null)
                    points = points + _membershipFullPaymentComponent.Product3.PointsReceived;
                if (_membershipFullPaymentComponent.Product4 != null)
                    points = points + _membershipFullPaymentComponent.Product4.PointsReceived;
                if (_membershipFullPaymentComponent.Product5 != null)
                    points = points + _membershipFullPaymentComponent.Product5.PointsReceived;
                if (_membershipFullPaymentComponent.Product6 != null)
                    points = points + _membershipFullPaymentComponent.Product6.PointsReceived;
                if (_membershipFullPaymentComponent.Product7 != null)
                    points = points + _membershipFullPaymentComponent.Product7.PointsReceived;
                return points;
            }
        }


        #endregion Properties

        #region Constructor

        public MembershipFullPaymentCostModel(MembershipFullPaymentComponent membershipFullPaymentComponent)
        {
            _membershipFullPaymentComponent = membershipFullPaymentComponent;
        }

        #endregion Constructor
    }
}
