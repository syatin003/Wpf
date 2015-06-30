using System.Collections.Generic;
using System.Windows;
using EventManagementSystem.Models.Custom;
using Telerik.Windows.Controls;
using EventManagementSystem.ViewModels.Membership;

namespace EventManagementSystem.Views.Membership
{
    /// <summary>
    /// Interaction logic for MembershipUpdatesHistoryView.xaml
    /// </summary>
    public partial class MembershipUpdatesHistoryView
    {
        public MembershipUpdatesHistoryViewModel ViewModel { get; private set; }

        public MembershipUpdatesHistoryView(List<MembershipUpdatesHistoryModel> membershipUpdatesHistory)
        {
            InitializeComponent();
            DataContext = ViewModel = new MembershipUpdatesHistoryViewModel(membershipUpdatesHistory);

            Owner = Application.Current.MainWindow;
        }

        private void OkButtonClick(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }
    }
}
