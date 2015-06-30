using EventManagementSystem.Models;
using EventManagementSystem.ViewModels.Membership.MembershipTabs;
using System.Windows;

namespace EventManagementSystem.Views.Membership.MembershipTabs
{
    /// <summary>
    /// Interaction logic for MemberUpdateDetailsView.xaml
    /// </summary>
    public partial class MemberUpdateDetailsView
    {
        private readonly MemberUpdateDetailsViewModel _viewModel;

        public MemberUpdateDetailsView(MemberModel member)
        {
            InitializeComponent();
            DataContext = _viewModel = new MemberUpdateDetailsViewModel(member);

            IsVisibleChanged += OnIsVisibleChanged;
        }
        private void OnIsVisibleChanged(object sender, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            if ((bool)dependencyPropertyChangedEventArgs.NewValue)
            {
                _viewModel.LoadData();
            }
        }
    }
}
