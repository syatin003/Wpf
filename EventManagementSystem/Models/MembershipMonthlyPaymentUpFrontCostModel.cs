using System;
using System.Runtime.Serialization;
using EventManagementSystem.Core.ViewModels;
using EventManagementSystem.Data.Model;

namespace EventManagementSystem.Models
{
    [Serializable]
    public class MembershipMonthlyPaymentUpFrontCostModel : ModelBase
    {
        #region Fields

        private readonly MembershipMonthlyPaymentUpFrontCost _membershipMonthlyPaymentUpFrontCost;

        #endregion Fields

        #region Properties

        [DataMember]
        public MembershipMonthlyPaymentUpFrontCost MembershipMonthlyPaymentUpFrontCost
        {
            get { return _membershipMonthlyPaymentUpFrontCost; }
        }

        [DataMember]
        public Product Product
        {
            get { return _membershipMonthlyPaymentUpFrontCost.Product; }

            set
            {
                if (_membershipMonthlyPaymentUpFrontCost.Product == value) return;
                _membershipMonthlyPaymentUpFrontCost.Product = value;
                RaisePropertyChanged(() => Product);
                RaisePropertyChanged(() => TotalPrice);
                RaisePropertyChanged(() => TotalPoints);
                RaisePropertyChanged(() => IsAnyUpFrontCostComponent);

            }
        }

        [DataMember]
        public Product Product1
        {
            get { return _membershipMonthlyPaymentUpFrontCost.Product1; }

            set
            {
                if (_membershipMonthlyPaymentUpFrontCost.Product1 == value) return;
                _membershipMonthlyPaymentUpFrontCost.Product1 = value;
                RaisePropertyChanged(() => Product1);
                RaisePropertyChanged(() => TotalPrice);
                RaisePropertyChanged(() => TotalPoints);
                RaisePropertyChanged(() => IsAnyUpFrontCostComponent);

            }
        }

        [DataMember]
        public Product Product2
        {
            get { return _membershipMonthlyPaymentUpFrontCost.Product2; }

            set
            {
                if (_membershipMonthlyPaymentUpFrontCost.Product2 == value) return;
                _membershipMonthlyPaymentUpFrontCost.Product2 = value;
                RaisePropertyChanged(() => Product2);
                RaisePropertyChanged(() => TotalPoints);
                RaisePropertyChanged(() => TotalPrice);
                RaisePropertyChanged(() => IsAnyUpFrontCostComponent);

            }
        }

        [DataMember]
        public Product Product3
        {
            get { return _membershipMonthlyPaymentUpFrontCost.Product3; }

            set
            {
                if (_membershipMonthlyPaymentUpFrontCost.Product3 == value) return;
                _membershipMonthlyPaymentUpFrontCost.Product3 = value;
                RaisePropertyChanged(() => Product3);
                RaisePropertyChanged(() => TotalPrice);
                RaisePropertyChanged(() => TotalPoints);
                RaisePropertyChanged(() => IsAnyUpFrontCostComponent);

            }
        }

        [DataMember]
        public Product Product4
        {
            get { return _membershipMonthlyPaymentUpFrontCost.Product4; }

            set
            {
                if (_membershipMonthlyPaymentUpFrontCost.Product4 == value) return;
                _membershipMonthlyPaymentUpFrontCost.Product4 = value;
                RaisePropertyChanged(() => Product4);
                RaisePropertyChanged(() => TotalPrice);
                RaisePropertyChanged(() => TotalPoints);
                RaisePropertyChanged(() => IsAnyUpFrontCostComponent);

            }
        }
        [DataMember]
        public Product Product5
        {
            get { return _membershipMonthlyPaymentUpFrontCost.Product5; }

            set
            {
                if (_membershipMonthlyPaymentUpFrontCost.Product5 == value) return;
                _membershipMonthlyPaymentUpFrontCost.Product5 = value;
                RaisePropertyChanged(() => Product5);
                RaisePropertyChanged(() => TotalPrice);
                RaisePropertyChanged(() => TotalPoints);
                RaisePropertyChanged(() => IsAnyUpFrontCostComponent);

            }
        }
        [DataMember]
        public Product Product6
        {
            get { return _membershipMonthlyPaymentUpFrontCost.Product6; }

            set
            {
                if (_membershipMonthlyPaymentUpFrontCost.Product6 == value) return;
                _membershipMonthlyPaymentUpFrontCost.Product6 = value;
                RaisePropertyChanged(() => Product6);
                RaisePropertyChanged(() => TotalPrice);
                RaisePropertyChanged(() => TotalPoints);
                RaisePropertyChanged(() => IsAnyUpFrontCostComponent);

            }
        }
        [DataMember]
        public Product Product7
        {
            get { return _membershipMonthlyPaymentUpFrontCost.Product7; }

            set
            {
                if (_membershipMonthlyPaymentUpFrontCost.Product7 == value) return;
                _membershipMonthlyPaymentUpFrontCost.Product7 = value;
                RaisePropertyChanged(() => Product7);
                RaisePropertyChanged(() => TotalPrice);
                RaisePropertyChanged(() => TotalPoints);
                RaisePropertyChanged(() => IsAnyUpFrontCostComponent);

            }
        }


        [DataMember]
        public double TotalPrice
        {
            get
            {
                double price = 0.0;
                if (_membershipMonthlyPaymentUpFrontCost.Product != null)
                    price = price + _membershipMonthlyPaymentUpFrontCost.Product.GrossPrice;
                if (_membershipMonthlyPaymentUpFrontCost.Product1 != null)
                    price = price + _membershipMonthlyPaymentUpFrontCost.Product1.GrossPrice;
                if (_membershipMonthlyPaymentUpFrontCost.Product2 != null)
                    price = price + _membershipMonthlyPaymentUpFrontCost.Product2.GrossPrice;
                if (_membershipMonthlyPaymentUpFrontCost.Product3 != null)
                    price = price + _membershipMonthlyPaymentUpFrontCost.Product3.GrossPrice;
                if (_membershipMonthlyPaymentUpFrontCost.Product4 != null)
                    price = price + _membershipMonthlyPaymentUpFrontCost.Product4.GrossPrice;
                if (_membershipMonthlyPaymentUpFrontCost.Product5 != null)
                    price = price + _membershipMonthlyPaymentUpFrontCost.Product5.GrossPrice;
                if (_membershipMonthlyPaymentUpFrontCost.Product6 != null)
                    price = price + _membershipMonthlyPaymentUpFrontCost.Product6.GrossPrice;
                if (_membershipMonthlyPaymentUpFrontCost.Product7 != null)
                    price = price + _membershipMonthlyPaymentUpFrontCost.Product7.GrossPrice;
                return price;
            }
        }

        public bool IsAnyUpFrontCostComponent
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
                if (_membershipMonthlyPaymentUpFrontCost.Product != null)
                    points = points + _membershipMonthlyPaymentUpFrontCost.Product.PointsReceived;
                if (_membershipMonthlyPaymentUpFrontCost.Product1 != null)
                    points = points + _membershipMonthlyPaymentUpFrontCost.Product1.PointsReceived;
                if (_membershipMonthlyPaymentUpFrontCost.Product2 != null)
                    points = points + _membershipMonthlyPaymentUpFrontCost.Product2.PointsReceived;
                if (_membershipMonthlyPaymentUpFrontCost.Product3 != null)
                    points = points + _membershipMonthlyPaymentUpFrontCost.Product3.PointsReceived;
                if (_membershipMonthlyPaymentUpFrontCost.Product4 != null)
                    points = points + _membershipMonthlyPaymentUpFrontCost.Product4.PointsReceived;
                if (_membershipMonthlyPaymentUpFrontCost.Product5 != null)
                    points = points + _membershipMonthlyPaymentUpFrontCost.Product5.PointsReceived;
                if (_membershipMonthlyPaymentUpFrontCost.Product6 != null)
                    points = points + _membershipMonthlyPaymentUpFrontCost.Product6.PointsReceived;
                if (_membershipMonthlyPaymentUpFrontCost.Product7 != null)
                    points = points + _membershipMonthlyPaymentUpFrontCost.Product7.PointsReceived;
                return points;
            }
        }
        #endregion Properties


        #region Constructor

        public MembershipMonthlyPaymentUpFrontCostModel(MembershipMonthlyPaymentUpFrontCost membershipMonthlyPaymentUpFrontCost)
        {
            _membershipMonthlyPaymentUpFrontCost = membershipMonthlyPaymentUpFrontCost;
        }

        #endregion Constructor
    }
}
