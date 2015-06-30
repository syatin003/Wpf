using System.Windows;
using EventManagementSystem.ViewModels.Admin.Resources;
using Telerik.Windows.Controls;

namespace EventManagementSystem.Views.Admin.Resources
{
    /// <summary>
    /// Interaction logic for FacilitiesView.xaml
    /// </summary>
    public partial class FacilitiesView : RadWindow
    {
        public FacilitiesViewModel ViewModel { get; private set; }

        public FacilitiesView()
        {
            InitializeComponent();

            Owner = Application.Current.MainWindow;
            ResizeMode = ResizeMode.NoResize;

            ViewModel = new FacilitiesViewModel();
            DataContext = ViewModel;
        }

        private void Ok_OnClick(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }
    }
}
