using System.Windows;
using EventManagementSystem.Models;
using EventManagementSystem.ViewModels.Admin.Members.Group;
using Telerik.Windows.Controls;

namespace EventManagementSystem.Views.Admin.Members.Group
{
    /// <summary>
    /// Interaction logic for AddMembershipGroupView.xaml
    /// </summary>
    public partial class AddMembershipGroupView : RadWindow
    {
        public readonly AddMembershipGroupViewModel ViewModel;

        public AddMembershipGroupView(MembershipGroupModel membershipGroup)
        {
            InitializeComponent();

            if (membershipGroup != null)
                Header = "Edit Category Group";

            ViewModel = new AddMembershipGroupViewModel(membershipGroup);
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
