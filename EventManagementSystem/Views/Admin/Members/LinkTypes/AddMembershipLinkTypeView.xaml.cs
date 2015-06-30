using System.Windows;
using EventManagementSystem.Models;
using EventManagementSystem.ViewModels.Admin.Members.LinkTypes;
using Telerik.Windows.Controls;

namespace EventManagementSystem.Views.Admin.Members.LinkTypes
{
    /// <summary>
    /// Interaction logic for AddMembershipLinkTypeView.xaml
    /// </summary>
    public partial class AddMembershipLinkTypeView : RadWindow
    {
        public readonly AddMembershipLinkTypeViewModel ViewModel;

        public AddMembershipLinkTypeView(MembershipLinkTypeModel membershipLinkType)
        {
            InitializeComponent();
            if (membershipLinkType != null)
                Header = "Edit Link Type";
            ViewModel = new AddMembershipLinkTypeViewModel(membershipLinkType);
            DataContext = ViewModel;
            Owner = Application.Current.MainWindow;
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
