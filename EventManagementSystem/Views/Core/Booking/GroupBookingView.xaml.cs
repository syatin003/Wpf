using System.Windows.Controls;
using EventManagementSystem.ViewModels.Core.Booking;

namespace EventManagementSystem.Views.Core.Booking
{
    /// <summary>
    /// Interaction logic for GroupBookingView.xaml
    /// </summary>
    public partial class GroupBookingView : UserControl
    {
        private readonly GroupBookingViewModel _viewModel;

        public GroupBookingView()
        {
            InitializeComponent();
            DataContext = _viewModel = new GroupBookingViewModel();
        }
    }
}
