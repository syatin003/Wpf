using EventManagementSystem.Controls;
using EventManagementSystem.Core.Commands;
using EventManagementSystem.Core.ViewModels;
using EventManagementSystem.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EventManagementSystem.ViewModels.Reports.ContentViewModels
{
    public class JoinersActivityOptionsViewModel : ViewModelBase
    {
        #region Fields

        private bool _isBusy;
        private bool _incDateSale;
        private bool _incMembershipNumber;
        private bool _incMemberName;
        private bool _incSalesPerson;
        private bool _incCategoryName;
        private bool _incMembers;
        private bool _incChargeType;
        private bool _incMethodOfPayment;
        private bool _incLastPaymentMonth;
        private bool _incOutGoingMonthPayment;
        private bool _incStartDate;
        private bool _incRenewalDate;
        private bool _incIstPaymentMonth;
        private bool _incIstMonthPayment;
        private bool _incContractPeriod;
        private bool _incAnnualFeePaid;
        private bool _incJoiningFeePaid;
        private bool _incPromoSource;

        #endregion Fields

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

        public bool IncDateSale
        {
            get { return _incDateSale; }
            set
            {
                if (_incDateSale == value) return;
                _incDateSale = value;
                RaisePropertyChanged(() => IncDateSale);

            }
        }

        public bool IncMembershipNumber
        {
            get { return _incMembershipNumber; }
            set
            {
                if (_incMembershipNumber == value) return;
                _incMembershipNumber = value;
                RaisePropertyChanged(() => IncMembershipNumber);

            }
        }

        public bool IncMemberName
        {
            get { return _incMemberName; }
            set
            {
                if (_incMemberName == value) return;
                _incMemberName = value;
                RaisePropertyChanged(() => IncMemberName);

            }
        }
        public bool IncSalesPerson
        {
            get { return _incSalesPerson; }
            set
            {
                if (_incSalesPerson == value) return;
                _incSalesPerson = value;
                RaisePropertyChanged(() => IncSalesPerson);

            }
        }

        public bool IncCategoryName
        {
            get { return _incCategoryName; }
            set
            {
                if (_incCategoryName == value) return;
                _incCategoryName = value;
                RaisePropertyChanged(() => IncCategoryName);

            }
        }

        public bool IncMembers
        {
            get { return _incMembers; }
            set
            {
                if (_incMembers == value) return;
                _incMembers = value;
                RaisePropertyChanged(() => IncMembers);

            }
        }

        public bool IncChargeType
        {
            get { return _incChargeType; }
            set
            {
                if (_incChargeType == value) return;
                _incChargeType = value;
                RaisePropertyChanged(() => IncChargeType);
            }
        }

        public bool IncMethodOfPayment
        {
            get { return _incMethodOfPayment; }
            set
            {
                if (_incMethodOfPayment == value) return;
                _incMethodOfPayment = value;
                RaisePropertyChanged(() => IncMethodOfPayment);
            }
        }

        public bool IncStartDate
        {
            get { return _incStartDate; }
            set
            {
                if (_incStartDate == value) return;
                _incStartDate = value;
                RaisePropertyChanged(() => IncStartDate);
            }
        }

        public bool IncRenewalDate
        {
            get { return _incRenewalDate; }
            set
            {
                if (_incRenewalDate == value) return;
                _incRenewalDate = value;
                RaisePropertyChanged(() => IncRenewalDate);
            }
        }

        public bool IncIstPaymentMonth
        {
            get { return _incIstPaymentMonth; }
            set
            {
                if (_incIstPaymentMonth == value) return;
                _incIstPaymentMonth = value;
                RaisePropertyChanged(() => IncIstPaymentMonth);
            }
        }


        public bool IncIstMonthPayment
        {
            get { return _incIstMonthPayment; }
            set
            {
                if (_incIstMonthPayment == value) return;
                _incIstMonthPayment = value;
                RaisePropertyChanged(() => IncIstMonthPayment);
            }
        }

        public bool IncOutGoingMonthPayment
        {
            get { return _incOutGoingMonthPayment; }
            set
            {
                if (_incOutGoingMonthPayment == value) return;
                _incOutGoingMonthPayment = value;
                RaisePropertyChanged(() => IncOutGoingMonthPayment);
            }
        }

        public bool IncLastPaymentMonth
        {
            get { return _incLastPaymentMonth; }
            set
            {
                if (_incLastPaymentMonth == value) return;
                _incLastPaymentMonth = value;
                RaisePropertyChanged(() => IncLastPaymentMonth);
            }
        }

        public bool IncContractPeriod
        {
            get { return _incContractPeriod; }
            set
            {
                if (_incContractPeriod == value) return;
                _incContractPeriod = value;
                RaisePropertyChanged(() => IncContractPeriod);
            }
        }

        public bool IncAnnualFeePaid
        {
            get { return _incAnnualFeePaid; }
            set
            {
                if (_incAnnualFeePaid == value) return;
                _incAnnualFeePaid = value;
                RaisePropertyChanged(() => IncAnnualFeePaid);
            }
        }

        public bool IncJoiningFeePaid
        {
            get { return _incJoiningFeePaid; }
            set
            {
                if (_incJoiningFeePaid == value) return;
                _incJoiningFeePaid = value;
                RaisePropertyChanged(() => IncJoiningFeePaid);
            }
        }

        public bool IncPromoSource
        {
            get { return _incPromoSource; }
            set
            {
                if (_incPromoSource == value) return;
                _incPromoSource = value;
                RaisePropertyChanged(() => IncPromoSource);
            }
        }

        public RelayCommand SaveCommand { get; private set; }
        public RelayCommand CancelCommand { get; private set; }

        #endregion Properties

        #region Constructor

        public JoinersActivityOptionsViewModel()
        {
            SaveCommand = new RelayCommand(SaveCommandExecuted);
            CancelCommand = new RelayCommand(CancelCommandExecuted);
        }

        #endregion Constructor

        #region Methods

        public void ResetOptions()
        {
            LoadOptions();
        }

        public void LoadOptions()
        {
            IsBusy = true;

            IncDateSale = Properties.Settings.Default.IncDateSale;
            IncMembershipNumber = Properties.Settings.Default.IncMembershipNumber;
            IncMemberName = Properties.Settings.Default.IncMemberName;
            IncSalesPerson = Properties.Settings.Default.IncSalesPerson;
            IncCategoryName = Properties.Settings.Default.IncCategoryName;
            IncMembers = Properties.Settings.Default.IncMembers;
            IncChargeType = Properties.Settings.Default.IncChargeType;
            IncMethodOfPayment = Properties.Settings.Default.IncMethodOfPayment;
            IncStartDate = Properties.Settings.Default.IncStartDate;
            IncRenewalDate = Properties.Settings.Default.IncRenewalDate;
            IncIstPaymentMonth = Properties.Settings.Default.IncIstPaymentMonth;
            IncIstMonthPayment = Properties.Settings.Default.IncIstMonthPayment;
            IncOutGoingMonthPayment = Properties.Settings.Default.IncOutGoingMonthPayment;
            IncLastPaymentMonth = Properties.Settings.Default.IncLastPaymentMonth;
            IncContractPeriod = Properties.Settings.Default.IncContractPeriod;
            IncAnnualFeePaid = Properties.Settings.Default.IncAnnualFeePaid;
            IncJoiningFeePaid = Properties.Settings.Default.IncJoiningFeePaid;
            IncPromoSource = Properties.Settings.Default.IncPromoSource;

            IsBusy = false;
        }

        #endregion Methods

        #region Commands

        private void SaveCommandExecuted()
        {
            IsBusy = true;

            Properties.Settings.Default.IncDateSale = IncDateSale;
            Properties.Settings.Default.IncMembershipNumber = IncMembershipNumber;
            Properties.Settings.Default.IncMemberName = IncMemberName;
            Properties.Settings.Default.IncSalesPerson = IncSalesPerson;
            Properties.Settings.Default.IncCategoryName = IncCategoryName;
            Properties.Settings.Default.IncMembers = IncMembers;
            Properties.Settings.Default.IncChargeType = IncChargeType;
            Properties.Settings.Default.IncMethodOfPayment = IncMethodOfPayment;
            Properties.Settings.Default.IncStartDate = IncStartDate;
            Properties.Settings.Default.IncRenewalDate = IncRenewalDate;
            Properties.Settings.Default.IncIstPaymentMonth = IncIstPaymentMonth;
            Properties.Settings.Default.IncIstMonthPayment = IncIstMonthPayment;
            Properties.Settings.Default.IncOutGoingMonthPayment = IncOutGoingMonthPayment;
            Properties.Settings.Default.IncLastPaymentMonth = IncLastPaymentMonth;
            Properties.Settings.Default.IncContractPeriod = IncContractPeriod;
            Properties.Settings.Default.IncAnnualFeePaid = IncAnnualFeePaid;
            Properties.Settings.Default.IncJoiningFeePaid = IncJoiningFeePaid;
            Properties.Settings.Default.IncPromoSource = IncPromoSource;

            Properties.Settings.Default.Save();

            IsBusy = false;

            PopupService.ShowMessage(Properties.Resources.MESSAGE_SETTINGS_SAVED, MessageType.Successful);
        }

        private void CancelCommandExecuted()
        {
            LoadOptions();
        }

        #endregion Commands

    }
}
