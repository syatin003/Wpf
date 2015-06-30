using System.Windows;
using EventManagementSystem.Models;
using EventManagementSystem.ViewModels.Admin.Members.Age;
using Telerik.Windows.Controls;

namespace EventManagementSystem.Views.Admin.Members.Age
{
    /// <summary>
    /// Interaction logic for AddMembershipGroupAgeView.xaml
    /// </summary>
    public partial class AddMembershipGroupAgeView : RadWindow
    {
        public readonly AddMembershipGroupAgeViewModel ViewModel;

        public AddMembershipGroupAgeView(MembershipGroupAgeModel membershipGroupAge)
        {
            InitializeComponent();
            if (membershipGroupAge != null)
                Header = "Edit Category Group Age";
            ViewModel = new AddMembershipGroupAgeViewModel(membershipGroupAge);
            DataContext = ViewModel;
            Owner = Application.Current.MainWindow;
        }
        private void OkButton_OnClick(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }
    }
}
