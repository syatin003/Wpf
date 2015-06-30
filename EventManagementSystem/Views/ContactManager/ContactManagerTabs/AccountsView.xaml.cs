using System.Windows;
using System.Windows.Controls;
using EventManagementSystem.Models;
using EventManagementSystem.ViewModels.ContactManager.ContactManagerTabs;

namespace EventManagementSystem.Views.ContactManager.ContactManagerTabs
{
    /// <summary>
    /// Interaction logic for AccountsView.xaml
    /// </summary>
    public partial class AccountsView : UserControl
    {
        private readonly AccountsViewModel _viewModel;

        public AccountsView(ContactModel model)
        {
            InitializeComponent();
            DataContext = _viewModel = new AccountsViewModel(model);
            Loaded += OnAccountsViewLoaded;
        }

        private void OnAccountsViewLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            _viewModel.LoadData();
        }
    }
}
