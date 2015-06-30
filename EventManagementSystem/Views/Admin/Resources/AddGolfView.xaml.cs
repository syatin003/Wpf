using System.Windows;
using EventManagementSystem.ViewModels.Admin.Resources;
using Telerik.Windows.Controls;

namespace EventManagementSystem.Views.Admin.Resources
{
    /// <summary>
    /// Interaction logic for AddGolfView.xaml
    /// </summary>
    public partial class AddGolfView : RadWindow
    {
        public readonly AddGolfViewModel ViewModel;

        public AddGolfView()
        {
            InitializeComponent();
            DataContext = ViewModel = new AddGolfViewModel();

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
