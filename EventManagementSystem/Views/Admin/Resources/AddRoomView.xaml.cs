using System.Windows;
using EventManagementSystem.ViewModels.Admin.Resources;
using Telerik.Windows.Controls;

namespace EventManagementSystem.Views.Admin.Resources
{
    /// <summary>
    /// Interaction logic for AddRoomView.xaml
    /// </summary>
    public partial class AddRoomView : RadWindow
    {
        public readonly AddRoomViewModel ViewModel;

        public AddRoomView()
        {
            InitializeComponent();
            DataContext = ViewModel = new AddRoomViewModel();

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
