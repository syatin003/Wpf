using System.Windows;
using EventManagementSystem.Models;
using EventManagementSystem.ViewModels.Admin.Members.Category;
using Telerik.Windows.Controls;

namespace EventManagementSystem.Views.Admin.Members.Category
{
    /// <summary>
    /// Interaction logic for AddMembershipCategoryView.xaml
    /// </summary>
    public partial class AddMembershipCategoryView : RadWindow
    {
        public readonly AddMembershipCategoryViewModel ViewModel;

        public AddMembershipCategoryView(MembershipCategoryModel membershipCategory)
        {
            InitializeComponent();
            Owner = Application.Current.MainWindow;
            if (membershipCategory != null)
                Header = "Edit Category";
            ViewModel = new AddMembershipCategoryViewModel(membershipCategory);
            DataContext = ViewModel;
            Loaded += OnViewLoaded;
        }
        private void OnViewLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            Loaded -= OnViewLoaded;
            ViewModel.LoadData();
        }
        private void OkButton_OnClick(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }
    }
}
