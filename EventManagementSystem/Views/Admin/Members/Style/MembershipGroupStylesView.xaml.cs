using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using EventManagementSystem.Models;
using EventManagementSystem.ViewModels.Admin.Members.Style;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.GridView;

namespace EventManagementSystem.Views.Admin.Members.Style
{
    /// <summary>
    /// Interaction logic for MembershipGroupStylesView.xaml
    /// </summary>
    public partial class MembershipGroupStylesView : UserControl
    {
        private readonly MembershipGroupStylesViewModel _viewModel;

        public MembershipGroupStylesView()
        {
            InitializeComponent();

            _viewModel = new MembershipGroupStylesViewModel();
            DataContext = _viewModel;
            _viewModel.PropertyChanged += ViewModelOnPropertyChanged;

            RadGridViewGroupStyle.MouseDoubleClick += GridViewOnMouseDoubleClick;
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
                    _viewModel.EditMembershipGroupStyleCommand.Execute(row.Item as MembershipGroupStyleModel);
            }
        }
        private void OnViewLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            Loaded -= OnViewLoaded;
            _viewModel.LoadData();
        }
    }
}
