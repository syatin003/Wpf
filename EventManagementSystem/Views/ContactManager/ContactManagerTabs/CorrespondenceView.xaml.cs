using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using EventManagementSystem.Models;
using EventManagementSystem.ViewModels.ContactManager.ContactManagerTabs;
using Telerik.Windows.Controls;

namespace EventManagementSystem.Views.ContactManager.ContactManagerTabs
{
    /// <summary>
    /// Interaction logic for CorrespondenceView.xaml
    /// </summary>
    public partial class CorrespondenceView : UserControl
    {
        private readonly CorrespondenceViewModel _viewModel;

        public CorrespondenceView(ContactModel model)
        {
            InitializeComponent();
            DataContext = _viewModel = new CorrespondenceViewModel(model);

            _viewModel.PropertyChanged += ViewModelOnPropertyChanged;

            Loaded += OnCorrespondenceViewLoaded;
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

        private void OnCorrespondenceViewLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            _viewModel.LoadData();
        }
    }
}
