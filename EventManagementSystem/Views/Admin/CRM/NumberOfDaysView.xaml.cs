using System.Windows.Controls;
using EventManagementSystem.ViewModels.Admin.CRM;

namespace EventManagementSystem.Views.Admin.CRM
{
    /// <summary>
    /// Interaction logic for NumberOfDaysView.xaml
    /// </summary>
    public partial class NumberOfDaysView : UserControl
    {
        private readonly NumberOfDaysViewModel _viewModel;

        public NumberOfDaysView()
        {
            InitializeComponent();

            DataContext = _viewModel = new NumberOfDaysViewModel();
        }
    }
}
