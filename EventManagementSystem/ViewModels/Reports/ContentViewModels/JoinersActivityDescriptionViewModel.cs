using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using EventManagementSystem.Core.Unity;
using EventManagementSystem.Core.ViewModels;
using EventManagementSystem.Data.UnitOfWork.Interfaces;
using EventManagementSystem.Models;
using Microsoft.Practices.Unity;

namespace EventManagementSystem.ViewModels.Reports.ContentViewModels
{
    public class JoinersActivityDescriptionViewModel : ViewModelBase
    {
        #region Fields

        private readonly IReportsDataUnit _reportsDataUnit;
        private bool _isBusy;
        private List<MemberModel> _allMembers;
        private ObservableCollection<JoinerActivityModel> _joinersActivity;
        private DateTime _startDate;
        private DateTime _endDate;

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

        public ObservableCollection<JoinerActivityModel> JoinersActivity
        {
            get { return _joinersActivity; }
            set
            {
                if (_joinersActivity == value) return;
                _joinersActivity = value;
                RaisePropertyChanged(() => JoinersActivity);
            }
        }

        public DateTime StartDate
        {
            get { return _startDate; }
            set
            {
                if (_startDate == value) return;
                _startDate = value;
                Properties.Settings.Default.StartDateOptionJoinersActivity = _startDate;
                RaisePropertyChanged(() => StartDate);
                UpdateJoinersActivityData();

            }
        }

        public DateTime EndDate
        {
            get { return _endDate; }
            set
            {
                if (_endDate == value) return;
                _endDate = value;
                Properties.Settings.Default.EndDateOptionJoinersActivity = _endDate;
                RaisePropertyChanged(() => EndDate);
                UpdateJoinersActivityData();
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

        #endregion  Properties

        #region Constructor

        public JoinersActivityDescriptionViewModel()
        {
            var dataUnitLocator = ContainerAccessor.Instance.GetContainer().Resolve<IDataUnitLocator>();
            _reportsDataUnit = dataUnitLocator.ResolveDataUnit<IReportsDataUnit>();
            _allMembers = new List<MemberModel>();
        }

        #endregion  Constructor

        #region Methods

        public async void LoadData()
        {
            IsBusy = true;

            LoadOptions();

            _reportsDataUnit.MembersRepository.Refresh();
            var members = await _reportsDataUnit.MembersRepository.GetAllAsync();
            _allMembers = new List<MemberModel>(members.Select(member => new MemberModel(member)));

            UpdateJoinersActivityData();

            IsBusy = false;
        }

        private void UpdateJoinersActivityData()
        {
            var members = (_allMembers.Where(member => member.StartDate >= StartDate && member.StartDate <= EndDate));
            JoinersActivity = new ObservableCollection<JoinerActivityModel>();
            foreach (var memberItem in members)
            {
                var joinerActivity = new JoinerActivityModel
                {
                    MemberName = string.Format("{0} {1}", (memberItem.Contact.Title) != null ? memberItem.Contact.Title.Title : "", memberItem.Contact.ContactName),
                    CategoryName = memberItem.Category.Name,
                    Members = memberItem.Category.Members.Count,
                    StartDate = Convert.ToDateTime(memberItem.StartDate),
                    RenewalDate = Convert.ToDateTime(memberItem.RenewalDate),
                    /* Will be Available when we will add the accounts package.                   
                                      DateSale            =;
                                      MembershipNumber    =;
                                      SalesPerson         =;
                                      ChargeType          =;
                                      MethodOfPayment     =;
                                      IstPaymentMonth     =;
                                      IstMonthPayment     =;
                                      OutGoingMonthPayment=;
                                      LastPaymentMonth    =;
                                      ContractPeriod      =;
                                      AnnualFeePaid       =;
                                      JoiningFeePaid      =;
                                      PromoSource         =;    */

                };
                if (joinerActivity.DateSale == default(DateTime))
                    joinerActivity.DateSale = null;
                if (joinerActivity.IstPaymentMonth == default(DateTime))
                    joinerActivity.IstPaymentMonth = null;
                if (joinerActivity.LastPaymentMonth == default(DateTime))
                    joinerActivity.LastPaymentMonth = null;
                JoinersActivity.Add(joinerActivity);
            }
        }

        public void LoadOptions()
        {
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
            if (Properties.Settings.Default.StartDateOptionJoinersActivity == default(DateTime) && Properties.Settings.Default.EndDateOptionJoinersActivity == default(DateTime))
            {
                Properties.Settings.Default.StartDateOptionJoinersActivity = new DateTime(DateTime.Now.Date.Year, 01, 01, 0, 0, 0);
                Properties.Settings.Default.EndDateOptionJoinersActivity = DateTime.Now;
            }
            StartDate = Properties.Settings.Default.StartDateOptionJoinersActivity;
            EndDate = Properties.Settings.Default.EndDateOptionJoinersActivity;
        }
        #endregion  Methods
    }
}
