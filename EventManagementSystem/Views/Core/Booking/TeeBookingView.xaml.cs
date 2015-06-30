using System.Windows.Controls;
using EventManagementSystem.ViewModels.Core.Booking;

namespace EventManagementSystem.Views.Core.Booking
{
    /// <summary>
    /// Interaction logic for TeeBookingView.xaml
    /// </summary>
    public partial class TeeBookingView : UserControl
    {
        private readonly TeeBookingViewModel _viewModel;

        public TeeBookingView()
        {
            InitializeComponent();
            DataContext = _viewModel = new TeeBookingViewModel();
        }
    }
}
