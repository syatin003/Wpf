using System.ComponentModel;
using System.Windows;
using EventManagementSystem.ViewModels.Core.Contacts;
using Telerik.Windows.Controls;

namespace EventManagementSystem.Views.Core.Contacts
{
    /// <summary>
    /// Interaction logic for ContactsListView.xaml
    /// </summary>
    public partial class ContactsListView : RadWindow
    {
        public readonly ContactsListViewModel ViewModel;

        public ContactsListView(bool isFromMembership = false)
        {
            InitializeComponent();
            DataContext = ViewModel = new ContactsListViewModel(isFromMembership);
            ViewModel.PropertyChanged += ViewModelOnPropertyChanged;

            Owner = Application.Current.MainWindow;

            Loaded += OnContactsListViewLoaded;
        }

        private void ViewModelOnPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "EnableParentWindow")
            {
                this.IsEnabled = true;
            }
            else if (args.PropertyName == "DisableParentWindow")
            {
                this.IsEnabled = false;
            }
            else if (args.PropertyName == "ScrollToSelectedItem")
            {
                ContactListRadGridView.ScrollIntoView(ContactListRadGridView.SelectedItem);
            }
        }

        private void OnContactsListViewLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            ViewModel.LoadData();
        }

        private void OnOkButtonClick(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }
    }
}