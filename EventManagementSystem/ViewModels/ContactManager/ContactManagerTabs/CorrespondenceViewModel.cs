using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using EventManagementSystem.Core.Commands;
using EventManagementSystem.Core.Unity;
using EventManagementSystem.Core.ViewModels;
using EventManagementSystem.Data.Model;
using EventManagementSystem.Data.UnitOfWork.Interfaces;
using EventManagementSystem.Models;
using EventManagementSystem.Views.ContactManager.ContactManagerTabs;
using Microsoft.Practices.Unity;

namespace EventManagementSystem.ViewModels.ContactManager.ContactManagerTabs
{
    public class CorrespondenceViewModel : ViewModelBase
    {
        #region Fields

        private readonly IContactsDataUnit _contactsDataUnit;
        private bool _isBusy;
        private ContactModel _contactModel;
        private List<CorrespondenceModel> _allCorrespondence;
        private ObservableCollection<CorrespondenceModel> _correspondence;
        private List<CCContactsCorrespondence> _ccContactsCorrespondence;
        private DateTime _startDate;
        private DateTime _endDate;
        private List<EventModel> _events;
        private List<EnquiryModel> _enquiries;
        private string _loadingMessage;
        private string view;
       
        #endregion

        #region Properties

        public string LoadingMessage
        {
            get { return _loadingMessage; }
            set
            {
                if (_loadingMessage == value) return;
                _loadingMessage = value;
                RaisePropertyChanged(() => LoadingMessage);
            }
        }

        public DateTime StartDate
        {
            get
            {
                return _startDate;
            }
            set
            {
                if (_startDate == value) return;
                _startDate = value;
                RaisePropertyChanged(() => StartDate);
                UpdateCorrespondenceCollection();
                RaisePropertyChanged(() => Correspondence);
            }
        }

        public DateTime EndDate
        {
            get
            {
                return _endDate;
            }
            set
            {
                if (_endDate == value) return;
                _endDate = value;
                RaisePropertyChanged(() => EndDate);
                UpdateCorrespondenceCollection();
                RaisePropertyChanged(() => Correspondence);
            }
        }

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

        public ContactModel ContactModel
        {
            get { return _contactModel; }
            set
            {
                if (_contactModel == value) return;
                _contactModel = value;
                RaisePropertyChanged(() => ContactModel);
            }
        }
 

        public ObservableCollection<CorrespondenceModel> Correspondence
        {
            get { return _correspondence; }
            set
            {
                if (_correspondence == value) return;
                _correspondence = value;
                RaisePropertyChanged(() => Correspondence);
            }
        }

        public RelayCommand<CorrespondenceModel> OpenEmailCommand { get; private set; }

        #endregion

        #region Constructors

        public CorrespondenceViewModel(ContactModel model,string sample)
        {
            var dataUnitLocator = ContainerAccessor.Instance.GetContainer().Resolve<IDataUnitLocator>();
            _contactsDataUnit = dataUnitLocator.ResolveDataUnit<IContactsDataUnit>();
            this.view = sample;
            _contactModel = model;

            _startDate = DateTime.Today.AddDays(-1);
            _endDate = DateTime.Today.AddDays(1);

            OpenEmailCommand = new RelayCommand<CorrespondenceModel>(OpenEmailCommandExecute);
        }

        #endregion

        #region Methods

        public async void LoadData()
        {
            IsBusy = true;

            LoadingMessage = "Loading events..";

            var events = await _contactsDataUnit.EventsRepository.GetLightEventsAsync(x => !x.IsDeleted);
            _events = new List<EventModel>(events.Select(x => new EventModel(x)));

            LoadingMessage = "Loading enquiries..";

            var enquiries = await _contactsDataUnit.EnquiriesRepository.GetLightEnquiriesAsync();
            _enquiries = new List<EnquiryModel>(enquiries.Select(x => new EnquiryModel(x)));

            LoadingMessage = "Loading correspondence..";

            var correspondence = await _contactsDataUnit.CorresponcencesRepository.GetAllAsync();
            _allCorrespondence = new List<CorrespondenceModel>(correspondence.OrderBy(x => x.Date).Select(x => new CorrespondenceModel(x)));
            Correspondence = new ObservableCollection<CorrespondenceModel>(_allCorrespondence);

            var ccContactsCorrespondence = await _contactsDataUnit.CCContactsCorrespondenceRepository.GetAllAsync();
            _ccContactsCorrespondence = new List<CCContactsCorrespondence>(ccContactsCorrespondence);

            UpdateCorrespondenceCollection();

            LoadingMessage = "..and we have done!";

            IsBusy = false;
        }

        private void UpdateCorrespondenceCollection()
        
         {
            //Correspondence = new ObservableCollection<CorrespondenceModel>(
            //    _allCorrespondence.Where(x => x.Date.Date >= StartDate.Date && x.Date.Date <= EndDate.Date && 
            //                                  ( x.Correspondence.ContactToID == _contactModel.Contact.ID
            //                                  || _ccContactsCorrespondence.Where(c => c.ContactID == _contactModel.Contact.ID).Select(c => c.CorrespondenceID).Contains(x.Correspondence.ID))
            //                                  ));
             //Correspondence = new ObservableCollection<CorrespondenceModel>(
             //        _allCorrespondence.Where(x => x.Date.Date >= StartDate.Date && x.Date.Date <= EndDate.Date &&
             //                                      (x.Correspondence.ContactToID == _contactModel.Contact.ID
             //            //|| (x.Correspondence.OwnerID == _contactModel.Contact.ID && x.Correspondence.CorresponcenceType.Type == "Member")
             //                                      || (x.Correspondence.OwnerID == _contactModel.Contact.ID && x.Correspondence.CorresponcenceType.Type == "Contact")
             //                                      || _ccContactsCorrespondence.Where(c => c.ContactID == _contactModel.Contact.ID).Select(c => c.CorrespondenceID).Contains(x.Correspondence.ID))
             //                                      ));

             if (view == "Contact")
             {
                 Correspondence = new ObservableCollection<CorrespondenceModel>(
                     _allCorrespondence.Where(x => x.Date.Date >= StartDate.Date && x.Date.Date <= EndDate.Date &&
                                                   (x.Correspondence.ContactToID == _contactModel.Contact.ID
                                                   || (x.Correspondence.OwnerID == _contactModel.Contact.ID && x.Correspondence.CorresponcenceType.Type == "Contact")
                                                   || _ccContactsCorrespondence.Where(c => c.ContactID == _contactModel.Contact.ID).Select(c => c.CorrespondenceID).Contains(x.Correspondence.ID))
                                                   ));
             }
             else if(view == "Member")
             {
                 Correspondence = new ObservableCollection<CorrespondenceModel>(
                        _allCorrespondence.Where(x => x.Date.Date >= StartDate.Date && x.Date.Date <= EndDate.Date &&
                                                      (x.Correspondence.ContactToID == _contactModel.Contact.ID
                                                   || (x.Correspondence.OwnerID == _contactModel.Contact.ID && x.Correspondence.CorresponcenceType.Type == "Member")
                                                   || _ccContactsCorrespondence.Where(c => c.ContactID == _contactModel.Contact.ID).Select(c => c.CorrespondenceID).Contains(x.Correspondence.ID))
                                                      ));
             }
            
            SetEventOrEnquiryName();
        }

        private void SetEventOrEnquiryName()
        {
            foreach (var correspondence in Correspondence)
            {
                if (correspondence.Correspondence.OwnerType == "Event")
                {
                    var firstOrDefault = _events.FirstOrDefault(x => x.Event.ID == correspondence.Correspondence.OwnerID);
                    if (firstOrDefault != null)
                        correspondence.EventName = firstOrDefault.Name;
                }
                else if (correspondence.Correspondence.OwnerType == "Enquiry")
                {
                    var firstOrDefault = _enquiries.FirstOrDefault(x => x.Enquiry.ID == correspondence.Correspondence.OwnerID);
                    if (firstOrDefault != null)
                        correspondence.EventName = firstOrDefault.Name;
                }
            }
        }

        private void OpenEmailCommandExecute(CorrespondenceModel model)
        {
            RaisePropertyChanged("DisableParentWindow");

            var window = new EmailView(model);
            window.ShowDialog();

            RaisePropertyChanged("EnableParentWindow");
        }

        #endregion
    }
}
