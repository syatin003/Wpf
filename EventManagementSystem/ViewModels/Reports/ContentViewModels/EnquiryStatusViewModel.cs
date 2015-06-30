using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using EventManagementSystem.Core.Unity;
using EventManagementSystem.Core.ViewModels;
using EventManagementSystem.Data.Model;
using EventManagementSystem.Data.UnitOfWork.Interfaces;
using EventManagementSystem.Models;
using Microsoft.Practices.Unity;

namespace EventManagementSystem.ViewModels.Reports.ContentViewModels
{
    public class EnquiryStatusViewModel : ViewModelBase
    {
        #region Fields

        private readonly IReportsDataUnit _reportsDataUnit;
        private bool _isBusy;
        private DateTime _startDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
        private DateTime _endDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 23, 59, 59);
        private ObservableCollection<EnquiryModel> _enquiries;
        private List<EnquiryModel> _allEnquiries;

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

        public DateTime StartDate
        {
            get { return _startDate; }
            set
            {
                if (_startDate == value) return;
                _startDate = value;
                RaisePropertyChanged(() => StartDate);
                UpdateEnquiriesDataRange();
            }
        }

        public DateTime EndDate
        {
            get { return _endDate; }
            set
            {
                if (_endDate == value) return;
                _endDate = value;
                RaisePropertyChanged(() => EndDate);
                UpdateEnquiriesDataRange();
            }
        }

        public ObservableCollection<EnquiryModel> Enquiries
        {
            get { return _enquiries; }
            set
            {
                if (_enquiries == value) return;
                _enquiries = value;
                RaisePropertyChanged(() => Enquiries);
            }
        }
     
        public List<EventType> EventTypes { get; set; }

        #endregion

        #region Constructor

        public EnquiryStatusViewModel()
        {
            var dataUnitLocator = ContainerAccessor.Instance.GetContainer().Resolve<IDataUnitLocator>();
            _reportsDataUnit = dataUnitLocator.ResolveDataUnit<IReportsDataUnit>();
        }

        #endregion

        #region Methods

        public async void LoadData()
        {
            IsBusy = true;

            var eventTypes = await _reportsDataUnit.EventTypesRepository.GetAllAsync();
            EventTypes = new List<EventType>(eventTypes);

            _reportsDataUnit.EnquiriesRepository.Refresh();
            var enquiries = await _reportsDataUnit.EnquiriesRepository.GetLightEnquiriesAsync();
            _allEnquiries = new List<EnquiryModel>(enquiries.Select(x => new EnquiryModel(x)));
            Enquiries = new ObservableCollection<EnquiryModel>(_allEnquiries);

            UpdateEnquiriesDataRange();

            IsBusy = false;
        }

        private void UpdateEnquiriesDataRange()
        {
            Enquiries = new ObservableCollection<EnquiryModel>(_allEnquiries.Where(x => x.CreationDate.Date >= StartDate.Date && x.CreationDate.Date <= EndDate.Date));
        }

        #endregion
    }
}
