using System.Collections.Generic;
using System.Windows;
using EventManagementSystem.Models.Custom;
using Telerik.Windows.Controls;
using EventManagementSystem.ViewModels.Events;

namespace EventManagementSystem.Views.Events
{
    /// <summary>
    /// Interaction logic for UpdatesHistoryView.xaml
    /// </summary>
    public partial class UpdatesHistoryView : RadWindow
    {
        public UpdatesHistoryViewModel ViewModel { get; private set; }

        public UpdatesHistoryView(List<UpdatesHistoryModel> updatesHistory)
        {
            InitializeComponent();
            DataContext = ViewModel = new UpdatesHistoryViewModel(updatesHistory);

            Owner = Application.Current.MainWindow;
        }

        private void OkButtonClick(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }
    }
}
