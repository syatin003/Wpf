using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using EventManagementSystem.Models;
using EventManagementSystem.ViewModels.Admin.Members.Category;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.GridView;

namespace EventManagementSystem.Views.Admin.Members.Category
{
    /// <summary>
    /// Interaction logic for MembershipCategoriesView.xaml
    /// </summary>
    public partial class MembershipCategoriesView : UserControl
    {
        private readonly MembershipCategoriesViewModel _viewModel;

        public MembershipCategoriesView()
        {
            InitializeComponent();
            _viewModel = new MembershipCategoriesViewModel();
            DataContext = _viewModel;

            _viewModel.PropertyChanged += ViewModelOnPropertyChanged;

            RadGridViewCategories.MouseDoubleClick += GridViewOnMouseDoubleClick;

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
                    _viewModel.EditMembershipCategoryCommand.Execute(row.Item as MembershipCategoryModel);
            }
        }
        private void OnViewLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            Loaded -= OnViewLoaded;
            _viewModel.LoadData();
        }
    }
}
