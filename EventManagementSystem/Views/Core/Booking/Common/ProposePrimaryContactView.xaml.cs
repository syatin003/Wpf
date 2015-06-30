using System.Collections.Generic;
using System.Windows;
using EventManagementSystem.Data.Model;
using EventManagementSystem.ViewModels.Core.Booking.Common;
using Telerik.Windows.Controls;

namespace EventManagementSystem.Views.Core.Booking.Common
{
    /// <summary>
    /// Interaction logic for ProposePrimaryContactView.xaml
    /// </summary>
    public partial class ProposePrimaryContactView : RadWindow
    {
        public ProposePrimaryContactViewModel ViewModel;

        public ProposePrimaryContactView(IEnumerable<Contact> contacts)
        {
            InitializeComponent();
            DataContext = ViewModel = new ProposePrimaryContactViewModel(contacts);

            Owner = Application.Current.MainWindow;
        }

        private void OkButton_OnClick(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }

        private void CancelButton_OnClick(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
