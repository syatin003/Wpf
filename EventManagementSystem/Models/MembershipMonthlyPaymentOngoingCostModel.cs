using System;
using System.Runtime.Serialization;
using EventManagementSystem.Core.ViewModels;
using EventManagementSystem.Data.Model;

namespace EventManagementSystem.Models
{
    [Serializable]
    public class MembershipMonthlyPaymentOngoingCostModel : ModelBase
    {
        #region Fields

        private readonly MembershipMonthlyPaymentOngoingCost _membershipMonthlyPaymentOngoingCost;

        #endregion Fields

        #region Properties

        [DataMember]
        public MembershipMonthlyPaymentOngoingCost MembershipMonthlyPaymentOngoingCost
        {
            get { return _membershipMonthlyPaymentOngoingCost; }
        }

        [DataMember]
        public Product Product
        {
            get { return _membershipMonthlyPaymentOngoingCost.Product; }

            set
            {
                if (_membershipMonthlyPaymentOngoingCost.Product == value) return;
                _membershipMonthlyPaymentOngoingCost.Product = value;
                RaisePropertyChanged(() => Product);
                RaisePropertyChanged(() => TotalPrice);
                RaisePropertyChanged(() => TotalPoints);
                RaisePropertyChanged(() => IsAnyOngoingCostComponent);


            }
        }

        [DataMember]
        public Product Product1
        {
            get { return _membershipMonthlyPaymentOngoingCost.Product1; }

            set
            {
                if (_membershipMonthlyPaymentOngoingCost.Product1 == value) return;
                _membershipMonthlyPaymentOngoingCost.Product1 = value;
                RaisePropertyChanged(() => Product1);
                RaisePropertyChanged(() => TotalPrice);
                RaisePropertyChanged(() => TotalPoints);
                RaisePropertyChanged(() => IsAnyOngoingCostComponent);

            }
        }

        [DataMember]
        public Product Product2
        {
            get { return _membershipMonthlyPaymentOngoingCost.Product2; }

            set
            {
                if (_membershipMonthlyPaymentOngoingCost.Product2 == value) return;
                _membershipMonthlyPaymentOngoingCost.Product2 = value;
                RaisePropertyChanged(() => Product2);
                RaisePropertyChanged(() => TotalPrice);
                RaisePropertyChanged(() => TotalPoints);
                RaisePropertyChanged(() => IsAnyOngoingCostComponent);

            }
        }

        [DataMember]
        public Product Product3
        {
            get { return _membershipMonthlyPaymentOngoingCost.Product3; }

            set
            {
                if (_membershipMonthlyPaymentOngoingCost.Product3 == value) return;
                _membershipMonthlyPaymentOngoingCost.Product3 = value;
                RaisePropertyChanged(() => Product3);
                RaisePropertyChanged(() => TotalPrice);
                RaisePropertyChanged(() => TotalPoints);
                RaisePropertyChanged(() => IsAnyOngoingCostComponent);

            }
        }

        [DataMember]
        public double TotalPrice
        {
            get
            {
                double price = 0;
                if (_membershipMonthlyPaymentOngoingCost.Product != null)
                    price = price + _membershipMonthlyPaymentOngoingCost.Product.GrossPrice;
                if (_membershipMonthlyPaymentOngoingCost.Product1 != null)
                    price = price + _membershipMonthlyPaymentOngoingCost.Product1.GrossPrice;
                if (_membershipMonthlyPaymentOngoingCost.Product2 != null)
                    price = price + _membershipMonthlyPaymentOngoingCost.Product2.GrossPrice;
                if (_membershipMonthlyPaymentOngoingCost.Product3 != null)
                    price = price + _membershipMonthlyPaymentOngoingCost.Product3.GrossPrice;
                return price;
            }
        }

        public bool IsAnyOngoingCostComponent
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
                if (_membershipMonthlyPaymentOngoingCost.Product != null)
                    points = points + _membershipMonthlyPaymentOngoingCost.Product.PointsReceived;
                if (_membershipMonthlyPaymentOngoingCost.Product1 != null)
                    points = points + _membershipMonthlyPaymentOngoingCost.Product1.PointsReceived;
                if (_membershipMonthlyPaymentOngoingCost.Product2 != null)
                    points = points + _membershipMonthlyPaymentOngoingCost.Product2.PointsReceived;
                if (_membershipMonthlyPaymentOngoingCost.Product3 != null)
                    points = points + _membershipMonthlyPaymentOngoingCost.Product3.PointsReceived;
                return points;
            }
        }


        #endregion Properties

        #region Constructor

        public MembershipMonthlyPaymentOngoingCostModel(MembershipMonthlyPaymentOngoingCost membershipMonthlyPaymentOngoingCost)
        {
            _membershipMonthlyPaymentOngoingCost = membershipMonthlyPaymentOngoingCost;
        }

        #endregion Constructor
    }
}
