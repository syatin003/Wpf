using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using EventManagementSystem.ViewModels.ContactManager;
using Telerik.Windows.Controls;

namespace EventManagementSystem.Views.ContactManager
{
    /// <summary>
    /// Interaction logic for ContactsView.xaml
    /// </summary>
    public partial class ContactManagerView : UserControl
    {
        private readonly ContactManagerViewModel _viewModel;

        public ContactManagerView()
        {
            InitializeComponent();
            DataContext = _viewModel = new ContactManagerViewModel();

            _viewModel.PropertyChanged += ViewModelOnPropertyChanged;

            Loaded += OnContactsListViewLoaded;
            IsVisibleChanged += ContactManagerView_IsVisibleChanged;
        }

        private void ContactManagerView_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (!IsVisible)
            {
                _viewModel.ChangeSelectedContact();
            }
        }

        private void ViewModelOnPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            var tile = this.ParentOfType<RadTileView>();

            if (args.PropertyName == "EnableParentWindow")
            {
                tile.IsEnabled = true;
            }
            else if (args.PropertyName == "DisableParentWindow")
            {
                tile.IsEnabled = false;
            }
            else if (args.PropertyName == "ScrollToSelectedItem")
            {
                ContactsRadGridView.ScrollIntoView(ContactsRadGridView.SelectedItem);
            }
        }

        private void OnContactsListViewLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            _viewModel.LoadData();
        }

    }
}
