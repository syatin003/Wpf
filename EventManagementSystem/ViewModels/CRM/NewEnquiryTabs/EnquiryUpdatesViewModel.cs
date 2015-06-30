using EventManagementSystem.Core.ViewModels;
using EventManagementSystem.Models;

namespace EventManagementSystem.ViewModels.CRM.NewEnquiryTabs
{
    class EnquiryUpdatesViewModel : ViewModelBase
    {
        #region Fields

        private EnquiryModel _enquiry;

        #endregion

        #region Properties

        public EnquiryModel Enquiry
        {
            get { return _enquiry; }
            set
            {
                if (_enquiry == value) return;
                _enquiry = value;
                RaisePropertyChanged(() => Enquiry);
            }
        }

        #endregion
    }
}
