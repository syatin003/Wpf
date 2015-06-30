using EventManagementSystem.Core.ViewModels;
using EventManagementSystem.Data.Model;

namespace EventManagementSystem.Models
{
    public class FacilityModel : ModelBase
    {
        #region Fields

        private readonly Facility _facility;
        private bool _isSelected;

        #endregion

        #region Properties

        public Facility Facility
        {
            get { return _facility; }
        }

        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                if (_isSelected == value) return;
                _isSelected = value;
                RaisePropertyChanged(() => IsSelected);
            }
        }

        #endregion

        #region Constructors

        public FacilityModel(Facility facility)
        {
            _facility = facility;
        }

        #endregion
    }
}
