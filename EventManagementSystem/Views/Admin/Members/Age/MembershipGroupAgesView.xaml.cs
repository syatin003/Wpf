using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using EventManagementSystem.Models;
using EventManagementSystem.ViewModels.Admin.Members.Age;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.GridView;

namespace EventManagementSystem.Views.Admin.Members.Age
{
    /// <summary>
    /// Interaction logic for MembershipGroupAgesView.xaml
    /// </summary>
    public partial class MembershipGroupAgesView : UserControl
    {
        private readonly MembershipGroupAgesViewModel _viewModel;

        public MembershipGroupAgesView()
        {
            InitializeComponent();

            _viewModel = new MembershipGroupAgesViewModel();
            DataContext = _viewModel;
            _viewModel.PropertyChanged += ViewModelOnPropertyChanged;

            RadGridViewGroupAge.MouseDoubleClick += GridViewOnMouseDoubleClick;
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
                    _viewModel.EditMembershipGroupAgeCommand.Execute(row.Item as MembershipGroupAgeModel);
            }
        }
        private void OnViewLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            Loaded -= OnViewLoaded;
            _viewModel.LoadData();
        }
    }
}
