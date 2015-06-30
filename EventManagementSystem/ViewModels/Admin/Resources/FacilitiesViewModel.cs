using System.Collections.ObjectModel;
using EventManagementSystem.Models;
using EventManagementSystem.Core.ViewModels;

namespace EventManagementSystem.ViewModels.Admin.Resources
{
    public class FacilitiesViewModel : ViewModelBase
    {
        #region Fields

        private ObservableCollection<FacilityModel> _facilityModels;

        #endregion

        #region Properties

        public ObservableCollection<FacilityModel> FacilityModels
        {
            get { return _facilityModels; }
            set
            {
                if (_facilityModels == value) return;
                _facilityModels = value;
                RaisePropertyChanged(() => FacilityModels);
            }
        }

        #endregion
    }
}