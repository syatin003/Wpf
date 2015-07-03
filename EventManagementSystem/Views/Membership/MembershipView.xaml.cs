using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using EventManagementSystem.ViewModels.Membership;
using Telerik.Windows.Controls;

namespace EventManagementSystem.Views.Membership
{
    /// <summary>
    /// Interaction logic for MembershipView.xaml
    /// </summary>
    public partial class MembershipView : UserControl
    {
        private readonly MembershipViewModel _viewModel;

        public MembershipView()
        {
            InitializeComponent();
            DataContext = _viewModel = new MembershipViewModel();

            _viewModel.PropertyChanged += ViewModelOnPropertyChanged;

            Loaded += OnMembersListViewLoaded;
            IsVisibleChanged += MemberView_IsVisibleChanged;
        }

        private void MemberView_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (!IsVisible)
                _viewModel.ChangeSelectedMember();
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
                MembersRadGridView.ScrollIntoView(MembersRadGridView.SelectedItem);
            }
        }

        private void OnMembersListViewLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            if (_viewModel.SelectedTab == 0)
                _viewModel.LoadData();
        }
    }
}
