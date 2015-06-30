using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using EventManagementSystem.Models;
using EventManagementSystem.ViewModels.ContactManager.ContactManagerTabs;
using Telerik.Windows.Controls;

namespace EventManagementSystem.Views.ContactManager.ContactManagerTabs
{
    /// <summary>
    /// Interaction logic for ActivityView.xaml
    /// </summary>
    public partial class ActivityView : UserControl
    {
        private readonly ActivityViewModel _viewModel;

        public ActivityView(ContactModel model)
        {
            InitializeComponent();
            DataContext = _viewModel = new ActivityViewModel(model);

            _viewModel.PropertyChanged += ViewModelOnPropertyChanged;

            Loaded += OnActivityViewLoaded;
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
        }

        private void OnActivityViewLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            _viewModel.LoadData();
        }
    }
}
