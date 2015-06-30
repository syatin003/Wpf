using System.Windows.Controls;
using EventManagementSystem.Models;
using EventManagementSystem.ViewModels.Admin.Resources;

namespace EventManagementSystem.Views.Admin.Resources
{
    /// <summary>
    /// Interaction logic for RoomView.xaml
    /// </summary>
    public partial class RoomView : UserControl
    {
        private readonly RoomViewModel _viewModel;

        public RoomView(RoomModel model)
        {
            InitializeComponent();
            DataContext = _viewModel = new RoomViewModel(model);
            Unloaded += RoomView_Unloaded;
        }

        private void RoomView_Unloaded(object sender, System.Windows.RoutedEventArgs e)
        {
            _viewModel.Refresh();
        }
    }
}
