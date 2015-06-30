using System.Windows;
using System.Windows.Controls;
using EventManagementSystem.Models;
using EventManagementSystem.ViewModels.Membership.MembershipTabs;

namespace EventManagementSystem.Views.Membership.MembershipTabs
{
    /// <summary>
    /// Interaction logic for MemberDetailsView.xaml
    /// </summary>
    public partial class MemberDetailsView : UserControl
    {
        private readonly MemberDetailsViewModel _viewModel;

        public MemberDetailsView(MemberModel member)
        {
            InitializeComponent();
            DataContext = _viewModel = new MemberDetailsViewModel(member);
            Loaded += OnMemberDetailsViewLoaded;
        }
        private void OnMemberDetailsViewLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            _viewModel.LoadData();
        }
    }
}
