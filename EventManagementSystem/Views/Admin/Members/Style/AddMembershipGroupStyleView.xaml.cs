using System.Windows;
using EventManagementSystem.Models;
using EventManagementSystem.ViewModels.Admin.Members.Style;
using Telerik.Windows.Controls;

namespace EventManagementSystem.Views.Admin.Members.Style
{
    /// <summary>
    /// Interaction logic for AddMembershipGroupStyleView.xaml
    /// </summary>
    public partial class AddMembershipGroupStyleView : RadWindow
    {

        public readonly AddMembershipGroupStyleViewModel ViewModel;

        public AddMembershipGroupStyleView(MembershipGroupStyleModel membershipGroupStyle)
        {
            InitializeComponent();
            if (membershipGroupStyle != null)
                Header = "Edit Category Group Style";
            ViewModel = new AddMembershipGroupStyleViewModel(membershipGroupStyle);
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
