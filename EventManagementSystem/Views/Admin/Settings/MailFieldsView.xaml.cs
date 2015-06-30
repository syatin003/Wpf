using System.Windows;
using EventManagementSystem.ViewModels.Admin.Settings;
using Telerik.Windows.Controls;

namespace EventManagementSystem.Views.Admin.Settings
{
    /// <summary>
    /// Interaction logic for MailFieldsView.xaml
    /// </summary>
    public partial class MailFieldsView : RadWindow
    {
        public readonly MailFieldsViewModel ViewModel;

        public MailFieldsView()
        {
            InitializeComponent();
            DataContext = ViewModel = new MailFieldsViewModel();

            Owner = Application.Current.MainWindow;

            Loaded += OnMailFieldsViewLoaded;
        }

        private void OnMailFieldsViewLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            ViewModel.LoadData();
        }

        private void CancelButton_OnClick(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void InsertButton_OnClick(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }
    }
}
