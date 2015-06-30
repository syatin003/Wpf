using System.Windows;
using System.Windows.Controls;
using EventManagementSystem.ViewModels.Admin.CRM;
using Telerik.Windows;
using Telerik.Windows.Controls;

namespace EventManagementSystem.Views.Admin.CRM
{
    /// <summary>
    /// Interaction logic for CRMView.xaml
    /// </summary>
    public partial class CRMView : UserControl
    {
        private readonly CRMViewModel _viewModel;

        public CRMView()
        {
            InitializeComponent();
            _viewModel = new CRMViewModel();
            DataContext = _viewModel;
            Loaded += OnCrmViewLoaded;
        }

        private void OnCrmViewLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            Loaded -= OnCrmViewLoaded;
            _viewModel.LoadData();
        }

        private void HideRadMenuItem_OnClick(object sender, RadRoutedEventArgs e)
        {
            var item = e.Source as RadMenuItem;

            var btn = item.ParentOfType<RadDropDownButton>();
            if (btn != null)
                btn.IsOpen = false;
        }
    }
}
