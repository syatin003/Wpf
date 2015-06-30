using EventManagementSystem.Core.ViewModels;

namespace EventManagementSystem.Helpers
{
    public class CheckedListItem<T> : ObservableObject
    {
        #region Fields

        private bool _isChecked;
        private T _item;

        #endregion

        #region Properties

        public T Item
        {
            get { return _item; }
            set
            {
                _item = value;
                RaisePropertyChanged(() => Item);
            }
        }

        public bool IsChecked
        {
            get { return _isChecked; }
            set
            {
                _isChecked = value;
                RaisePropertyChanged(() => IsChecked);
            }
        }

        #endregion

        #region Constructors

        public CheckedListItem() {}

        public CheckedListItem(T item, bool isChecked)
        {
            this.Item = item;
            this.IsChecked = isChecked;
        }

        #endregion
    }
}
