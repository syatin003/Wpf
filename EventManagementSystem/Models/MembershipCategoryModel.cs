using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using EventManagementSystem.Core.ViewModels;
using EventManagementSystem.Data.Model;
using EventManagementSystem.Enums.Admin;

namespace EventManagementSystem.Models
{
    public class MembershipCategoryModel : ModelBase, IDataErrorInfo
    {
        #region Fields

        private readonly MembershipCategory _membershipCategory;

        private CategoryType? _categoryType;
        private bool _isMale;
        private bool _isFemale;
        private bool _isBoth;

        private MembershipCategoryGroupDefaultModel _membershipCategoryGroupDefault;
        private MembershipFullPaymentCostModel _membershipFullPaymentComponent;
        private MembershipMonthlyPaymentUpFrontCostModel _membershipMonthlyPaymentUpFrontCost;
        private MembershipMonthlyPaymentOngoingCostModel _membershipMonthlyPaymentOngoingCost;

        private bool _isStartDaySunday;
        private bool _isStartDayMonday;
        private bool _startDatePerMonthCalendar;
        private bool _startDatePerMonthMembership;
        private bool _startDatePerYearCalendar;
        private bool _startDatePerYearMembership;

        private bool _isExpanded;

        #endregion Fields

        #region Properties

        [DataMember]
        public MembershipCategory MembershipCategory
        {
            get { return _membershipCategory; }
        }

        [DataMember]
        public string Name
        {
            get { return _membershipCategory.Name; }
            set
            {
                if (_membershipCategory.Name == value) return;
                _membershipCategory.Name = value;
                RaisePropertyChanged(() => Name);
            }
        }
        [DataMember]
        public Gender? CategoryGender
        {
            get
            {
                if (_membershipCategory.Gender == 0)
                    return Gender.Male;
                if (_membershipCategory.Gender == 1)
                    return Gender.Female;
                if (_membershipCategory.Gender == 2)
                    return Gender.Both;
                return null;
            }
        }

        [DataMember]
        public StartDay? StartDayForRoundsPerWeek
        {
            get
            {
                if (_membershipCategory.StartDay == 0)
                    return StartDay.Sunday;
                if (_membershipCategory.StartDay == 1)
                    return StartDay.Monday;
                return null;
            }
        }

        [DataMember]
        public StartDate? StartDateForRoundsPerMonth
        {
            get
            {
                if (_membershipCategory.StartDateForMonth == 0)
                    return StartDate.Calendar;
                if (_membershipCategory.StartDateForMonth == 1)
                    return StartDate.Membership;
                return null;
            }
        }

        [DataMember]
        public StartDate? StartDateForRoundsPerYear
        {
            get
            {
                if (_membershipCategory.StartDateForYear == 0)
                    return StartDate.Calendar;
                if (_membershipCategory.StartDateForYear == 1)
                    return StartDate.Membership;
                return null;
            }
        }

        [DataMember]
        public CategoryType? CategoryType
        {
            get
            {
                if (_membershipCategory.CategoryType == 1)
                    return Enums.Admin.CategoryType.Member;
                if (_membershipCategory.CategoryType == 2)
                    return Enums.Admin.CategoryType.NonMember;
                if (_membershipCategory.CategoryType == 3)
                    return Enums.Admin.CategoryType.Corporate;
                if (_membershipCategory.CategoryType == 4)
                    return Enums.Admin.CategoryType.Event;
                return null;
            }
            set
            {
                if (_categoryType == value) return;
                _categoryType = value;
                _membershipCategory.CategoryType = Convert.ToInt32(_categoryType);
                RaisePropertyChanged(() => CategoryType);
            }
        }

        public MembershipGroupStyle MembershipGroupStyle
        {
            get { return _membershipCategory.MembershipGroupStyle; }
            set
            {
                if (_membershipCategory.MembershipGroupStyle == value) return;
                _membershipCategory.MembershipGroupStyle = value;
                _membershipCategory.MembershipGroupStyleID = _membershipCategory.MembershipGroupStyle.ID;
                RaisePropertyChanged(() => MembershipGroupStyle);

            }
        }
        public MembershipGroupAge MembershipGroupAge
        {
            get { return _membershipCategory.MembershipGroupAge; }
            set
            {
                if (_membershipCategory.MembershipGroupAge == value) return;
                _membershipCategory.MembershipGroupAge = value;
                _membershipCategory.MembershipGroupAgeID = _membershipCategory.MembershipGroupAge.ID;
                RaisePropertyChanged(() => MembershipGroupAge);
            }
        }
        public MembershipGroup MembershipGroup
        {
            get { return _membershipCategory.MembershipGroup; }
            set
            {
                if (_membershipCategory.MembershipGroup == value) return;
                _membershipCategory.MembershipGroup = value;
                _membershipCategory.MembershipGroupID = _membershipCategory.MembershipGroup.ID;
                RaisePropertyChanged(() => MembershipGroup);
            }
        }

        public bool IsMale
        {
            get
            { return (_membershipCategory.Gender == Convert.ToInt32(Gender.Male)); }
            set
            {
                if (_isMale == value) return;
                _isMale = value;
                if (_isMale)
                    _membershipCategory.Gender = Convert.ToInt32(Gender.Male);
                RaisePropertyChanged(() => IsMale);
                RaisePropertyChanged(() => CategoryGender);
            }
        }

        public bool IsFemale
        {
            get
            { return (_membershipCategory.Gender == Convert.ToInt32(Gender.Female)); }
            set
            {
                if (_isFemale == value) return;
                _isFemale = value;
                if (_isFemale)
                    _membershipCategory.Gender = Convert.ToInt32(Gender.Female);
                RaisePropertyChanged(() => IsFemale);
                RaisePropertyChanged(() => CategoryGender);

            }
        }

        public bool IsBoth
        {
            get { return (_membershipCategory.Gender == Convert.ToInt32(Gender.Both)); }
            set
            {
                if (_isBoth == value) return;
                _isBoth = value;
                if (_isBoth)
                    _membershipCategory.Gender = Convert.ToInt32(Gender.Both);
                RaisePropertyChanged(() => IsBoth);
                RaisePropertyChanged(() => CategoryGender);


            }
        }

        public MembershipCategoryGroupDefaultModel MembershipCategoryGroupDefault
        {
            get { return _membershipCategoryGroupDefault; }
            set
            {
                if (_membershipCategoryGroupDefault == value) return;
                _membershipCategoryGroupDefault = value;
                RaisePropertyChanged(() => MembershipCategoryGroupDefault);
            }
        }

        public MembershipFullPaymentCostModel MembershipFullPaymentComponent
        {
            get { return _membershipFullPaymentComponent; }
            set
            {
                if (_membershipFullPaymentComponent == value) return;
                _membershipFullPaymentComponent = value;
                RaisePropertyChanged(() => MembershipFullPaymentComponent);
            }
        }

        public MembershipMonthlyPaymentUpFrontCostModel MembershipMonthlyPaymentUpFrontCost
        {
            get { return _membershipMonthlyPaymentUpFrontCost; }
            set
            {
                if (_membershipMonthlyPaymentUpFrontCost == value) return;
                _membershipMonthlyPaymentUpFrontCost = value;
                RaisePropertyChanged(() => MembershipMonthlyPaymentUpFrontCost);
            }
        }
        public MembershipMonthlyPaymentOngoingCostModel MembershipMonthlyPaymentOngoingCost
        {
            get { return _membershipMonthlyPaymentOngoingCost; }
            set
            {
                if (_membershipMonthlyPaymentOngoingCost == value) return;
                _membershipMonthlyPaymentOngoingCost = value;
                RaisePropertyChanged(() => MembershipMonthlyPaymentOngoingCost);
            }
        }

        public bool IsStartDaySunday
        {
            get
            { return (_membershipCategory.StartDay == Convert.ToInt32(StartDay.Sunday)); }
            set
            {
                if (_isStartDaySunday == value) return;
                _isStartDaySunday = value;
                if (_isStartDaySunday)
                    _membershipCategory.StartDay = Convert.ToInt32(StartDay.Sunday);
                RaisePropertyChanged(() => IsStartDaySunday);
            }
        }
        public bool IsStartDayMonday
        {
            get
            { return (_membershipCategory.StartDay == Convert.ToInt32(StartDay.Monday)); }
            set
            {
                if (_isStartDayMonday == value) return;
                _isStartDayMonday = value;
                if (_isStartDayMonday)
                    _membershipCategory.StartDay = Convert.ToInt32(StartDay.Monday);
                RaisePropertyChanged(() => IsStartDayMonday);
            }
        }

        public bool StartDatePerMonthCalendar
        {
            get
            { return (_membershipCategory.StartDateForMonth == Convert.ToInt32(StartDate.Calendar)); }
            set
            {
                if (_startDatePerMonthCalendar == value) return;
                _startDatePerMonthCalendar = value;
                if (_startDatePerMonthCalendar)
                    _membershipCategory.StartDateForMonth = Convert.ToInt32(StartDate.Calendar);
                RaisePropertyChanged(() => StartDatePerMonthCalendar);
            }
        }
        public bool StartDatePerMonthMembership
        {
            get
            { return (_membershipCategory.StartDateForMonth == Convert.ToInt32(StartDate.Membership)); }
            set
            {
                if (_startDatePerMonthMembership == value) return;
                _startDatePerMonthMembership = value;
                if (_startDatePerMonthMembership)
                    _membershipCategory.StartDateForMonth = Convert.ToInt32(StartDate.Membership);
                RaisePropertyChanged(() => StartDatePerMonthMembership);
            }
        }

        public bool StartDatePerYearCalendar
        {
            get
            { return (_membershipCategory.StartDateForYear == Convert.ToInt32(StartDate.Calendar)); }
            set
            {
                if (_startDatePerYearCalendar == value) return;
                _startDatePerYearCalendar = value;
                if (_startDatePerYearCalendar)
                    _membershipCategory.StartDateForYear = Convert.ToInt32(StartDate.Calendar);
                RaisePropertyChanged(() => StartDatePerYearCalendar);
            }
        }
        public bool StartDatePerYearMembership
        {
            get
            { return (_membershipCategory.StartDateForYear == Convert.ToInt32(StartDate.Membership)); }
            set
            {
                if (_startDatePerYearMembership == value) return;
                _startDatePerYearMembership = value;
                if (_startDatePerYearMembership)
                    _membershipCategory.StartDateForYear = Convert.ToInt32(StartDate.Membership);
                RaisePropertyChanged(() => StartDatePerYearMembership);
            }
        }

        public bool IsExpanded
        {
            get { return _isExpanded; }
            set
            {
                if (_isExpanded == value) return;
                _isExpanded = value;
                RaisePropertyChanged(() => IsExpanded);
            }
        }

        #endregion Properties

        #region Constructor

        public MembershipCategoryModel(MembershipCategory membershipCategory)
        {
            _membershipCategory = membershipCategory;
            //if (_membershipCategory.MembershipCategoryGroupDefaults.FirstOrDefault() != null)
            if (_membershipCategory.MembershipCategoryGroupDefault != null)
                MembershipCategoryGroupDefault = new MembershipCategoryGroupDefaultModel(_membershipCategory.MembershipCategoryGroupDefault);

            //if (_membershipCategory.MembershipFullPaymentComponents.FirstOrDefault() != null)
            if (_membershipCategory.MembershipFullPaymentComponent != null)
                MembershipFullPaymentComponent = new MembershipFullPaymentCostModel(_membershipCategory.MembershipFullPaymentComponent);

            //if (_membershipCategory.MembershipMonthlyPaymentUpFrontCosts.FirstOrDefault() != null)
            if (_membershipCategory.MembershipMonthlyPaymentUpFrontCost != null)
                MembershipMonthlyPaymentUpFrontCost = new MembershipMonthlyPaymentUpFrontCostModel(_membershipCategory.MembershipMonthlyPaymentUpFrontCost);

            //if (_membershipCategory.MembershipMonthlyPaymentOngoingCosts.FirstOrDefault() != null)
            if (_membershipCategory.MembershipMonthlyPaymentOngoingCost != null)
                MembershipMonthlyPaymentOngoingCost = new MembershipMonthlyPaymentOngoingCostModel(_membershipCategory.MembershipMonthlyPaymentOngoingCost);
        }

        #endregion Constructor

        //#region Methods

        //public override bool Equals(object obj)
        //{
            

        //    return true;
        //}

        //#endregion



        #region IDataErrorInfo Properties

        /// <summary>
        /// Indicates whenever the model has errors
        /// </summary>
        public bool HasErrors
        {
            get { return typeof(MembershipCategoryModel).GetProperties().Any(prop => !string.IsNullOrEmpty(this[prop.Name])); }
        }

        public string this[string columnName]
        {
            get
            {
                Error = string.Empty;
                if (columnName == "Name")
                    if (string.IsNullOrWhiteSpace(Name))
                        Error = "Name can't be empty!";
                if (columnName == "MembershipGroupStyle")
                    if (MembershipGroupStyle == null)
                        Error = "Membership group style can't be empty!";
                if (columnName == "MembershipGroupAge")
                    if (MembershipGroupAge == null)
                        Error = "Membership group age can't be empty!";
                if (columnName == "MembershipGroup")
                    if (MembershipGroup == null)
                        Error = "Membership group can't be empty!";
                if (columnName == "CategoryType")
                    if (CategoryType == null)
                        Error = "Category type can't be empty!";
                return Error;
            }
        }

        public string Error { get; private set; }

        #endregion
    }
}
