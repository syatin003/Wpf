using EventManagementSystem.Core.ViewModels;
using EventManagementSystem.Models;

namespace EventManagementSystem.ViewModels.ContactManager.ContactManagerTabs
{
    public class EmailViewModel : ViewModelBase
    {
        #region Fields

        private bool _isBusy;
        private CorrespondenceModel _correspondence;      

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
     
        public CorrespondenceModel Correspondence
        {
            get { return _correspondence; }
            set
            {
                if (_correspondence == value) return;
                _correspondence = value;
                RaisePropertyChanged(() => Correspondence);
            }
        }
    
        #endregion

        #region Constructor

        public EmailViewModel(CorrespondenceModel mail)
        {         
            Correspondence = mail;
        }

        #endregion
    }
}
