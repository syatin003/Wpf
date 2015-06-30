using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using EventManagementSystem.Models;
using EventManagementSystem.ViewModels.Admin.Members.Group;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.GridView;

namespace EventManagementSystem.Views.Admin.Members.Group
{
    /// <summary>
    /// Interaction logic for MembershipGroupsView.xaml
    /// </summary>
    public partial class MembershipGroupsView : UserControl
    {
        private readonly MembershipGroupsViewModel _viewModel;

        public MembershipGroupsView()
        {
            InitializeComponent();

            _viewModel = new MembershipGroupsViewModel();
            DataContext = _viewModel;

            _viewModel.PropertyChanged += ViewModelOnPropertyChanged;

            RadGridViewGroups.MouseDoubleClick += GridViewOnMouseDoubleClick;

            Loaded += OnViewLoaded;
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
        private void GridViewOnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var originalSender = e.OriginalSource as FrameworkElement;
            if (originalSender != null)
            {
                var row = originalSender.ParentOfType<GridViewRow>();
                if (row != null)
                    _viewModel.EditMembershipGroupCommand.Execute(row.Item as MembershipGroupModel);
            }
        }
        private void OnViewLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            _viewModel.LoadData();
        }
    }
}
