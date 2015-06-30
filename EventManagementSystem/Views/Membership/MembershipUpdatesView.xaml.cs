using System.ComponentModel;
using System.Windows;
using Telerik.Windows.Controls;
using EventManagementSystem.ViewModels.Membership;

namespace EventManagementSystem.Views.Membership
{
    /// <summary>
    /// Interaction logic for MembershipUpdatesView.xaml
    /// </summary>
    public partial class MembershipUpdatesView
    {
        private readonly MembershipUpdatesViewModel _viewModel;

        public MembershipUpdatesView()
        {
            InitializeComponent();
            DataContext = _viewModel = new MembershipUpdatesViewModel();
            _viewModel.PropertyChanged += ViewModelOnPropertyChanged;

            IsVisibleChanged += OnIsVisibleChanged;
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
            else if (args.PropertyName == "OnDataLoaded")
            {
                _viewModel.IsBusy = false;
            }
        }

        private void OnIsVisibleChanged(object sender, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            if (IsVisible)
                _viewModel.LoadData();
        }
    }
}
