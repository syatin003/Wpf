using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using EventManagementSystem.Core.ViewModels;
using EventManagementSystem.Data.Model;

namespace EventManagementSystem.Models
{
    [Serializable]
    public class CampaignModel : ModelBase, IDataErrorInfo
    {
        #region Fields

        private readonly Campaign _campaign;

        #endregion

        #region Properties

        [DataMember]
        public Campaign Campaign
        {
            get { return _campaign; }
        }

        public DateTime LoadedTime { get; set; }

        [DataMember]
        public CampaignType CampaignType
        {
            get { return _campaign.CampaignType; }
            set
            {
                if (_campaign.CampaignType == value) return;
                _campaign.CampaignType = value;
                RaisePropertyChanged(() => CampaignType);
            }
        }

        [DataMember]
        public string Name
        {
            get { return _campaign.Name; }
            set
            {
                if (_campaign.Name == value) return;
                _campaign.Name = value;
                RaisePropertyChanged(() => Name);
            }
        }

        [DataMember]
        public bool IsActive
        {
            get { return _campaign.IsActive; }
            set
            {
                if (_campaign.IsActive == value) return;
                _campaign.IsActive = value;
                RaisePropertyChanged(() => IsActive);
            }
        }

        [DataMember]
        public DateTime StartDate
        {
            get { return _campaign.StartDate; }
            set
            {
                if (_campaign.StartDate == value) return;
                _campaign.StartDate = value;
                RaisePropertyChanged(() => StartDate);
            }
        }

        [DataMember]
        public DateTime EndDate
        {
            get { return _campaign.EndDate; }
            set
            {
                if (_campaign.EndDate == value) return;
                _campaign.EndDate = value;
                RaisePropertyChanged(() => EndDate);
            }
        }

        #endregion

        #region Constructor

        public CampaignModel(Campaign campaign)
        {
            _campaign = campaign;

            LoadedTime = DateTime.Now;
        }

        #endregion

        #region Methods

        public void Refresh()
        {
            RaisePropertyChanged(() => IsActive);
            RaisePropertyChanged(() => CampaignType);
            RaisePropertyChanged(() => Name);
            RaisePropertyChanged(() => StartDate);
            RaisePropertyChanged(() => EndDate);
        }

        #endregion

        #region IDataErrorInfo

        public bool HasErrors
        {
            get { return typeof(CampaignModel).GetProperties().Any(prop => !string.IsNullOrEmpty(this[prop.Name])); }
        }

        public string this[string columnName]
        {
            get
            {
                Error = string.Empty;

                if (columnName == "Name")
                    if (string.IsNullOrWhiteSpace(Name))
                        Error = "Name can't be empty.";

                //if (columnName == "CampaignType")
                //    if (CampaignType == null)
                //        Error = "Campaign Type can't be empty!";

                return Error;
            }
        }

        public string Error { get; private set; }

        #endregion
    }
}
