using System.Windows;
using EventManagementSystem.Models;
using EventManagementSystem.ViewModels.Core.Contacts;
using Telerik.Windows.Controls;
using System.ComponentModel;

namespace EventManagementSystem.Views.Core.Contacts
{
    /// <summary>
    /// Interaction logic for AddContactView.xaml
    /// </summary>
    public partial class AddContactView : RadWindow
    {
        public readonly AddContactViewModel ViewModel;

        public AddContactView(ContactModel model = null, bool isFromMembership = false)
        {
            InitializeComponent();
            DataContext = ViewModel = new AddContactViewModel(model, isFromMembership);
            ViewModel.PropertyChanged += ViewModelOnPropertyChanged;

            Owner = Application.Current.MainWindow;

            Loaded += OnAddContactViewLoaded;
        }

        private void OnAddContactViewLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            ViewModel.LoadData();
        }
        private void ViewModelOnPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "CloseDialog")
            {
                DialogResult = true;
                Close();
            }
            if (args.PropertyName == "EnableParentWindow")
            {
                IsEnabled = true;
            }
            else if (args.PropertyName == "DisableParentWindow")
            {
                IsEnabled = false;
            }
        }
        private void OnCancelClick(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();

        }
        private async void txtLastName_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!ViewModel.IsOkButtonClick)
            {
                if (txtLastName.Text != string.Empty && txtEmail.Text != string.Empty)
                {
                    if (!ViewModel.IsIgnored)
                        await ViewModel.ShowReleventContact();
                }
            }
        }

        private async void txtEmail_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!ViewModel.IsOkButtonClick)
            {
                if (txtEmail.Text != string.Empty && txtLastName.Text != string.Empty)
                {
                    if (!ViewModel.IsIgnored)
                        await ViewModel.ShowReleventContact();
                }
            }
        }
    }
}
