using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using EventManagementSystem.Models;
using EventManagementSystem.ViewModels.Admin.Members.LinkTypes;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.GridView;

namespace EventManagementSystem.Views.Admin.Members.LinkTypes
{
    /// <summary>
    /// Interaction logic for MembershipLinkTypesView.xaml
    /// </summary>
    public partial class MembershipLinkTypesView : UserControl
    {
        private readonly MembershipLinkTypesViewModel _viewModel;

        public MembershipLinkTypesView()
        {
            InitializeComponent();
            _viewModel = new MembershipLinkTypesViewModel();
            DataContext = _viewModel;

            _viewModel.PropertyChanged += ViewModelOnPropertyChanged;

            RadGridViewLinkTypes.MouseDoubleClick += GridViewOnMouseDoubleClick;

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
                    _viewModel.EditMembershipLinkTypeCommand.Execute(row.Item as MembershipLinkTypeModel);
            }
        }
        private void OnViewLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            Loaded -= OnViewLoaded;
            _viewModel.LoadData();
        }
    }
}
