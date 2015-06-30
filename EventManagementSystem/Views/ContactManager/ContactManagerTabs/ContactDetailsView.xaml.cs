using System.Windows;
using System.Windows.Controls;
using EventManagementSystem.Models;
using EventManagementSystem.ViewModels.ContactManager.ContactManagerTabs;

namespace EventManagementSystem.Views.ContactManager.ContactManagerTabs
{
    /// <summary>
    /// Interaction logic for ContactDetails.xaml
    /// </summary>
    public partial class ContactDetailsView : UserControl
    {
        private readonly ContactDetailsViewModel _viewModel;

        public ContactDetailsView(ContactModel model, bool isFromMembership = false, MemberModel member = null)
        {
            InitializeComponent();
            DataContext = _viewModel = new ContactDetailsViewModel(model, isFromMembership, member);
            Loaded += OnContactDetailsViewLoaded;
        }
        private void OnContactDetailsViewLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            _viewModel.LoadData();
        }
    }
}
