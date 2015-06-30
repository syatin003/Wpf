using System.ComponentModel;
using System.Windows;
using EventManagementSystem.ViewModels.Core.Contacts;
using Telerik.Windows.Controls;
using System.Collections.ObjectModel;
using EventManagementSystem.Models;

namespace EventManagementSystem.Views.Core.Contacts
{
    /// <summary>
    /// Interaction logic for ReleventContactsView.xaml
    /// </summary>
    public partial class ReleventContactsView : RadWindow
    {
        public readonly ReleventContactsViewModel ViewModel;

        public ReleventContactsView(ObservableCollection<ContactModel> contacts)
        {
            InitializeComponent();
            DataContext = ViewModel = new ReleventContactsViewModel(contacts);

            Owner = Application.Current.MainWindow;

        }

        private void OkButtonClick(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }
    }
}