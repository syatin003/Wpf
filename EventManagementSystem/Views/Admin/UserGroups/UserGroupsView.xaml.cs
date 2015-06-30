using System.Windows;
using System.Windows.Controls;
using EventManagementSystem.ViewModels.Admin.UserGroups;
using Telerik.Windows;
using Telerik.Windows.Controls;

namespace EventManagementSystem.Views.Admin.UserGroups
{
    /// <summary>
    /// Interaction logic for UserGroupsView.xaml
    /// </summary>
    public partial class UserGroupsView : UserControl
    {
        private readonly UserGroupsViewModel _viewModel;

        public UserGroupsView()
        {
            InitializeComponent();

            DataContext = _viewModel = new UserGroupsViewModel();

            Loaded += OnUserGroupsViewLoaded;

            Unloaded += UserGroupsView_Unloaded;
        }

        private void UserGroupsView_Unloaded(object sender, RoutedEventArgs e)
        {
            _viewModel.RefreshUserGroupAndRevertAllChanges();
        }

        private void OnUserGroupsViewLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            Loaded -= OnUserGroupsViewLoaded;

            _viewModel.LoadData();
        }

        /// <summary>
        ///     Hide RadDropDownButton after user clicks on MenuItem
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HideRadMenuItem_OnClick(object sender, RadRoutedEventArgs e)
        {
            var item = e.Source as RadMenuItem;

            var btn = item.ParentOfType<RadDropDownButton>();
            if (btn != null)
                btn.IsOpen = false;
        }
    }
}
