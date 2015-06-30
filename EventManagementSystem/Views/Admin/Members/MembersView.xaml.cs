using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using EventManagementSystem.ViewModels.Admin.Members;
using EventManagementSystem.ViewModels.Admin.Members.LinkTypes;
using Telerik.Windows;
using Telerik.Windows.Controls;

namespace EventManagementSystem.Views.Admin.Members
{
    /// <summary>
    /// Interaction logic for MembersView.xaml
    /// </summary>
    public partial class MembersView : UserControl
    {
        private readonly MembersViewModel _viewModel;
        public MembersView()
        {
            InitializeComponent();

            _viewModel = new MembersViewModel();
            DataContext = _viewModel;
            _viewModel.PropertyChanged += ViewModelOnPropertyChanged;
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
            else if (args.PropertyName == "SelectTreeViewItem")
            {
                RadMembersTreeView.ExpandItemByPath(_viewModel.TreeViewItemPath, "|");
                RadMembersTreeView.SelectItemByPath(_viewModel.TreeViewItemPath, "|");
            }
        }

        private void OnViewLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            Loaded -= OnViewLoaded;
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
