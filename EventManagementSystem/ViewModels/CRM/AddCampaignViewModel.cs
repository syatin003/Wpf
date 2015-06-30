using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using EventManagementSystem.Core.Commands;
using EventManagementSystem.Core.Unity;
using EventManagementSystem.Core.ViewModels;
using EventManagementSystem.Data.Model;
using EventManagementSystem.Data.UnitOfWork.Interfaces;
using EventManagementSystem.Models;
using Microsoft.Practices.Unity;

namespace EventManagementSystem.ViewModels.CRM
{
    public class AddCampaignViewModel : ViewModelBase
    {
        #region Fields

        private readonly ICrmDataUnit _crmDataUnit;
        private bool _isBusy;
        private CampaignModel _campaign;
        private bool _isEditMode;
        private ObservableCollection<CampaignType> _campaignTypes;

        private CampaignType _campaignType;
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

        public CampaignType CampaignType
        {
            get { return _campaignType; }
            set
            {
                _campaignType = value;
                RaisePropertyChanged(() => CampaignType);
                SubmitCommand.RaiseCanExecuteChanged();
            }
        }

        public CampaignModel Campaign
        {
            get { return _campaign; }
            set
            {
                if (_campaign == value) return;
                _campaign = value;
                RaisePropertyChanged(() => Campaign);
            }
        }

        public ObservableCollection<CampaignType> CampaignTypes
        {
            get { return _campaignTypes; }
            set
            {
                if (_campaignTypes == value) return;
                _campaignTypes = value;
                RaisePropertyChanged(() => CampaignTypes);
            }
        }

        public RelayCommand SubmitCommand { get; private set; }
        public RelayCommand CancelCommand { get; private set; }

        #endregion

        #region Constructor

        public AddCampaignViewModel(CampaignModel campaignModel)
        {
            var dataUnitLocator = ContainerAccessor.Instance.GetContainer().Resolve<IDataUnitLocator>();
            _crmDataUnit = dataUnitLocator.ResolveDataUnit<ICrmDataUnit>();

            SubmitCommand = new RelayCommand(SubmitCommandExecuted, SubmitCommandCanExecute);
            CancelCommand = new RelayCommand(CancelCommandExecuted);

            ProcessCampaign(campaignModel);
        }

        #endregion

        #region Methods

        private void ProcessCampaign(CampaignModel campaignModel)
        {
            _isEditMode = (campaignModel != null);

            if (_isEditMode)
            {
                CampaignType = campaignModel.CampaignType;
            }

            Campaign = (_isEditMode) ? campaignModel : GetCampaign();
            Campaign.PropertyChanged += CampaignOnPropertyChanged;
        }

        private CampaignModel GetCampaign()
        {
            var campaign = new CampaignModel(new Campaign()
            {
                ID = Guid.NewGuid(),               
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(1),
                IsActive = true
            });

            return campaign;
        }

        private void CampaignOnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            SubmitCommand.RaiseCanExecuteChanged();
        }

        public async void LoadData()
        {
            IsBusy = true;

            var types = await _crmDataUnit.CampaignTypesRepository.GetAllAsync();
            CampaignTypes = new ObservableCollection<CampaignType>(types.OrderBy(x => x.Name));

            if (_isEditMode)
            {
                var desiredCampaign = await _crmDataUnit.CampaignsRepository.GetUpdatedCampaign(_campaign.Campaign.ID);

                // Check if we have new changes
                if (desiredCampaign != null && desiredCampaign.LastEditDate != null && _campaign.LoadedTime < desiredCampaign.LastEditDate)
                {
                    Campaign = new CampaignModel(desiredCampaign);
                    CampaignType = desiredCampaign.CampaignType;
                }
            }

            IsBusy = false;
        }

        #endregion

        #region Commands

        private void SubmitCommandExecuted()
        {
            if (!_isEditMode)
            {
                Campaign.CampaignType = CampaignType;
                _crmDataUnit.CampaignsRepository.Add(Campaign.Campaign);              
            }
            else
            {
                _campaign.Campaign.CampaignTypeID = CampaignType.ID;
                _campaign.Campaign.LastEditDate = DateTime.Now;
            }

            _crmDataUnit.SaveChanges();
        }

        private bool SubmitCommandCanExecute()
        {
            return !Campaign.HasErrors && CampaignType != null;
        }

        private void CancelCommandExecuted()
        {
            _crmDataUnit.RevertChanges();

            if (_isEditMode)
            {
                Campaign.Refresh();
            }
        }

        #endregion
    }
}
