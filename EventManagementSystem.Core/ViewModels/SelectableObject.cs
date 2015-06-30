namespace EventManagementSystem.Core.ViewModels
{
    public class SelectableObject <T> : ModelBase
    {
        #region Fields

        private bool _isSelected;
        private T _obj;

        #endregion

        #region Properties

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

        public T Object
        {
            get { return _obj; }
            set
            {
                _obj = value;
                RaisePropertyChanged(() => Object);
            }
        }

        #endregion

        #region Constructors

        public SelectableObject(T obj)
        {
            Object = obj;
        }

        public SelectableObject(T obj, bool isSelected)
        {
            Object = obj;
            IsSelected = isSelected;
        }

        #endregion
    }
}
